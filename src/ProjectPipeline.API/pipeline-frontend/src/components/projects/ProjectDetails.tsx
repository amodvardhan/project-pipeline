'use client';

import { useState, useEffect } from 'react';
import { useRouter } from 'next/navigation';
import { useAuth } from '@/hooks/useAuth';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { Alert, AlertDescription } from '@/components/ui/alert';
import { 
  ArrowLeft, 
  Edit, 
  Trash2, 
  DollarSign, 
  Calendar, 
  Building, 
  User, 
  Code, 
  Briefcase,
  Clock,
  Target
} from 'lucide-react';
import { Project } from '@/types';
import apiClient from '@/lib/api';
import Link from 'next/link';
import ExportButtons from '@/components/common/ExportButtons';

const statusColors = {
  Pipeline: 'bg-blue-100 text-blue-800',
  Won: 'bg-green-100 text-green-800',
  Lost: 'bg-red-100 text-red-800',
  Missed: 'bg-yellow-100 text-yellow-800',
  OnHold: 'bg-gray-100 text-gray-800',
  Cancelled: 'bg-red-100 text-red-800'
};

interface ProjectDetailsProps {
  projectId: string;
}

export default function ProjectDetails({ projectId }: ProjectDetailsProps) {
  const router = useRouter();
  const { user } = useAuth();
  const [project, setProject] = useState<Project | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [deleteLoading, setDeleteLoading] = useState(false);

  const isAdmin = user?.email === 'admin@projectpipeline.com';
  const canEdit = !!user;
  const canDelete = isAdmin;

  useEffect(() => {
    fetchProject();
  }, [projectId]);

  const fetchProject = async () => {
    try {
      setLoading(true);
      const response = await apiClient.get(`/projects/${projectId}`);
      
      if (response.data.isSuccess && response.data.data) {
        setProject(response.data.data);
      } else {
        setError('Project not found');
      }
    } catch (err: any) {
      setError(err.response?.data?.message || 'Failed to fetch project details');
    } finally {
      setLoading(false);
    }
  };

  const handleDelete = async () => {
    if (!confirm('Are you sure you want to delete this project? This action cannot be undone.')) {
      return;
    }

    try {
      setDeleteLoading(true);
      const response = await apiClient.delete(`/projects/${projectId}`);
      
      if (response.data.isSuccess) {
        router.push('/projects');
      } else {
        setError('Failed to delete project');
      }
    } catch (err: any) {
      setError(err.response?.data?.message || 'Failed to delete project');
    } finally {
      setDeleteLoading(false);
    }
  };

  const formatCurrency = (amount?: number) => {
    if (!amount) return 'N/A';
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD',
      minimumFractionDigits: 0,
      maximumFractionDigits: 0,
    }).format(amount);
  };

  const formatDate = (dateString?: string) => {
    if (!dateString) return 'N/A';
    return new Date(dateString).toLocaleDateString('en-US', {
      year: 'numeric',
      month: 'long',
      day: 'numeric'
    });
  };

  const formatDateTime = (dateString?: string) => {
    if (!dateString) return 'N/A';
    return new Date(dateString).toLocaleString('en-US', {
      year: 'numeric',
      month: 'short',
      day: 'numeric',
      hour: '2-digit',
      minute: '2-digit'
    });
  };

  if (loading) {
    return (
      <div className="flex items-center justify-center h-64">
        <div className="animate-spin rounded-full h-32 w-32 border-b-2 border-gray-900"></div>
      </div>
    );
  }

  if (error || !project) {
    return (
      <div className="max-w-4xl mx-auto space-y-6">
        <Alert variant="destructive">
          <AlertDescription>{error || 'Project not found'}</AlertDescription>
        </Alert>
        <Button onClick={() => router.back()} variant="outline">
          <ArrowLeft className="h-4 w-4 mr-2" />
          Go Back
        </Button>
      </div>
    );
  }

  return (
    <div className="max-w-6xl mx-auto space-y-6">
      {/* Header */}
      <div className="flex items-center justify-between">
        <div className="flex items-center gap-4">
          <Button onClick={() => router.back()} variant="outline" size="sm">
            <ArrowLeft className="h-4 w-4 mr-2" />
            Back to Projects
          </Button>
          <div>
            <h1 className="text-3xl font-bold">{project.name}</h1>
            <p className="text-gray-600">{project.clientName}</p>
          </div>
        </div>
        
        <div className="flex items-center gap-2">
          <Badge className={statusColors[project.status as keyof typeof statusColors] || 'bg-gray-100 text-gray-800'}>
            {project.status}
          </Badge>
          <ExportButtons type="single-project" singleProject={project} />
          {canEdit && (
            <Link href={`/projects/${projectId}/edit`}>
              <Button variant="outline" size="sm">
                <Edit className="h-4 w-4 mr-2" />
                Edit
              </Button>
            </Link>
          )}
          {canDelete && (
            <Button 
              variant="destructive" 
              size="sm" 
              onClick={handleDelete}
              disabled={deleteLoading}
            >
              <Trash2 className="h-4 w-4 mr-2" />
              {deleteLoading ? 'Deleting...' : 'Delete'}
            </Button>
          )}
        </div>
      </div>

      {/* Main Content */}
      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        {/* Left Column - Main Details */}
        <div className="lg:col-span-2 space-y-6">
          {/* Project Overview */}
          <Card>
            <CardHeader>
              <CardTitle className="flex items-center gap-2">
                <Briefcase className="h-5 w-5" />
                Project Overview
              </CardTitle>
            </CardHeader>
            <CardContent className="space-y-4">
              {project.description && (
                <div>
                  <h4 className="font-semibold mb-2">Description</h4>
                  <p className="text-gray-700">{project.description}</p>
                </div>
              )}
              
              <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                <div>
                  <h4 className="font-semibold mb-2">Project Type</h4>
                  <p className="text-gray-700">{project.projectType}</p>
                </div>
                <div>
                  <h4 className="font-semibold mb-2">Business Unit</h4>
                  <p className="text-gray-700">{project.businessUnitName}</p>
                </div>
              </div>

              {project.statusReason && (
                <div>
                  <h4 className="font-semibold mb-2">Status Reason</h4>
                  <p className="text-gray-700">{project.statusReason}</p>
                </div>
              )}
            </CardContent>
          </Card>

          {/* Technology Stack */}
          <Card>
            <CardHeader>
              <CardTitle className="flex items-center gap-2">
                <Code className="h-5 w-5" />
                Technology Stack
              </CardTitle>
            </CardHeader>
            <CardContent>
              <div className="flex flex-wrap gap-2">
                {project.technology?.split(',').map((tech, index) => (
                  <Badge key={index} variant="secondary">
                    {tech.trim()}
                  </Badge>
                ))}
              </div>
            </CardContent>
          </Card>

          {/* Resource Metrics */}
          <Card>
            <CardHeader>
              <CardTitle className="flex items-center gap-2">
                <Target className="h-5 w-5" />
                Resource Metrics
              </CardTitle>
            </CardHeader>
            <CardContent>
              <div className="grid grid-cols-3 gap-4">
                <div className="text-center">
                  <div className="text-2xl font-bold text-blue-600">{project.profilesSubmitted}</div>
                  <div className="text-sm text-gray-600">Profiles Submitted</div>
                </div>
                <div className="text-center">
                  <div className="text-2xl font-bold text-yellow-600">{project.profilesShortlisted}</div>
                  <div className="text-sm text-gray-600">Shortlisted</div>
                </div>
                <div className="text-center">
                  <div className="text-2xl font-bold text-green-600">{project.profilesSelected}</div>
                  <div className="text-sm text-gray-600">Selected</div>
                </div>
              </div>
            </CardContent>
          </Card>
        </div>

        {/* Right Column - Key Information */}
        <div className="space-y-6">
          {/* Financial Information */}
          <Card>
            <CardHeader>
              <CardTitle className="flex items-center gap-2">
                <DollarSign className="h-5 w-5" />
                Financial Details
              </CardTitle>
            </CardHeader>
            <CardContent className="space-y-4">
              <div>
                <div className="text-sm text-gray-600">Estimated Value</div>
                <div className="text-xl font-bold text-green-600">
                  {formatCurrency(project.estimatedValue)}
                </div>
              </div>
              
              {project.actualValue && (
                <div>
                  <div className="text-sm text-gray-600">Actual Value</div>
                  <div className="text-xl font-bold text-blue-600">
                    {formatCurrency(project.actualValue)}
                  </div>
                </div>
              )}
            </CardContent>
          </Card>

          {/* Timeline */}
          <Card>
            <CardHeader>
              <CardTitle className="flex items-center gap-2">
                <Calendar className="h-5 w-5" />
                Timeline
              </CardTitle>
            </CardHeader>
            <CardContent className="space-y-4">
              <div>
                <div className="text-sm text-gray-600">Expected Closure</div>
                <div className="font-semibold">{formatDate(project.expectedClosureDate)}</div>
              </div>
              
              {project.startDate && (
                <div>
                  <div className="text-sm text-gray-600">Start Date</div>
                  <div className="font-semibold">{formatDate(project.startDate)}</div>
                </div>
              )}
              
              {project.endDate && (
                <div>
                  <div className="text-sm text-gray-600">End Date</div>
                  <div className="font-semibold">{formatDate(project.endDate)}</div>
                </div>
              )}
            </CardContent>
          </Card>

          {/* Project Meta */}
          <Card>
            <CardHeader>
              <CardTitle className="flex items-center gap-2">
                <Clock className="h-5 w-5" />
                Project Information
              </CardTitle>
            </CardHeader>
            <CardContent className="space-y-4">
              <div>
                <div className="text-sm text-gray-600">Created By</div>
                <div className="font-semibold">{project.createdByName || 'Unknown'}</div>
              </div>
              
              <div>
                <div className="text-sm text-gray-600">Created On</div>
                <div className="font-semibold">{formatDateTime(project.createdAt)}</div>
              </div>
              
              <div>
                <div className="text-sm text-gray-600">Project ID</div>
                <div className="font-mono text-sm">{project.id}</div>
              </div>
            </CardContent>
          </Card>
        </div>
      </div>
    </div>
  );
}
