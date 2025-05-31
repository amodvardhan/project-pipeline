'use client';

import { useState } from 'react';
import { useRouter } from 'next/navigation';
import { useAuth } from '@/hooks/useAuth';
import { useBusinessUnits } from '@/hooks/useProjects';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { Textarea } from '@/components/ui/textarea';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Alert, AlertDescription } from '@/components/ui/alert';
import { CalendarIcon, DollarSign, Building, Code, Briefcase } from 'lucide-react';
import apiClient from '@/lib/api';

interface CreateProjectData {
  name: string;
  description: string;
  clientName: string;
  estimatedValue: string;
  businessUnitId: string;
  technology: string;
  projectType: string;
  expectedClosureDate: string;
}

const projectTypes = [
  'Development',
  'Migration',
  'Analytics',
  'Mobile Development',
  'Consulting',
  'Support & Maintenance',
  'Infrastructure',
  'Integration'
];

const technologies = [
  '.NET Core',
  'React',
  'Angular',
  'Vue.js',
  'Node.js',
  'Python',
  'Java',
  'Azure',
  'AWS',
  'Docker',
  'Kubernetes',
  'SQL Server',
  'MongoDB',
  'PostgreSQL'
];

export default function AddProjectForm() {
  const router = useRouter();
  const { user } = useAuth();
  const { businessUnits } = useBusinessUnits();
  
  const [formData, setFormData] = useState<CreateProjectData>({
    name: '',
    description: '',
    clientName: '',
    estimatedValue: '',
    businessUnitId: '',
    technology: '',
    projectType: '',
    expectedClosureDate: ''
  });
  
  const [selectedTechnologies, setSelectedTechnologies] = useState<string[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');

  const handleInputChange = (field: keyof CreateProjectData, value: string) => {
    setFormData(prev => ({
      ...prev,
      [field]: value
    }));
  };

  const addTechnology = (tech: string) => {
    if (!selectedTechnologies.includes(tech)) {
      const newTechs = [...selectedTechnologies, tech];
      setSelectedTechnologies(newTechs);
      setFormData(prev => ({
        ...prev,
        technology: newTechs.join(', ')
      }));
    }
  };

  const removeTechnology = (tech: string) => {
    const newTechs = selectedTechnologies.filter(t => t !== tech);
    setSelectedTechnologies(newTechs);
    setFormData(prev => ({
      ...prev,
      technology: newTechs.join(', ')
    }));
  };

  const validateForm = (): boolean => {
    if (!formData.name.trim()) {
      setError('Project name is required');
      return false;
    }
    if (!formData.clientName.trim()) {
      setError('Client name is required');
      return false;
    }
    if (!formData.businessUnitId) {
      setError('Business unit is required');
      return false;
    }
    if (!formData.projectType) {
      setError('Project type is required');
      return false;
    }
    if (formData.estimatedValue && isNaN(Number(formData.estimatedValue))) {
      setError('Estimated value must be a valid number');
      return false;
    }
    return true;
  };

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setSuccess('');

    if (!validateForm()) {
      return;
    }

    setLoading(true);

    try {
      const projectData = {
        name: formData.name.trim(),
        description: formData.description.trim(),
        clientName: formData.clientName.trim(),
        estimatedValue: formData.estimatedValue ? parseFloat(formData.estimatedValue) : undefined,
        businessUnitId: parseInt(formData.businessUnitId),
        technology: formData.technology || 'Not specified',
        projectType: formData.projectType,
        expectedClosureDate: formData.expectedClosureDate || undefined,
        createdBy: user?.id
      };

      const response = await apiClient.post('/projects', projectData);

      if (response.data.isSuccess) {
        setSuccess('Project created successfully!');
        setTimeout(() => {
          router.push('/projects');
        }, 2000);
      } else {
        setError(response.data.message || 'Failed to create project');
      }
    } catch (err: any) {
      setError(err.response?.data?.message || 'An error occurred while creating the project');
    } finally {
      setLoading(false);
    }
  };

  const formatCurrency = (value: string) => {
    const num = parseFloat(value.replace(/[^0-9.]/g, ''));
    if (isNaN(num)) return '';
    return new Intl.NumberFormat('en-US').format(num);
  };

  return (
    <div className="max-w-4xl mx-auto space-y-6">
      {/* Header */}
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-3xl font-bold">Add New Project</h1>
          <p className="text-gray-600">Create a new project opportunity</p>
        </div>
        <Button variant="outline" onClick={() => router.back()}>
          Cancel
        </Button>
      </div>

      {/* Form */}
      <form onSubmit={handleSubmit} className="space-y-6">
        {/* Alerts */}
        {error && (
          <Alert variant="destructive">
            <AlertDescription>{error}</AlertDescription>
          </Alert>
        )}
        
        {success && (
          <Alert className="border-green-200 bg-green-50">
            <AlertDescription className="text-green-800">{success}</AlertDescription>
          </Alert>
        )}

        {/* Basic Information */}
        <Card>
          <CardHeader>
            <CardTitle className="flex items-center gap-2">
              <Briefcase className="h-5 w-5" />
              Basic Information
            </CardTitle>
            <CardDescription>
              Enter the basic details about the project
            </CardDescription>
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div className="space-y-2">
                <Label htmlFor="name">Project Name *</Label>
                <Input
                  id="name"
                  value={formData.name}
                  onChange={(e) => handleInputChange('name', e.target.value)}
                  placeholder="Enter project name"
                  required
                />
              </div>
              
              <div className="space-y-2">
                <Label htmlFor="clientName">Client Name *</Label>
                <Input
                  id="clientName"
                  value={formData.clientName}
                  onChange={(e) => handleInputChange('clientName', e.target.value)}
                  placeholder="Enter client name"
                  required
                />
              </div>
            </div>

            <div className="space-y-2">
              <Label htmlFor="description">Description</Label>
              <Textarea
                id="description"
                value={formData.description}
                onChange={(e) => handleInputChange('description', e.target.value)}
                placeholder="Enter project description"
                rows={3}
              />
            </div>
          </CardContent>
        </Card>

        {/* Project Details */}
        <Card>
          <CardHeader>
            <CardTitle className="flex items-center gap-2">
              <Building className="h-5 w-5" />
              Project Details
            </CardTitle>
            <CardDescription>
              Specify project categorization and business unit
            </CardDescription>
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div className="space-y-2">
                <Label htmlFor="businessUnit">Business Unit *</Label>
                <Select value={formData.businessUnitId} onValueChange={(value) => handleInputChange('businessUnitId', value)}>
                  <SelectTrigger>
                    <SelectValue placeholder="Select business unit" />
                  </SelectTrigger>
                  <SelectContent>
                    {businessUnits.map((bu) => (
                      <SelectItem key={bu.id} value={bu.id.toString()}>
                        {bu.name} ({bu.code})
                      </SelectItem>
                    ))}
                  </SelectContent>
                </Select>
              </div>

              <div className="space-y-2">
                <Label htmlFor="projectType">Project Type *</Label>
                <Select value={formData.projectType} onValueChange={(value) => handleInputChange('projectType', value)}>
                  <SelectTrigger>
                    <SelectValue placeholder="Select project type" />
                  </SelectTrigger>
                  <SelectContent>
                    {projectTypes.map((type) => (
                      <SelectItem key={type} value={type}>
                        {type}
                      </SelectItem>
                    ))}
                  </SelectContent>
                </Select>
              </div>
            </div>
          </CardContent>
        </Card>

        {/* Financial & Timeline */}
        <Card>
          <CardHeader>
            <CardTitle className="flex items-center gap-2">
              <DollarSign className="h-5 w-5" />
              Financial & Timeline
            </CardTitle>
            <CardDescription>
              Set estimated value and expected timeline
            </CardDescription>
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div className="space-y-2">
                <Label htmlFor="estimatedValue">Estimated Value (USD)</Label>
                <div className="relative">
                  <DollarSign className="absolute left-3 top-3 h-4 w-4 text-gray-400" />
                  <Input
                    id="estimatedValue"
                    type="text"
                    value={formData.estimatedValue}
                    onChange={(e) => {
                      const value = e.target.value.replace(/[^0-9.]/g, '');
                      handleInputChange('estimatedValue', value);
                    }}
                    placeholder="0"
                    className="pl-10"
                  />
                </div>
                {formData.estimatedValue && (
                  <p className="text-sm text-gray-500">
                    Formatted: ${formatCurrency(formData.estimatedValue)}
                  </p>
                )}
              </div>

              <div className="space-y-2">
                <Label htmlFor="expectedClosureDate">Expected Closure Date</Label>
                <div className="relative">
                  <CalendarIcon className="absolute left-3 top-3 h-4 w-4 text-gray-400" />
                  <Input
                    id="expectedClosureDate"
                    type="date"
                    value={formData.expectedClosureDate}
                    onChange={(e) => handleInputChange('expectedClosureDate', e.target.value)}
                    className="pl-10"
                  />
                </div>
              </div>
            </div>
          </CardContent>
        </Card>

        {/* Technology Stack */}
        <Card>
          <CardHeader>
            <CardTitle className="flex items-center gap-2">
              <Code className="h-5 w-5" />
              Technology Stack
            </CardTitle>
            <CardDescription>
              Select technologies that will be used in this project
            </CardDescription>
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="space-y-2">
              <Label>Add Technologies</Label>
              <Select onValueChange={addTechnology}>
                <SelectTrigger>
                  <SelectValue placeholder="Select technologies" />
                </SelectTrigger>
                <SelectContent>
                  {technologies.filter(tech => !selectedTechnologies.includes(tech)).map((tech) => (
                    <SelectItem key={tech} value={tech}>
                      {tech}
                    </SelectItem>
                  ))}
                </SelectContent>
              </Select>
            </div>

            {selectedTechnologies.length > 0 && (
              <div className="space-y-2">
                <Label>Selected Technologies</Label>
                <div className="flex flex-wrap gap-2">
                  {selectedTechnologies.map((tech) => (
                    <div
                      key={tech}
                      className="flex items-center gap-1 bg-blue-100 text-blue-800 px-2 py-1 rounded-md text-sm"
                    >
                      {tech}
                      <button
                        type="button"
                        onClick={() => removeTechnology(tech)}
                        className="ml-1 text-blue-600 hover:text-blue-800"
                      >
                        ×
                      </button>
                    </div>
                  ))}
                </div>
              </div>
            )}

            <div className="space-y-2">
              <Label htmlFor="customTech">Or enter custom technologies</Label>
              <Input
                id="customTech"
                value={formData.technology}
                onChange={(e) => {
                  handleInputChange('technology', e.target.value);
                  setSelectedTechnologies(e.target.value.split(',').map(t => t.trim()).filter(t => t));
                }}
                placeholder="e.g., React, Node.js, MongoDB"
              />
              <p className="text-sm text-gray-500">
                Separate multiple technologies with commas
              </p>
            </div>
          </CardContent>
        </Card>

        {/* Submit Button */}
        <div className="flex justify-end space-x-4">
          <Button type="button" variant="outline" onClick={() => router.back()}>
            Cancel
          </Button>
          <Button type="submit" disabled={loading}>
            {loading ? 'Creating Project...' : 'Create Project'}
          </Button>
        </div>
      </form>
    </div>
  );
}
