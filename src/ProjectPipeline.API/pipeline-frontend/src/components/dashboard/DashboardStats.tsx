'use client';

import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { Badge } from '@/components/ui/badge';
import { useDashboardStats } from '@/hooks/useDashboard';
import { 
  TrendingUp, 
  TrendingDown, 
  DollarSign, 
  Target, 
  Users, 
  Building,
  Award,
  BarChart3
} from 'lucide-react';

export default function DashboardStats() {
  const { stats, loading, error } = useDashboardStats();

  if (loading) {
    return (
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
        {Array.from({ length: 8 }).map((_, i) => (
          <Card key={i}>
            <CardContent className="p-6">
              <div className="animate-pulse">
                <div className="h-4 bg-gray-200 rounded w-3/4 mb-2"></div>
                <div className="h-8 bg-gray-200 rounded w-1/2"></div>
              </div>
            </CardContent>
          </Card>
        ))}
      </div>
    );
  }

  if (error || !stats) {
    return (
      <Card>
        <CardContent className="p-6">
          <div className="text-red-600">Error loading dashboard statistics</div>
        </CardContent>
      </Card>
    );
  }

  const formatCurrency = (amount: number) => {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD',
      minimumFractionDigits: 0,
      maximumFractionDigits: 0,
    }).format(amount);
  };

  const formatPercentage = (value: number) => {
    return `${value.toFixed(1)}%`;
  };

  return (
    <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
      {/* Total Projects */}
      <Card>
        <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
          <CardTitle className="text-sm font-medium">Total Projects</CardTitle>
          <BarChart3 className="h-4 w-4 text-muted-foreground" />
        </CardHeader>
        <CardContent>
          <div className="text-2xl font-bold">{stats.totalProjects}</div>
          <p className="text-xs text-muted-foreground">
            All projects in system
          </p>
        </CardContent>
      </Card>

      {/* Won Projects */}
      <Card>
        <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
          <CardTitle className="text-sm font-medium">Won Projects</CardTitle>
          <Award className="h-4 w-4 text-green-600" />
        </CardHeader>
        <CardContent>
          <div className="text-2xl font-bold text-green-600">{stats.wonProjects}</div>
          <p className="text-xs text-muted-foreground">
            Successfully closed deals
          </p>
        </CardContent>
      </Card>

      {/* Pipeline Projects */}
      <Card>
        <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
          <CardTitle className="text-sm font-medium">Pipeline</CardTitle>
          <Target className="h-4 w-4 text-blue-600" />
        </CardHeader>
        <CardContent>
          <div className="text-2xl font-bold text-blue-600">{stats.pipelineProjects}</div>
          <p className="text-xs text-muted-foreground">
            Active opportunities
          </p>
        </CardContent>
      </Card>

      {/* Total Pipeline Value */}
      <Card>
        <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
          <CardTitle className="text-sm font-medium">Pipeline Value</CardTitle>
          <DollarSign className="h-4 w-4 text-green-600" />
        </CardHeader>
        <CardContent>
          <div className="text-2xl font-bold">{formatCurrency(stats.totalValue)}</div>
          <p className="text-xs text-muted-foreground">
            Total estimated value
          </p>
        </CardContent>
      </Card>

      {/* Won Value */}
      <Card>
        <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
          <CardTitle className="text-sm font-medium">Revenue Generated</CardTitle>
          <TrendingUp className="h-4 w-4 text-green-600" />
        </CardHeader>
        <CardContent>
          <div className="text-2xl font-bold text-green-600">{formatCurrency(stats.wonValue)}</div>
          <p className="text-xs text-muted-foreground">
            From won projects
          </p>
        </CardContent>
      </Card>

      {/* Win Rate */}
      <Card>
        <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
          <CardTitle className="text-sm font-medium">Win Rate</CardTitle>
          <Target className="h-4 w-4 text-purple-600" />
        </CardHeader>
        <CardContent>
          <div className="text-2xl font-bold text-purple-600">{formatPercentage(stats.winRate)}</div>
          <p className="text-xs text-muted-foreground">
            Success rate
          </p>
        </CardContent>
      </Card>

      {/* Average Deal Size */}
      <Card>
        <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
          <CardTitle className="text-sm font-medium">Avg Deal Size</CardTitle>
          <DollarSign className="h-4 w-4 text-orange-600" />
        </CardHeader>
        <CardContent>
          <div className="text-2xl font-bold text-orange-600">{formatCurrency(stats.averageDealSize)}</div>
          <p className="text-xs text-muted-foreground">
            Per won project
          </p>
        </CardContent>
      </Card>

      {/* Business Units */}
      <Card>
        <CardHeader className="flex flex-row items-center justify-between space-y-0 pb-2">
          <CardTitle className="text-sm font-medium">Business Units</CardTitle>
          <Building className="h-4 w-4 text-indigo-600" />
        </CardHeader>
        <CardContent>
          <div className="text-2xl font-bold text-indigo-600">{stats.activeBusinessUnits}</div>
          <p className="text-xs text-muted-foreground">
            Active units
          </p>
        </CardContent>
      </Card>
    </div>
  );
}
