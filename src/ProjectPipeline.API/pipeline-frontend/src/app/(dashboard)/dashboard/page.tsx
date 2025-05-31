'use client';

import { useAuth } from '@/hooks/useAuth';
import { useDashboardStats } from '@/hooks/useDashboard';
import { Button } from '@/components/ui/button';
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Badge } from '@/components/ui/badge';
import Link from 'next/link';
import { FolderOpen, Users, BarChart3, Settings, Shield, RefreshCw } from 'lucide-react';
import DashboardStats from '@/components/dashboard/DashboardStats';
import { ProjectStatusChart, MonthlyTrendsChart, ValueTrendsChart } from '@/components/dashboard/DashboardCharts';
import RecentProjects from '@/components/dashboard/RecentProjects';
import ExportButtons from '@/components/common/ExportButtons';
import { useState, useEffect } from 'react';
import apiClient from '@/lib/api';
import { Project } from '@/types';

export default function DashboardPage() {
    const { user, logout } = useAuth();
    const { stats } = useDashboardStats();
    const [refreshKey, setRefreshKey] = useState(0);
    const [projects, setProjects] = useState<Project[]>([]);
    const isAdmin = user?.email === 'admin@projectpipeline.com';

    useEffect(() => {
        fetchProjects();
    }, [refreshKey]);

    const fetchProjects = async () => {
        try {
            const response = await apiClient.get('/projects?page=1&pageSize=1000');
            if (response.data.isSuccess && response.data.data) {
                setProjects(response.data.data.items || []);
            }
        } catch (error) {
            console.error('Failed to fetch projects for export:', error);
        }
    };

    const handleRefresh = () => {
        setRefreshKey(prev => prev + 1);
    };

    return (
        <div className="container mx-auto p-6 space-y-6">
            {/* Header */}
            <div className="flex justify-between items-center">
                <div>
                    <h1 className="text-3xl font-bold">Project Pipeline Dashboard</h1>
                    <div className="flex items-center gap-2 mt-2">
                        <p className="text-gray-600">Welcome back, {user?.firstName}!</p>
                        {isAdmin && (
                            <Badge className="bg-green-100 text-green-800 flex items-center gap-1">
                                <Shield className="h-3 w-3" />
                                Admin
                            </Badge>
                        )}
                    </div>
                </div>
                <div className="flex items-center gap-2">
                    {stats && (
                        <ExportButtons data={projects} type="dashboard" stats={stats} />
                    )}
                    <Button onClick={handleRefresh} variant="outline" size="sm">
                        <RefreshCw className="h-4 w-4 mr-2" />
                        Refresh
                    </Button>
                    <Button onClick={logout} variant="outline">
                        Logout
                    </Button>
                </div>
            </div>

            {/* Admin Notice */}
            {isAdmin && (
                <Card className="border-green-200 bg-green-50">
                    <CardHeader>
                        <CardTitle className="flex items-center gap-2 text-green-800">
                            <Shield className="h-5 w-5" />
                            Administrator Dashboard
                        </CardTitle>
                        <CardDescription className="text-green-700">
                            You have full system access with real-time analytics, export capabilities, and comprehensive project insights.
                        </CardDescription>
                    </CardHeader>
                </Card>
            )}

            {/* Real-time Statistics */}
            <div key={refreshKey}>
                <DashboardStats />
            </div>

            {/* Charts Section */}
            <div className="grid grid-cols-1 lg:grid-cols-2 gap-6">
                <ProjectStatusChart />
                <MonthlyTrendsChart />
            </div>

            {/* Value Trends - Full Width */}
            <ValueTrendsChart />

            {/* Bottom Section */}
            <div className="grid grid-cols-1 lg:grid-cols-3 gap-6">
                {/* Recent Projects - Takes 2 columns */}
                <div className="lg:col-span-2">
                    <RecentProjects />
                </div>

                {/* Quick Actions */}
                <div className="space-y-4">
                    <Card>
                        <CardHeader>
                            <CardTitle>Quick Actions</CardTitle>
                            <CardDescription>
                                Frequently used features
                            </CardDescription>
                        </CardHeader>
                        <CardContent className="space-y-3">
                            <Link href="/projects" className="block">
                                <Button variant="outline" className="w-full justify-start">
                                    <FolderOpen className="h-4 w-4 mr-2" />
                                    Manage Projects
                                </Button>
                            </Link>

                            <Link href="/projects/add" className="block">
                                <Button variant="outline" className="w-full justify-start">
                                    <FolderOpen className="h-4 w-4 mr-2" />
                                    Add New Project
                                </Button>
                            </Link>

                            <Button variant="outline" className="w-full justify-start" disabled>
                                <Users className="h-4 w-4 mr-2" />
                                Team Management
                                <Badge variant="secondary" className="ml-auto">Soon</Badge>
                            </Button>

                            <Button variant="outline" className="w-full justify-start" disabled>
                                <BarChart3 className="h-4 w-4 mr-2" />
                                Advanced Reports
                                <Badge variant="secondary" className="ml-auto">Soon</Badge>
                            </Button>

                            <Button variant="outline" className="w-full justify-start" disabled>
                                <Settings className="h-4 w-4 mr-2" />
                                System Settings
                                <Badge variant="secondary" className="ml-auto">Soon</Badge>
                            </Button>
                        </CardContent>
                    </Card>

                    {/* User Information */}
                    <Card>
                        <CardHeader>
                            <CardTitle>User Information</CardTitle>
                        </CardHeader>
                        <CardContent>
                            <div className="space-y-2 text-sm">
                                <p><strong>Name:</strong> {user?.firstName} {user?.lastName}</p>
                                <p><strong>Email:</strong> {user?.email}</p>
                                <p><strong>Department:</strong> {user?.department}</p>
                                <p><strong>Business Unit:</strong> {user?.businessUnitName}</p>
                                {isAdmin && (
                                    <p><strong>Access Level:</strong> <span className="text-green-600 font-semibold">Administrator</span></p>
                                )}
                            </div>
                        </CardContent>
                    </Card>
                </div>
            </div>
        </div>
    );
}