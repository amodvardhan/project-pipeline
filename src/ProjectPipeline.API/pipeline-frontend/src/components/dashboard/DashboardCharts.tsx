'use client';

import { Card, CardContent, CardDescription, CardHeader, CardTitle } from '@/components/ui/card';
import { PieChart, Pie, Cell, ResponsiveContainer, BarChart, Bar, XAxis, YAxis, CartesianGrid, Tooltip, Legend, LineChart, Line } from 'recharts';
import { useProjectDistribution, useMonthlyTrends } from '@/hooks/useDashboard';

const COLORS = {
  Pipeline: '#3B82F6',
  Won: '#10B981',
  Lost: '#EF4444',
  Missed: '#F59E0B',
  OnHold: '#6B7280',
  Cancelled: '#DC2626'
};

export function ProjectStatusChart() {
  const { distribution, loading } = useProjectDistribution();

  if (loading) {
    return (
      <Card>
        <CardHeader>
          <CardTitle>Project Status Distribution</CardTitle>
        </CardHeader>
        <CardContent>
          <div className="h-64 flex items-center justify-center">
            <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-gray-900"></div>
          </div>
        </CardContent>
      </Card>
    );
  }

  return (
    <Card>
      <CardHeader>
        <CardTitle>Project Status Distribution</CardTitle>
        <CardDescription>Current breakdown of all projects by status</CardDescription>
      </CardHeader>
      <CardContent>
        <div className="h-64">
          <ResponsiveContainer width="100%" height="100%">
            <PieChart>
              <Pie
                data={distribution}
                cx="50%"
                cy="50%"
                labelLine={false}
                label={({ status, percentage }) => `${status} (${percentage.toFixed(1)}%)`}
                outerRadius={80}
                fill="#8884d8"
                dataKey="count"
              >
                {distribution.map((entry, index) => (
                  <Cell key={`cell-${index}`} fill={COLORS[entry.status as keyof typeof COLORS] || '#8884d8'} />
                ))}
              </Pie>
              <Tooltip />
            </PieChart>
          </ResponsiveContainer>
        </div>
      </CardContent>
    </Card>
  );
}

export function MonthlyTrendsChart() {
  const { trends, loading } = useMonthlyTrends();

  if (loading) {
    return (
      <Card>
        <CardHeader>
          <CardTitle>Monthly Trends</CardTitle>
        </CardHeader>
        <CardContent>
          <div className="h-64 flex items-center justify-center">
            <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-gray-900"></div>
          </div>
        </CardContent>
      </Card>
    );
  }

  return (
    <Card>
      <CardHeader>
        <CardTitle>Monthly Project Trends</CardTitle>
        <CardDescription>Project outcomes over the last 6 months</CardDescription>
      </CardHeader>
      <CardContent>
        <div className="h-64">
          <ResponsiveContainer width="100%" height="100%">
            <BarChart data={trends}>
              <CartesianGrid strokeDasharray="3 3" />
              <XAxis dataKey="month" />
              <YAxis />
              <Tooltip />
              <Legend />
              <Bar dataKey="won" fill="#10B981" name="Won" />
              <Bar dataKey="lost" fill="#EF4444" name="Lost" />
              <Bar dataKey="pipeline" fill="#3B82F6" name="Pipeline" />
            </BarChart>
          </ResponsiveContainer>
        </div>
      </CardContent>
    </Card>
  );
}

export function ValueTrendsChart() {
  const { trends, loading } = useMonthlyTrends();

  if (loading) {
    return (
      <Card>
        <CardHeader>
          <CardTitle>Value Trends</CardTitle>
        </CardHeader>
        <CardContent>
          <div className="h-64 flex items-center justify-center">
            <div className="animate-spin rounded-full h-8 w-8 border-b-2 border-gray-900"></div>
          </div>
        </CardContent>
      </Card>
    );
  }

  const formatCurrency = (value: number) => {
    return new Intl.NumberFormat('en-US', {
      style: 'currency',
      currency: 'USD',
      minimumFractionDigits: 0,
      maximumFractionDigits: 0,
    }).format(value);
  };

  return (
    <Card>
      <CardHeader>
        <CardTitle>Pipeline Value Trends</CardTitle>
        <CardDescription>Total project value by month</CardDescription>
      </CardHeader>
      <CardContent>
        <div className="h-64">
          <ResponsiveContainer width="100%" height="100%">
            <LineChart data={trends}>
              <CartesianGrid strokeDasharray="3 3" />
              <XAxis dataKey="month" />
              <YAxis tickFormatter={(value) => `$${(value / 1000).toFixed(0)}K`} />
              <Tooltip formatter={(value) => [formatCurrency(value as number), 'Total Value']} />
              <Line 
                type="monotone" 
                dataKey="totalValue" 
                stroke="#8B5CF6" 
                strokeWidth={2}
                dot={{ fill: '#8B5CF6' }}
              />
            </LineChart>
          </ResponsiveContainer>
        </div>
      </CardContent>
    </Card>
  );
}
