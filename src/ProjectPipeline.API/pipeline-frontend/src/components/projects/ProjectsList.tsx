'use client';

import { useState } from 'react';
import { useProjects, useBusinessUnits } from '@/hooks/useProjects';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select';
import { Input } from '@/components/ui/input';
import { Search, Filter, Plus, DollarSign, Calendar, Building } from 'lucide-react';
import { Project } from '@/types';
import Link from 'next/link';
import { useAuth } from '@/hooks/useAuth';
import ExportButtons from '@/components/common/ExportButtons';

const statusColors = {
  Pipeline: 'bg-blue-100 text-blue-800',
  Won: 'bg-green-100 text-green-800',
  Lost: 'bg-red-100 text-red-800',
  Missed: 'bg-yellow-100 text-yellow-800',
  OnHold: 'bg-gray-100 text-gray-800',
  Cancelled: 'bg-red-100 text-red-800'
};

export default function ProjectsList() {
  const { user } = useAuth();
  const [currentPage, setCurrentPage] = useState(1);
  const [statusFilter, setStatusFilter] = useState('all');
  const [businessUnitFilter, setBusinessUnitFilter] = useState<string>('all');
  const [searchTerm, setSearchTerm] = useState('');
  
  const { projects, loading, error, totalCount, totalPages, refreshProjects } = useProjects(
    currentPage, 
    10, 
    statusFilter === 'all' ? undefined : statusFilter,
    businessUnitFilter === 'all' ? undefined : parseInt(businessUnitFilter)
  );
  
  const { businessUnits } = useBusinessUnits();

  const isAdmin = user?.email === 'admin@projectpipeline.com';
  const canCreateProjects = !!user;

  const filteredProjects = (projects || []).filter(project =>
    project?.name?.toLowerCase().includes(searchTerm.toLowerCase()) ||
    project?.clientName?.toLowerCase().includes(searchTerm.toLowerCase()) ||
    project?.technology?.toLowerCase().includes(searchTerm.toLowerCase())
  );

  if (loading) {
    return (
      <div className="flex items-center justify-center h-64">
        <div className="animate-spin rounded-full h-32 w-32 border-b-2 border-gray-900"></div>
      </div>
    );
  }

  if (error) {
    return (
      <div className="flex items-center justify-center h-64">
        <div className="text-red-600">Error: {error}</div>
      </div>
    );
  }

  return (
    <div className="space-y-6">
      {/* Header */}
      <div className="flex justify-between items-center">
        <div>
          <h1 className="text-3xl font-bold">Projects Pipeline</h1>
          <p className="text-gray-600">Manage and track your project opportunities</p>
          {isAdmin && (
            <div className="flex items-center gap-2 mt-2">
              <Badge variant="secondary" className="bg-green-100 text-green-800">
                Admin Access
              </Badge>
              <span className="text-sm text-gray-500">Full system access granted</span>
            </div>
          )}
        </div>
        <div className="flex items-center gap-2">
          <ExportButtons data={filteredProjects} type="projects" />
          {canCreateProjects && (
            <Link href="/projects/add">
              <Button className="flex items-center gap-2">
                <Plus className="h-4 w-4" />
                Add New Project
              </Button>
            </Link>
          )}
        </div>
      </div>

      {/* Filters */}
      <Card>
        <CardHeader>
          <CardTitle className="flex items-center gap-2">
            <Filter className="h-5 w-5" />
            Filters
          </CardTitle>
        </CardHeader>
        <CardContent>
          <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
            <div className="relative">
              <Search className="absolute left-3 top-3 h-4 w-4 text-gray-400" />
              <Input
                placeholder="Search projects..."
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                className="pl-10"
              />
            </div>

            <Select value={statusFilter} onValueChange={setStatusFilter}>
              <SelectTrigger>
                <SelectValue placeholder="Filter by status" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="all">All Statuses</SelectItem>
                <SelectItem value="Pipeline">Pipeline</SelectItem>
                <SelectItem value="Won">Won</SelectItem>
                <SelectItem value="Lost">Lost</SelectItem>
                <SelectItem value="Missed">Missed</SelectItem>
              </SelectContent>
            </Select>

            <Select value={businessUnitFilter} onValueChange={setBusinessUnitFilter}>
              <SelectTrigger>
                <SelectValue placeholder="Filter by business unit" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="all">All Business Units</SelectItem>
                {(businessUnits || []).map((bu) => (
                  <SelectItem key={bu.id} value={bu.id.toString()}>
                    {bu.name}
                  </SelectItem>
                ))}
              </SelectContent>
            </Select>

            <Button 
              variant="outline" 
              onClick={() => {
                setStatusFilter('all');
                setBusinessUnitFilter('all');
                setSearchTerm('');
              }}
            >
              Clear Filters
            </Button>
          </div>
        </CardContent>
      </Card>

      {/* Results Summary */}
      <div className="flex justify-between items-center">
        <p className="text-sm text-gray-600">
          Showing {filteredProjects.length} of {totalCount} projects
        </p>
        <div className="flex items-center gap-2">
          <Button variant="outline" size="sm" onClick={refreshProjects}>
            Refresh
          </Button>
          <span className="text-sm text-gray-600">Page {currentPage} of {totalPages}</span>
        </div>
      </div>

      {/* Projects Grid */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-3 gap-6">
        {filteredProjects.length > 0 ? (
          filteredProjects.map((project) => (
            <ProjectCard key={project.id} project={project} />
          ))
        ) : (
          <div className="col-span-full text-center py-12">
            <p className="text-gray-500">No projects found matching your criteria.</p>
            {canCreateProjects && (
              <Link href="/projects/add">
                <Button className="mt-4">
                  <Plus className="h-4 w-4 mr-2" />
                  Create Your First Project
                </Button>
              </Link>
            )}
          </div>
        )}
      </div>

      {/* Pagination */}
      {totalPages > 1 && (
        <div className="flex justify-center gap-2">
          <Button
            variant="outline"
            onClick={() => setCurrentPage(prev => Math.max(1, prev - 1))}
            disabled={currentPage === 1}
          >
            Previous
          </Button>
          
          {Array.from({ length: Math.min(5, totalPages) }, (_, i) => {
            const page = i + 1;
            return (
              <Button
                key={page}
                variant={currentPage === page ? "default" : "outline"}
                onClick={() => setCurrentPage(page)}
              >
                {page}
              </Button>
            );
          })}
          
          <Button
            variant="outline"
            onClick={() => setCurrentPage(prev => Math.min(totalPages, prev + 1))}
            disabled={currentPage === totalPages}
          >
            Next
          </Button>
        </div>
      )}
    </div>
  );
}

function ProjectCard({ project }: { project: Project }) {
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
      month: 'short',
      day: 'numeric'
    });
  };

  return (
    <Link href={`/projects/${project.id}`}>
      <Card className="hover:shadow-lg transition-shadow cursor-pointer">
        <CardHeader>
          <div className="flex justify-between items-start">
            <div className="flex-1">
              <CardTitle className="text-lg">{project.name}</CardTitle>
              <CardDescription className="mt-1">{project.clientName}</CardDescription>
            </div>
            <Badge className={statusColors[project.status as keyof typeof statusColors] || 'bg-gray-100 text-gray-800'}>
              {project.status}
            </Badge>
          </div>
        </CardHeader>
        <CardContent>
          <div className="space-y-3">
            <p className="text-sm text-gray-600 line-clamp-2">{project.description}</p>
            
            <div className="grid grid-cols-2 gap-4 text-sm">
              <div className="flex items-center gap-2">
                <DollarSign className="h-4 w-4 text-green-600" />
                <span>{formatCurrency(project.estimatedValue)}</span>
              </div>
              <div className="flex items-center gap-2">
                <Calendar className="h-4 w-4 text-blue-600" />
                <span>{formatDate(project.expectedClosureDate)}</span>
              </div>
            </div>

            <div className="flex items-center gap-2 text-sm">
              <Building className="h-4 w-4 text-purple-600" />
              <span>{project.businessUnitName}</span>
            </div>

            <div className="flex flex-wrap gap-1">
              {project.technology?.split(',').map((tech, index) => (
                <Badge key={index} variant="secondary" className="text-xs">
                  {tech.trim()}
                </Badge>
              ))}
            </div>

            <div className="flex justify-between text-xs text-gray-500 pt-2 border-t">
              <span>Profiles: {project.profilesSubmitted}</span>
              <span>Created: {formatDate(project.createdAt)}</span>
            </div>
          </div>
        </CardContent>
      </Card>
    </Link>
  );
}
