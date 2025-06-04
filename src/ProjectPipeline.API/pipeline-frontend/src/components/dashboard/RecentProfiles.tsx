'use client';

import { useState, useEffect } from 'react';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Badge } from '@/components/ui/badge';
import { Button } from '@/components/ui/button';
import { Clock, ExternalLink, User, Briefcase } from 'lucide-react';
import apiClient from '@/lib/api';
import Link from 'next/link';

interface RecentProfile {
  id: number;
  projectId: number;
  projectName: string;
  candidateName: string;
  candidateEmail: string;
  position: string;
  status: string;
  submissionDate: string;
  daysInPipeline: number;
}

const statusColors = {
  Submitted: 'bg-blue-100 text-blue-800',
  UnderScreening: 'bg-yellow-100 text-yellow-800',
  Shortlisted: 'bg-purple-100 text-purple-800',
  InterviewScheduled: 'bg-indigo-100 text-indigo-800',
  InterviewCompleted: 'bg-cyan-100 text-cyan-800',
  Selected: 'bg-green-100 text-green-800',
  Rejected: 'bg-red-100 text-red-800',
  OnHold: 'bg-gray-100 text-gray-800',
  Joined: 'bg-green-200 text-green-900'
};

export default function RecentProfiles() {
  const [profiles, setProfiles] = useState<RecentProfile[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetchRecentProfiles();
  }, []);

  const fetchRecentProfiles = async () => {
    try {
      // Get recent profiles from all projects
      const response = await apiClient.get('/profile-submissions/status/Submitted');
      if (response.data.isSuccess && response.data.data) {
        // Take only the 5 most recent
        const recentProfiles = (response.data.data as RecentProfile[])
          .sort((a, b) => new Date(b.submissionDate).getTime() - new Date(a.submissionDate).getTime())
          .slice(0, 5);
        setProfiles(recentProfiles);
      }
    } catch (error) {
      console.error('Failed to fetch recent profiles:', error);
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

  if (loading) {
    return (
      <Card>
        <CardHeader>
          <CardTitle className="flex items-center gap-2">
            <User className="h-5 w-5" />
            Recent Profile Submissions
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
          <User className="h-5 w-5" />
          Recent Profile Submissions
        </CardTitle>
        <CardDescription>
          Latest candidate profiles submitted to projects
        </CardDescription>
      </CardHeader>
      <CardContent>
        <div className="space-y-4">
          {profiles.length > 0 ? (
            profiles.map((profile) => (
              <div key={profile.id} className="flex items-center justify-between p-3 border rounded-lg hover:bg-gray-50 transition-colors">
                <div className="flex-1 min-w-0">
                  <div className="flex items-center gap-2 mb-1">
                    <h4 className="font-semibold text-sm truncate">{profile.candidateName}</h4>
                    <Badge className={statusColors[profile.status as keyof typeof statusColors] || 'bg-gray-100 text-gray-800'}>
                      {profile.status}
                    </Badge>
                  </div>
                  <div className="flex items-center gap-2 text-xs text-gray-600 mb-1">
                    <Briefcase className="h-3 w-3" />
                    <span className="truncate">{profile.position}</span>
                  </div>
                  <p className="text-xs text-gray-600 truncate">{profile.projectName}</p>
                  <div className="flex items-center gap-4 mt-1 text-xs text-gray-500">
                    <span>{formatDate(profile.submissionDate)}</span>
                    <span className={profile.daysInPipeline > 7 ? 'text-orange-600' : ''}>
                      {profile.daysInPipeline} days in pipeline
                    </span>
                  </div>
                </div>
                <Link href={`/projects/${profile.projectId}/profiles/${profile.id}`}>
                  <Button variant="ghost" size="sm">
                    <ExternalLink className="h-4 w-4" />
                  </Button>
                </Link>
              </div>
            ))
          ) : (
            <div className="text-center py-4 text-gray-500">
              No recent profile submissions found
            </div>
          )}
        </div>
        {profiles.length > 0 && (
          <div className="mt-4 pt-4 border-t">
            <Link href="/projects">
              <Button variant="outline" className="w-full">
                View All Projects & Profiles
              </Button>
            </Link>
          </div>
        )}
      </CardContent>
    </Card>
  );
}
