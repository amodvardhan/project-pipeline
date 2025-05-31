'use client';

import { useState, useEffect } from 'react';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Badge } from '@/components/ui/badge';
import { Button } from '@/components/ui/button';
import { Clock, ExternalLink } from 'lucide-react';
import { Project } from '@/types';
import apiClient from '@/lib/api';
import Link from 'next/link';

const statusColors = {
  Pipeline: 'bg-blue-100 text-blue-800',
  Won: 'bg-green-100 text-green-800',
  Lost: 'bg-red-100 text-red-800',
  Missed: 'bg-yellow-100 text-yellow-800',
  OnHold: 'bg-gray-100 text-gray-800',
  Cancelled: 'bg-red-100 text-red-800'
};

export default function RecentProjects() {
  const [projects, setProjects] = useState<Project[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetchRecentProjects();
  }, []);

  const fetchRecentProjects = async () => {
    try {
      const response = await apiClient.get('/projects?page=1&pageSize=5');
      if (response.data.isSuccess && response.data.data) {
        setProjects(response.data.data.items || []);
      }
    } catch (error) {
      console.error('Failed to fetch recent projects:', error);
    } finally {
      setLoading(false);
    }
  };

  const formatDate = (dateString: string) => {
    return new Date(dateString).toLocaleDateString('en-US', {
      month: 'short',
      day: 'numeric',
      year: 'numeric'
    });
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

  if (loading) {
    return (
      <Card>
        <CardHeader>
          <CardTitle className="flex items-center gap-2">
            <Clock className="h-5 w-5" />
            Recent Projects
          </CardTitle>
        </CardHeader>
        <CardContent>
          <div className="space-y-4">
            {Array.from({ length: 3 }).map((_, i) => (
              <div key={i} className="animate-pulse">
                <div className="h-4 bg-gray-200 rounded w-3/4 mb-2"></div>
                <div className="h-3 bg-gray-200 rounded w-1/2"></div>
              </div>
            ))}
          </div>
        </CardContent>
      </Card>
    );
  }

  return (
    <Card>
      <CardHeader>
        <CardTitle className="flex items-center gap-2">
          <Clock className="h-5 w-5" />
          Recent Projects
        </CardTitle>
        <CardDescription>
          Latest projects added to the system
        </CardDescription>
      </CardHeader>
      <CardContent>
        <div className="space-y-4">
          {projects.length > 0 ? (
            projects.map((project) => (
              <div key={project.id} className="flex items-center justify-between p-3 border rounded-lg hover:bg-gray-50 transition-colors">
                <div className="flex-1 min-w-0">
                  <div className="flex items-center gap-2 mb-1">
                    <h4 className="font-semibold text-sm truncate">{project.name}</h4>
                    <Badge className={statusColors[project.status as keyof typeof statusColors] || 'bg-gray-100 text-gray-800'}>
                      {project.status}
                    </Badge>
                  </div>
                  <p className="text-xs text-gray-600 truncate">{project.clientName}</p>
                  <div className="flex items-center gap-4 mt-1 text-xs text-gray-500">
                    <span>{formatCurrency(project.estimatedValue)}</span>
                    <span>{formatDate(project.createdAt)}</span>
                  </div>
                </div>
                <Link href={`/projects/${project.id}`}>
                  <Button variant="ghost" size="sm">
                    <ExternalLink className="h-4 w-4" />
                  </Button>
                </Link>
              </div>
            ))
          ) : (
            <div className="text-center py-4 text-gray-500">
              No projects found
            </div>
          )}
        </div>
        {projects.length > 0 && (
          <div className="mt-4 pt-4 border-t">
            <Link href="/projects">
              <Button variant="outline" className="w-full">
                View All Projects
              </Button>
            </Link>
          </div>
        )}
      </CardContent>
    </Card>
  );
}
