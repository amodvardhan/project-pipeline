'use client';

import { useState } from 'react';
import { useRouter } from 'next/navigation';
import { useAuth } from '@/hooks/useAuth';
import { Button } from '@/components/ui/button';
import { Input } from '@/components/ui/input';
import { Label } from '@/components/ui/label';
import { Textarea } from '@/components/ui/textarea';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Alert, AlertDescription } from '@/components/ui/alert';
import { ArrowLeft, Save, User, Mail, Phone, Briefcase, Code, DollarSign } from 'lucide-react';
import apiClient from '@/lib/api';

interface AddProfileFormProps {
  projectId: number;
  projectName: string;
}

interface FormData {
  candidateName: string;
  candidateEmail: string;
  candidatePhone: string;
  position: string;
  technology: string;
  experienceYears: string;
  expectedSalary: string;
  resumePath: string;
  initialComments: string;
}

export default function AddProfileForm({ projectId, projectName }: AddProfileFormProps) {
  const router = useRouter();
  const { user } = useAuth();
  
  const [formData, setFormData] = useState<FormData>({
    candidateName: '',
    candidateEmail: '',
    candidatePhone: '',
    position: '',
    technology: '',
    experienceYears: '',
    expectedSalary: '',
    resumePath: '',
    initialComments: ''
  });
  
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');

  const handleInputChange = (field: keyof FormData, value: string) => {
    setFormData(prev => ({
      ...prev,
      [field]: value
    }));
  };

  const validateForm = (): boolean => {
    if (!formData.candidateName.trim()) {
      setError('Candidate name is required');
      return false;
    }
    if (!formData.candidateEmail.trim()) {
      setError('Candidate email is required');
      return false;
    }
    if (!formData.position.trim()) {
      setError('Position is required');
      return false;
    }
    if (!formData.technology.trim()) {
      setError('Technology is required');
      return false;
    }
    if (!formData.experienceYears || isNaN(Number(formData.experienceYears))) {
      setError('Valid experience years is required');
      return false;
    }
    if (formData.expectedSalary && isNaN(Number(formData.expectedSalary))) {
      setError('Expected salary must be a valid number');
      return false;
    }
    
    // Email validation
    const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    if (!emailRegex.test(formData.candidateEmail)) {
      setError('Please enter a valid email address');
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
      const profileData = {
        projectId: projectId,
        candidateName: formData.candidateName.trim(),
        candidateEmail: formData.candidateEmail.trim(),
        candidatePhone: formData.candidatePhone.trim() || undefined,
        position: formData.position.trim(),
        technology: formData.technology.trim(),
        experienceYears: parseInt(formData.experienceYears),
        expectedSalary: formData.expectedSalary ? parseFloat(formData.expectedSalary) : undefined,
        resumePath: formData.resumePath.trim() || undefined,
        initialComments: formData.initialComments.trim() || undefined
      };

      const response = await apiClient.post('/profile-submissions', profileData);

      if (response.data.isSuccess) {
        setSuccess('Profile submitted successfully!');
        setTimeout(() => {
          router.push(`/projects/${projectId}/profiles`);
        }, 2000);
      } else {
        setError(response.data.message || 'Failed to submit profile');
      }
    } catch (err: any) {
      setError(err.response?.data?.message || 'An error occurred while submitting the profile');
    } finally {
      setLoading(false);
    }
  };

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
            <h1 className="text-3xl font-bold">Add Profile Submission</h1>
            <p className="text-gray-600">Submit a candidate profile for {projectName}</p>
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

        {/* Candidate Information */}
        <Card>
          <CardHeader>
            <CardTitle className="flex items-center gap-2">
              <User className="h-5 w-5" />
              Candidate Information
            </CardTitle>
            <CardDescription>
              Enter the candidate's basic details
            </CardDescription>
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div className="space-y-2">
                <Label htmlFor="candidateName">Full Name *</Label>
                <Input
                  id="candidateName"
                  value={formData.candidateName}
                  onChange={(e) => handleInputChange('candidateName', e.target.value)}
                  placeholder="Enter candidate's full name"
                  required
                />
              </div>
              
              <div className="space-y-2">
                <Label htmlFor="candidateEmail">Email Address *</Label>
                <div className="relative">
                  <Mail className="absolute left-3 top-3 h-4 w-4 text-gray-400" />
                  <Input
                    id="candidateEmail"
                    type="email"
                    value={formData.candidateEmail}
                    onChange={(e) => handleInputChange('candidateEmail', e.target.value)}
                    placeholder="candidate@email.com"
                    className="pl-10"
                    required
                  />
                </div>
              </div>
            </div>

            <div className="space-y-2">
              <Label htmlFor="candidatePhone">Phone Number</Label>
              <div className="relative">
                <Phone className="absolute left-3 top-3 h-4 w-4 text-gray-400" />
                <Input
                  id="candidatePhone"
                  type="tel"
                  value={formData.candidatePhone}
                  onChange={(e) => handleInputChange('candidatePhone', e.target.value)}
                  placeholder="+1 (555) 123-4567"
                  className="pl-10"
                />
              </div>
            </div>
          </CardContent>
        </Card>

        {/* Position Details */}
        <Card>
          <CardHeader>
            <CardTitle className="flex items-center gap-2">
              <Briefcase className="h-5 w-5" />
              Position Details
            </CardTitle>
            <CardDescription>
              Specify the role and technical requirements
            </CardDescription>
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div className="space-y-2">
                <Label htmlFor="position">Position *</Label>
                <Input
                  id="position"
                  value={formData.position}
                  onChange={(e) => handleInputChange('position', e.target.value)}
                  placeholder="e.g., Senior Software Engineer"
                  required
                />
              </div>
              
              <div className="space-y-2">
                <Label htmlFor="experienceYears">Experience (Years) *</Label>
                <Input
                  id="experienceYears"
                  type="number"
                  min="0"
                  max="50"
                  value={formData.experienceYears}
                  onChange={(e) => handleInputChange('experienceYears', e.target.value)}
                  placeholder="5"
                  required
                />
              </div>
            </div>

            <div className="space-y-2">
              <Label htmlFor="technology">Technology Stack *</Label>
              <div className="relative">
                <Code className="absolute left-3 top-3 h-4 w-4 text-gray-400" />
                <Input
                  id="technology"
                  value={formData.technology}
                  onChange={(e) => handleInputChange('technology', e.target.value)}
                  placeholder="e.g., React, Node.js, TypeScript"
                  className="pl-10"
                  required
                />
              </div>
            </div>
          </CardContent>
        </Card>

        {/* Compensation & Documents */}
        <Card>
          <CardHeader>
            <CardTitle className="flex items-center gap-2">
              <DollarSign className="h-5 w-5" />
              Compensation & Documents
            </CardTitle>
            <CardDescription>
              Salary expectations and document links
            </CardDescription>
          </CardHeader>
          <CardContent className="space-y-4">
            <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
              <div className="space-y-2">
                <Label htmlFor="expectedSalary">Expected Salary (USD)</Label>
                <div className="relative">
                  <DollarSign className="absolute left-3 top-3 h-4 w-4 text-gray-400" />
                  <Input
                    id="expectedSalary"
                    type="number"
                    min="0"
                    value={formData.expectedSalary}
                    onChange={(e) => handleInputChange('expectedSalary', e.target.value)}
                    placeholder="75000"
                    className="pl-10"
                  />
                </div>
              </div>
              
              <div className="space-y-2">
                <Label htmlFor="resumePath">Resume Link/Path</Label>
                <Input
                  id="resumePath"
                  value={formData.resumePath}
                  onChange={(e) => handleInputChange('resumePath', e.target.value)}
                  placeholder="https://drive.google.com/... or /documents/resume.pdf"
                />
              </div>
            </div>
          </CardContent>
        </Card>

        {/* Additional Comments */}
        <Card>
          <CardHeader>
            <CardTitle>Additional Comments</CardTitle>
            <CardDescription>
              Any additional notes about the candidate
            </CardDescription>
          </CardHeader>
          <CardContent>
            <div className="space-y-2">
              <Label htmlFor="initialComments">Comments</Label>
              <Textarea
                id="initialComments"
                value={formData.initialComments}
                onChange={(e) => handleInputChange('initialComments', e.target.value)}
                placeholder="Any additional information about the candidate..."
                rows={4}
              />
            </div>
          </CardContent>
        </Card>

        {/* Submit Buttons */}
        <div className="flex justify-end space-x-4">
          <Button type="button" variant="outline" onClick={() => router.back()}>
            Cancel
          </Button>
          <Button type="submit" disabled={loading}>
            <Save className="h-4 w-4 mr-2" />
            {loading ? 'Submitting...' : 'Submit Profile'}
          </Button>
        </div>
      </form>
    </div>
  );
}
