'use client';

import { useState, useEffect } from 'react';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Button } from '@/components/ui/button';
import { Badge } from '@/components/ui/badge';
import { Select, SelectContent, SelectItem, SelectTrigger, SelectValue } from '@/components/ui/select';
import { Input } from '@/components/ui/input';
import { Table, TableBody, TableCell, TableHead, TableHeader, TableRow } from '@/components/ui/table';
import { 
  Search, 
  Filter, 
  Plus, 
  Eye, 
  Edit, 
  Clock, 
  User, 
  Mail, 
  Phone,
  Calendar,
  DollarSign
} from 'lucide-react';
import { useAuth } from '@/hooks/useAuth';
import apiClient from '@/lib/api';
import Link from 'next/link';

interface ProfileSubmission {
  id: number;
  projectId: number;
  projectName: string;
  candidateName: string;
  candidateEmail: string;
  candidatePhone?: string;
  position: string;
  technology: string;
  experienceYears: number;
  expectedSalary?: number;
  offeredSalary?: number;
  status: string;
  statusComments?: string;
  rejectionReason?: string;
  holdReason?: string;
  submissionDate: string;
  interviewDate?: string;
  expectedJoiningDate?: string;
  actualJoiningDate?: string;
  interviewScore?: number;
  technicalScore?: number;
  daysInPipeline: number;
  submittedBy: string;
  statusHistoryCount: number;
}

const statusColors = {
  Submitted: 'bg-blue-100 text-blue-800',
  UnderScreening: 'bg-yellow-100 text-yellow-800',
  Shortlisted: 'bg-purple-100 text-purple-800',
  InterviewScheduled: 'bg-indigo-100 text-indigo-800',
  InterviewCompleted: 'bg-cyan-100 text-cyan-800',
  TechnicalRoundScheduled: 'bg-orange-100 text-orange-800',
  TechnicalRoundCompleted: 'bg-pink-100 text-pink-800',
  Selected: 'bg-green-100 text-green-800',
  Rejected: 'bg-red-100 text-red-800',
  OnHold: 'bg-gray-100 text-gray-800',
  OfferExtended: 'bg-emerald-100 text-emerald-800',
  OfferAccepted: 'bg-teal-100 text-teal-800',
  OfferDeclined: 'bg-rose-100 text-rose-800',
  Joined: 'bg-green-200 text-green-900',
  DroppedOut: 'bg-red-200 text-red-900'
};

interface ProfileSubmissionListProps {
  projectId: number;
}

export default function ProfileSubmissionList({ projectId }: ProfileSubmissionListProps) {
  const { user } = useAuth();
  const [profiles, setProfiles] = useState<ProfileSubmission[]>([]);
  const [filteredProfiles, setFilteredProfiles] = useState<ProfileSubmission[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');
  const [statusFilter, setStatusFilter] = useState('all');
  const [searchTerm, setSearchTerm] = useState('');

  const isAdmin = user?.email === 'admin@projectpipeline.com';
  const canCreateProfiles = !!user;

  useEffect(() => {
    fetchProfiles();
  }, [projectId]);

  useEffect(() => {
    filterProfiles();
  }, [profiles, statusFilter, searchTerm]);

  const fetchProfiles = async () => {
    try {
      setLoading(true);
      const response = await apiClient.get(`/profile-submissions/project/${projectId}`);
      
      if (response.data.isSuccess) {
        setProfiles(response.data.data || []);
      } else {
        setError(response.data.message || 'Failed to fetch profiles');
      }
    } catch (err: any) {
      setError(err.response?.data?.message || 'Failed to fetch profiles');
    } finally {
      setLoading(false);
    }
  };

  const filterProfiles = () => {
    let filtered = profiles;

    // Filter by status
    if (statusFilter !== 'all') {
      filtered = filtered.filter(profile => profile.status === statusFilter);
    }

    // Filter by search term
    if (searchTerm) {
      filtered = filtered.filter(profile =>
        profile.candidateName.toLowerCase().includes(searchTerm.toLowerCase()) ||
        profile.candidateEmail.toLowerCase().includes(searchTerm.toLowerCase()) ||
        profile.position.toLowerCase().includes(searchTerm.toLowerCase()) ||
        profile.technology.toLowerCase().includes(searchTerm.toLowerCase())
      );
    }

    setFilteredProfiles(filtered);
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
      month: 'short',
      day: 'numeric'
    });
  };

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
          <h2 className="text-2xl font-bold">Profile Submissions</h2>
          <p className="text-gray-600">Manage candidate profiles for this project</p>
        </div>
        {canCreateProfiles && (
          <Link href={`/projects/${projectId}/profiles/add`}>
            <Button className="flex items-center gap-2">
              <Plus className="h-4 w-4" />
              Add Profile
            </Button>
          </Link>
        )}
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
          <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
            {/* Search */}
            <div className="relative">
              <Search className="absolute left-3 top-3 h-4 w-4 text-gray-400" />
              <Input
                placeholder="Search candidates..."
                value={searchTerm}
                onChange={(e) => setSearchTerm(e.target.value)}
                className="pl-10"
              />
            </div>

            {/* Status Filter */}
            <Select value={statusFilter} onValueChange={setStatusFilter}>
              <SelectTrigger>
                <SelectValue placeholder="Filter by status" />
              </SelectTrigger>
              <SelectContent>
                <SelectItem value="all">All Statuses</SelectItem>
                <SelectItem value="Submitted">Submitted</SelectItem>
                <SelectItem value="UnderScreening">Under Screening</SelectItem>
                <SelectItem value="Shortlisted">Shortlisted</SelectItem>
                <SelectItem value="InterviewScheduled">Interview Scheduled</SelectItem>
                <SelectItem value="InterviewCompleted">Interview Completed</SelectItem>
                <SelectItem value="Selected">Selected</SelectItem>
                <SelectItem value="Rejected">Rejected</SelectItem>
                <SelectItem value="OnHold">On Hold</SelectItem>
                <SelectItem value="Joined">Joined</SelectItem>
              </SelectContent>
            </Select>

            {/* Clear Filters */}
            <Button 
              variant="outline" 
              onClick={() => {
                setStatusFilter('all');
                setSearchTerm('');
              }}
            >
              Clear Filters
            </Button>
          </div>
        </CardContent>
      </Card>

      {/* Summary Stats */}
      <div className="grid grid-cols-1 md:grid-cols-4 gap-4">
        <Card>
          <CardContent className="p-4">
            <div className="flex items-center gap-2">
              <User className="h-5 w-5 text-blue-600" />
              <div>
                <div className="text-2xl font-bold">{profiles.length}</div>
                <div className="text-sm text-gray-600">Total Profiles</div>
              </div>
            </div>
          </CardContent>
        </Card>

        <Card>
          <CardContent className="p-4">
            <div className="flex items-center gap-2">
              <Clock className="h-5 w-5 text-yellow-600" />
              <div>
                <div className="text-2xl font-bold">
                  {profiles.filter(p => p.status === 'Shortlisted' || p.status === 'InterviewScheduled').length}
                </div>
                <div className="text-sm text-gray-600">In Process</div>
              </div>
            </div>
          </CardContent>
        </Card>

        <Card>
          <CardContent className="p-4">
            <div className="flex items-center gap-2">
              <Badge className="h-5 w-5 bg-green-600" />
              <div>
                <div className="text-2xl font-bold text-green-600">
                  {profiles.filter(p => p.status === 'Selected' || p.status === 'Joined').length}
                </div>
                <div className="text-sm text-gray-600">Selected</div>
              </div>
            </div>
          </CardContent>
        </Card>

        <Card>
          <CardContent className="p-4">
            <div className="flex items-center gap-2">
              <Badge className="h-5 w-5 bg-red-600" />
              <div>
                <div className="text-2xl font-bold text-red-600">
                  {profiles.filter(p => p.status === 'Rejected').length}
                </div>
                <div className="text-sm text-gray-600">Rejected</div>
              </div>
            </div>
          </CardContent>
        </Card>
      </div>

      {/* Profiles Table */}
      <Card>
        <CardHeader>
          <CardTitle>Candidate Profiles ({filteredProfiles.length})</CardTitle>
          <CardDescription>
            Track candidate progress through the hiring pipeline
          </CardDescription>
        </CardHeader>
        <CardContent>
          {filteredProfiles.length > 0 ? (
            <div className="overflow-x-auto">
              <Table>
                <TableHeader>
                  <TableRow>
                    <TableHead>Candidate</TableHead>
                    <TableHead>Position</TableHead>
                    <TableHead>Experience</TableHead>
                    <TableHead>Status</TableHead>
                    <TableHead>Expected Salary</TableHead>
                    <TableHead>Days in Pipeline</TableHead>
                    <TableHead>Submitted By</TableHead>
                    <TableHead>Actions</TableHead>
                  </TableRow>
                </TableHeader>
                <TableBody>
                  {filteredProfiles.map((profile) => (
                    <TableRow key={profile.id}>
                      <TableCell>
                        <div>
                          <div className="font-medium">{profile.candidateName}</div>
                          <div className="text-sm text-gray-600 flex items-center gap-1">
                            <Mail className="h-3 w-3" />
                            {profile.candidateEmail}
                          </div>
                          {profile.candidatePhone && (
                            <div className="text-sm text-gray-600 flex items-center gap-1">
                              <Phone className="h-3 w-3" />
                              {profile.candidatePhone}
                            </div>
                          )}
                        </div>
                      </TableCell>
                      <TableCell>
                        <div>
                          <div className="font-medium">{profile.position}</div>
                          <div className="text-sm text-gray-600">{profile.technology}</div>
                        </div>
                      </TableCell>
                      <TableCell>
                        <div className="text-center">
                          <div className="font-medium">{profile.experienceYears}</div>
                          <div className="text-sm text-gray-600">years</div>
                        </div>
                      </TableCell>
                      <TableCell>
                        <Badge className={statusColors[profile.status as keyof typeof statusColors] || 'bg-gray-100 text-gray-800'}>
                          {profile.status.replace(/([A-Z])/g, ' $1').trim()}
                        </Badge>
                      </TableCell>
                      <TableCell>
                        <div className="flex items-center gap-1">
                          <DollarSign className="h-3 w-3 text-green-600" />
                          {formatCurrency(profile.expectedSalary)}
                        </div>
                      </TableCell>
                      <TableCell>
                        <div className="flex items-center gap-1">
                          <Calendar className="h-3 w-3 text-blue-600" />
                          <span className={profile.daysInPipeline > 30 ? 'text-red-600 font-medium' : ''}>
                            {profile.daysInPipeline} days
                          </span>
                        </div>
                      </TableCell>
                      <TableCell>
                        <div className="text-sm">{profile.submittedBy}</div>
                      </TableCell>
                      <TableCell>
                        <div className="flex items-center gap-2">
                          <Link href={`/projects/${projectId}/profiles/${profile.id}`}>
                            <Button variant="ghost" size="sm">
                              <Eye className="h-4 w-4" />
                            </Button>
                          </Link>
                          <Link href={`/projects/${projectId}/profiles/${profile.id}/edit`}>
                            <Button variant="ghost" size="sm">
                              <Edit className="h-4 w-4" />
                            </Button>
                          </Link>
                        </div>
                      </TableCell>
                    </TableRow>
                  ))}
                </TableBody>
              </Table>
            </div>
          ) : (
            <div className="text-center py-12">
              <User className="h-12 w-12 text-gray-400 mx-auto mb-4" />
              <p className="text-gray-500 mb-4">No profiles found matching your criteria.</p>
              {canCreateProfiles && (
                <Link href={`/projects/${projectId}/profiles/add`}>
                  <Button>
                    <Plus className="h-4 w-4 mr-2" />
                    Add First Profile
                  </Button>
                </Link>
              )}
            </div>
          )}
        </CardContent>
      </Card>
    </div>
  );
}
