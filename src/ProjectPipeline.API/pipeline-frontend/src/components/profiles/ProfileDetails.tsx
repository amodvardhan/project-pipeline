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
  User, 
  Mail, 
  Phone, 
  Briefcase, 
  Code, 
  DollarSign,
  Calendar,
  Clock,
  FileText,
  History
} from 'lucide-react';
import apiClient from '@/lib/api';

interface ProfileDetails {
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
  interviewFeedback?: string;
  technicalFeedback?: string;
  interviewerName?: string;
  resumePath?: string;
  offerLetterPath?: string;
  daysInPipeline: number;
  submittedBy: string;
  statusHistory: StatusHistory[];
}

interface StatusHistory {
  fromStatus: string;
  toStatus: string;
  comments?: string;
  reason?: string;
  changedBy: string;
  changedDate: string;
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

interface ProfileDetailsProps {
  profileId: string;
}

export default function ProfileDetails({ profileId }: ProfileDetailsProps) {
  const router = useRouter();
  const { user } = useAuth();
  const [profile, setProfile] = useState<ProfileDetails | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState('');

  const isAdmin = user?.email === 'admin@projectpipeline.com';
  const canEdit = !!user;

  useEffect(() => {
    fetchProfile();
  }, [profileId]);

  const fetchProfile = async () => {
    try {
      setLoading(true);
      const response = await apiClient.get(`/profile-submissions/${profileId}`);
      
      if (response.data.isSuccess && response.data.data) {
        setProfile(response.data.data);
      } else {
        setError('Profile not found');
      }
    } catch (err: any) {
      setError(err.response?.data?.message || 'Failed to fetch profile details');
    } finally {
      setLoading(false);
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

  if (error || !profile) {
    return (
      <div className="max-w-4xl mx-auto space-y-6">
        <Alert variant="destructive">
          <AlertDescription>{error || 'Profile not found'}</AlertDescription>
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
            Back to Profiles
          </Button>
          <div>
            <h1 className="text-3xl font-bold">{profile.candidateName}</h1>
            <p className="text-gray-600">{profile.position} • {profile.projectName}</p>
          </div>
        </div>
        
        <div className="flex items-center gap-2">
          <Badge className={statusColors[profile.status as keyof typeof statusColors] || 'bg-gray-100 text-gray-800'}>
            {profile.status.replace(/([A-Z])/g, ' $1').trim()}
          </Badge>
          {canEdit && (
            <Button variant="outline" size="sm">
              <Edit className="h-4 w-4 mr-2" />
              Update Status
            </Button>
          )}
        </div>
      </div>

      {/* Main Content */}
      <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
        {/* Left Column - Main Details */}
        <div className="lg:col-span-2 space-y-6">
          {/* Candidate Information */}
          <Card>
            <CardHeader>
              <CardTitle className="flex items-center gap-2">
                <User className="h-5 w-5" />
                Candidate Information
              </CardTitle>
            </CardHeader>
            <CardContent className="space-y-4">
              <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                <div>
                  <h4 className="font-semibold mb-2 flex items-center gap-2">
                    <Mail className="h-4 w-4" />
                    Email
                  </h4>
                  <p className="text-gray-700">{profile.candidateEmail}</p>
                </div>
                {profile.candidatePhone && (
                  <div>
                    <h4 className="font-semibold mb-2 flex items-center gap-2">
                      <Phone className="h-4 w-4" />
                      Phone
                    </h4>
                    <p className="text-gray-700">{profile.candidatePhone}</p>
                  </div>
                )}
              </div>

              <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                <div>
                  <h4 className="font-semibold mb-2 flex items-center gap-2">
                    <Briefcase className="h-4 w-4" />
                    Position
                  </h4>
                  <p className="text-gray-700">{profile.position}</p>
                </div>
                <div>
                  <h4 className="font-semibold mb-2">Experience</h4>
                  <p className="text-gray-700">{profile.experienceYears} years</p>
                </div>
              </div>

              <div>
                <h4 className="font-semibold mb-2 flex items-center gap-2">
                  <Code className="h-4 w-4" />
                  Technology Stack
                </h4>
                <div className="flex flex-wrap gap-2">
                  {profile.technology.split(',').map((tech, index) => (
                    <Badge key={index} variant="secondary">
                      {tech.trim()}
                    </Badge>
                  ))}
                </div>
              </div>

              {profile.resumePath && (
                <div>
                  <h4 className="font-semibold mb-2 flex items-center gap-2">
                    <FileText className="h-4 w-4" />
                    Resume
                  </h4>
                  <a 
                    href={profile.resumePath} 
                    target="_blank" 
                    rel="noopener noreferrer"
                    className="text-blue-600 hover:underline"
                  >
                    View Resume
                  </a>
                </div>
              )}
            </CardContent>
          </Card>

          {/* Interview & Assessment */}
          {(profile.interviewScore || profile.technicalScore || profile.interviewFeedback || profile.technicalFeedback) && (
            <Card>
              <CardHeader>
                <CardTitle>Interview & Assessment</CardTitle>
              </CardHeader>
              <CardContent className="space-y-4">
                {profile.interviewerName && (
                  <div>
                    <h4 className="font-semibold mb-2">Interviewer</h4>
                    <p className="text-gray-700">{profile.interviewerName}</p>
                  </div>
                )}

                <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
                  {profile.interviewScore && (
                    <div>
                      <h4 className="font-semibold mb-2">Interview Score</h4>
                      <div className="flex items-center gap-2">
                        <div className="text-2xl font-bold text-blue-600">{profile.interviewScore}</div>
                        <div className="text-sm text-gray-600">/ 10</div>
                      </div>
                    </div>
                  )}

                  {profile.technicalScore && (
                    <div>
                      <h4 className="font-semibold mb-2">Technical Score</h4>
                      <div className="flex items-center gap-2">
                        <div className="text-2xl font-bold text-green-600">{profile.technicalScore}</div>
                        <div className="text-sm text-gray-600">/ 10</div>
                      </div>
                    </div>
                  )}
                </div>

                {profile.interviewFeedback && (
                  <div>
                    <h4 className="font-semibold mb-2">Interview Feedback</h4>
                    <p className="text-gray-700 bg-gray-50 p-3 rounded">{profile.interviewFeedback}</p>
                  </div>
                )}

                {profile.technicalFeedback && (
                  <div>
                    <h4 className="font-semibold mb-2">Technical Feedback</h4>
                    <p className="text-gray-700 bg-gray-50 p-3 rounded">{profile.technicalFeedback}</p>
                  </div>
                )}
              </CardContent>
            </Card>
          )}

          {/* Status History */}
          <Card>
            <CardHeader>
              <CardTitle className="flex items-center gap-2">
                <History className="h-5 w-5" />
                Status History
              </CardTitle>
              <CardDescription>
                Complete timeline of status changes
              </CardDescription>
            </CardHeader>
            <CardContent>
              <div className="space-y-4">
                {profile.statusHistory.map((history, index) => (
                  <div key={index} className="flex items-start gap-4 pb-4 border-b last:border-b-0">
                    <div className="flex-shrink-0 w-2 h-2 bg-blue-600 rounded-full mt-2"></div>
                    <div className="flex-1">
                      <div className="flex items-center gap-2 mb-1">
                        <Badge variant="outline" className="text-xs">
                          {history.fromStatus} → {history.toStatus}
                        </Badge>
                        <span className="text-sm text-gray-500">
                          {formatDateTime(history.changedDate)}
                        </span>
                      </div>
                      <p className="text-sm text-gray-600 mb-1">
                        Changed by: {history.changedBy}
                      </p>
                      {history.comments && (
                        <p className="text-sm text-gray-700">{history.comments}</p>
                      )}
                      {history.reason && (
                        <p className="text-sm text-red-600">Reason: {history.reason}</p>
                      )}
                    </div>
                  </div>
                ))}
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
                Compensation
              </CardTitle>
            </CardHeader>
            <CardContent className="space-y-4">
              <div>
                <div className="text-sm text-gray-600">Expected Salary</div>
                <div className="text-xl font-bold text-green-600">
                  {formatCurrency(profile.expectedSalary)}
                </div>
              </div>
              
              {profile.offeredSalary && (
                <div>
                  <div className="text-sm text-gray-600">Offered Salary</div>
                  <div className="text-xl font-bold text-blue-600">
                    {formatCurrency(profile.offeredSalary)}
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
                <div className="text-sm text-gray-600">Submitted</div>
                <div className="font-semibold">{formatDate(profile.submissionDate)}</div>
              </div>
              
              {profile.interviewDate && (
                <div>
                  <div className="text-sm text-gray-600">Interview Date</div>
                  <div className="font-semibold">{formatDate(profile.interviewDate)}</div>
                </div>
              )}
              
              {profile.expectedJoiningDate && (
                <div>
                  <div className="text-sm text-gray-600">Expected Joining</div>
                  <div className="font-semibold">{formatDate(profile.expectedJoiningDate)}</div>
                </div>
              )}

              {profile.actualJoiningDate && (
                <div>
                  <div className="text-sm text-gray-600">Actual Joining</div>
                  <div className="font-semibold">{formatDate(profile.actualJoiningDate)}</div>
                </div>
              )}
            </CardContent>
          </Card>

          {/* Pipeline Metrics */}
          <Card>
            <CardHeader>
              <CardTitle className="flex items-center gap-2">
                <Clock className="h-5 w-5" />
                Pipeline Metrics
              </CardTitle>
            </CardHeader>
            <CardContent className="space-y-4">
              <div>
                <div className="text-sm text-gray-600">Days in Pipeline</div>
                <div className={`text-2xl font-bold ${profile.daysInPipeline > 30 ? 'text-red-600' : 'text-blue-600'}`}>
                  {profile.daysInPipeline}
                </div>
              </div>
              
              <div>
                <div className="text-sm text-gray-600">Submitted By</div>
                <div className="font-semibold">{profile.submittedBy}</div>
              </div>
            </CardContent>
          </Card>

          {/* Status Information */}
          {(profile.rejectionReason || profile.holdReason || profile.statusComments) && (
            <Card>
              <CardHeader>
                <CardTitle>Status Information</CardTitle>
              </CardHeader>
              <CardContent className="space-y-4">
                {profile.rejectionReason && (
                  <div>
                    <div className="text-sm text-gray-600">Rejection Reason</div>
                    <div className="text-red-600 font-medium">{profile.rejectionReason}</div>
                  </div>
                )}

                {profile.holdReason && (
                  <div>
                    <div className="text-sm text-gray-600">Hold Reason</div>
                    <div className="text-yellow-600 font-medium">{profile.holdReason}</div>
                  </div>
                )}

                {profile.statusComments && (
                  <div>
                    <div className="text-sm text-gray-600">Comments</div>
                    <div className="text-gray-700">{profile.statusComments}</div>
                  </div>
                )}
              </CardContent>
            </Card>
          )}
        </div>
      </div>
    </div>
  );
}
