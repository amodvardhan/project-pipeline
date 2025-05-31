'use client';

import { useState, useEffect } from 'react';
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
import { ArrowLeft, Save, DollarSign, Building, Code, Briefcase } from 'lucide-react';
import { Project } from '@/types';
import apiClient from '@/lib/api';

interface EditProjectFormProps {
  projectId: string;
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

const projectStatuses = [
  { value: 1, label: 'Pipeline' },
  { value: 2, label: 'Won' },
  { value: 3, label: 'Lost' },
  { value: 4, label: 'Missed' },
  { value: 5, label: 'OnHold' },
  { value: 6, label: 'Cancelled' }
];

export default function EditProjectForm({ projectId }: EditProjectFormProps) {
  const router = useRouter();
  const { user } = useAuth();
  const { businessUnits } = useBusinessUnits();
  
  const [project, setProject] = useState<Project | null>(null);
  const [formData, setFormData] = useState({
    name: '',
    description: '',
    clientName: '',
    estimatedValue: '',
    actualValue: '',
    status: '',
    statusReason: '',
    businessUnitId: '',
    technology: '',
    projectType: '',
    expectedClosureDate: '',
    startDate: '',
    endDate: ''
  });
  
  const [loading, setLoading] = useState(true);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');

  useEffect(() => {
    fetchProject();
  }, [projectId]);

  const fetchProject = async () => {
    try {
      setLoading(true);
      const response = await apiClient.get(`/projects/${projectId}`);
      
      if (response.data.isSuccess && response.data.data) {
        const proj = response.data.data;
        setProject(proj);
        
        // Map project status to enum value
        const statusMap: { [key: string]: string } = {
          'Pipeline': '1',
          'Won': '2',
          'Lost': '3',
          'Missed': '4',
          'OnHold': '5',
          'Cancelled': '6'
        };
        
        setFormData({
          name: proj.name || '',
          description: proj.description || '',
          clientName: proj.clientName || '',
          estimatedValue: proj.estimatedValue?.toString() || '',
          actualValue: proj.actualValue?.toString() || '',
          status: statusMap[proj.status] || '1',
          statusReason: proj.statusReason || '',
          businessUnitId: proj.businessUnitId?.toString() || '',
          technology: proj.technology || '',
          projectType: proj.projectType || '',
          expectedClosureDate: proj.expectedClosureDate ? proj.expectedClosureDate.split('T')[0] : '',
          startDate: proj.startDate ? proj.startDate.split('T')[0] : '',
          endDate: proj.endDate ? proj.endDate.split('T')[0] : ''
        });
      } else {
        setError('Project not found');
      }
    } catch (err: any) {
      setError(err.response?.data?.message || 'Failed to fetch project details');
    } finally {
      setLoading(false);
    }
  };

  const handleInputChange = (field: string, value: string) => {
    setFormData(prev => ({
      ...prev,
      [field]: value
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
    if (formData.actualValue && isNaN(Number(formData.actualValue))) {
      setError('Actual value must be a valid number');
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

    setSaving(true);

    try {
      const updateData = {
        name: formData.name.trim(),
        description: formData.description.trim(),
        clientName: formData.clientName.trim(),
        estimatedValue: formData.estimatedValue ? parseFloat(formData.estimatedValue) : undefined,
        actualValue: formData.actualValue ? parseFloat(formData.actualValue) : undefined,
        status: parseInt(formData.status),
        statusReason: formData.statusReason.trim(),
        technology: formData.technology || 'Not specified',
        projectType: formData.projectType,
        expectedClosureDate: formData.expectedClosureDate || undefined,
        startDate: formData.startDate || undefined,
        endDate: formData.endDate || undefined,
        updatedBy: user?.id
      };

      const response = await apiClient.put(`/projects/${projectId}`, updateData);

      if (response.data.isSuccess) {
        setSuccess('Project updated successfully!');
        setTimeout(() => {
          router.push(`/projects/${projectId}`);
        }, 2000);
      } else {
        setError(response.data.message || 'Failed to update project');
      }
    } catch (err: any) {
      setError(err.response?.data?.message || 'An error occurred while updating the project');
    } finally {
      setSaving(false);
    }
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center h-64">
        <div className="animate-spin rounded-full h-32 w-32 border-b-2 border-gray-900"></div>
      </div>
    );
  }

  if (error && !project) {
    return (
      <div className="max-w-4xl mx-auto space-y-6">
        <Alert variant="destructive">
          <AlertDescription>{error}</AlertDescription>
        </Alert>
        <Button onClick={() => router.back()} variant="outline">
          <ArrowLeft className="h-4 w-4 mr-2" />
          Go Back
        </Button>
      </div>
    );
  }

  return (
    <div className="max-w-4xl mx-auto space-y-6">
      {/* Header */}
      <div className="flex items-center justify-between">
        <div className="flex items-center gap-4">
          <Button onClick={() => router.back()} variant="outline" size="sm">
            <ArrowLeft className="h-4 w-4 mr-2" />
            Back
          </Button>
          <div>
            <h1 className="text-3xl font-bold">Edit Project</h1>
            <p className="text-gray-600">{project?.name}</p>
          </div>
        </div>
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
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div className="space-y-2">
                <Label htmlFor="name">Project Name *</Label>
                <Input
                  id="name"
                  value={formData.name}
                  onChange={(e) => handleInputChange('name', e.target.value)}
                  required
                />
              </div>
              
              <div className="space-y-2">
                <Label htmlFor="clientName">Client Name *</Label>
                <Input
                  id="clientName"
                  value={formData.clientName}
                  onChange={(e) => handleInputChange('clientName', e.target.value)}
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
                rows={3}
              />
            </div>
          </CardContent>
        </Card>

        {/* Project Status */}
        <Card>
          <CardHeader>
            <CardTitle>Project Status & Details</CardTitle>
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div className="space-y-2">
                <Label htmlFor="status">Status *</Label>
                <Select value={formData.status} onValueChange={(value) => handleInputChange('status', value)}>
                  <SelectTrigger>
                    <SelectValue placeholder="Select status" />
                  </SelectTrigger>
                  <SelectContent>
                    {projectStatuses.map((status) => (
                      <SelectItem key={status.value} value={status.value.toString()}>
                        {status.label}
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

            <div className="space-y-2">
              <Label htmlFor="statusReason">Status Reason</Label>
              <Textarea
                id="statusReason"
                value={formData.statusReason}
                onChange={(e) => handleInputChange('statusReason', e.target.value)}
                placeholder="Explain why the project has this status..."
                rows={2}
              />
            </div>

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
          </CardContent>
        </Card>

        {/* Financial Information */}
        <Card>
          <CardHeader>
            <CardTitle className="flex items-center gap-2">
              <DollarSign className="h-5 w-5" />
              Financial Information
            </CardTitle>
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div className="space-y-2">
                <Label htmlFor="estimatedValue">Estimated Value (USD)</Label>
                <Input
                  id="estimatedValue"
                  type="number"
                  value={formData.estimatedValue}
                  onChange={(e) => handleInputChange('estimatedValue', e.target.value)}
                  placeholder="0"
                />
              </div>

              <div className="space-y-2">
                <Label htmlFor="actualValue">Actual Value (USD)</Label>
                <Input
                  id="actualValue"
                  type="number"
                  value={formData.actualValue}
                  onChange={(e) => handleInputChange('actualValue', e.target.value)}
                  placeholder="0"
                />
              </div>
            </div>
          </CardContent>
        </Card>

        {/* Timeline */}
        <Card>
          <CardHeader>
            <CardTitle>Timeline</CardTitle>
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
              <div className="space-y-2">
                <Label htmlFor="startDate">Start Date</Label>
                <Input
                  id="startDate"
                  type="date"
                  value={formData.startDate}
                  onChange={(e) => handleInputChange('startDate', e.target.value)}
                />
              </div>

              <div className="space-y-2">
                <Label htmlFor="expectedClosureDate">Expected Closure Date</Label>
                <Input
                  id="expectedClosureDate"
                  type="date"
                  value={formData.expectedClosureDate}
                  onChange={(e) => handleInputChange('expectedClosureDate', e.target.value)}
                />
              </div>

              <div className="space-y-2">
                <Label htmlFor="endDate">End Date</Label>
                <Input
                  id="endDate"
                  type="date"
                  value={formData.endDate}
                  onChange={(e) => handleInputChange('endDate', e.target.value)}
                />
              </div>
            </div>
          </CardContent>
        </Card>

        {/* Technology */}
        <Card>
          <CardHeader>
            <CardTitle className="flex items-center gap-2">
              <Code className="h-5 w-5" />
              Technology Stack
            </CardTitle>
          </CardHeader>
          <CardContent>
            <div className="space-y-2">
              <Label htmlFor="technology">Technologies</Label>
              <Input
                id="technology"
                value={formData.technology}
                onChange={(e) => handleInputChange('technology', e.target.value)}
                placeholder="e.g., React, Node.js, MongoDB"
              />
              <p className="text-sm text-gray-500">
                Separate multiple technologies with commas
              </p>
            </div>
          </CardContent>
        </Card>

        {/* Submit Buttons */}
        <div className="flex justify-end space-x-4">
          <Button type="button" variant="outline" onClick={() => router.back()}>
            Cancel
          </Button>
          <Button type="submit" disabled={saving}>
            <Save className="h-4 w-4 mr-2" />
            {saving ? 'Saving...' : 'Save Changes'}
          </Button>
        </div>
      </form>
    </div>
  );
}
