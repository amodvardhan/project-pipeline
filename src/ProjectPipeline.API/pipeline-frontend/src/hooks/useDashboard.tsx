'use client';

import { useState, useEffect } from 'react';
import { ApiResponse } from '@/types';
import apiClient from '@/lib/api';

export interface DashboardStats {
  totalProjects: number;
  pipelineProjects: number;
  wonProjects: number;
  lostProjects: number;
  missedProjects: number;
  totalValue: number;
  wonValue: number;
  lostValue: number;
  winRate: number;
  averageDealSize: number;
  activeBusinessUnits: number;
}

export interface ProjectStatusDistribution {
  status: string;
  count: number;
  value: number;
  percentage: number;
}

export interface MonthlyTrend {
  month: string;
  year: number;
  won: number;
  lost: number;
  pipeline: number;
  totalValue: number;
}

export interface BusinessUnitPerformance {
  businessUnitName: string;
  totalProjects: number;
  wonProjects: number;
  totalValue: number;
  winRate: number;
}

export function useDashboardStats() {
  const [stats, setStats] = useState<DashboardStats | null>(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);

  useEffect(() => {
    fetchDashboardStats();
  }, []);

  const fetchDashboardStats = async () => {
    try {
      setLoading(true);
      setError(null);

      // Fetch all projects to calculate stats
      const response = await apiClient.get('/projects?page=1&pageSize=1000');
      
      if (response.data.isSuccess && response.data.data) {
        const projects = response.data.data.items || [];
        const businessUnitsResponse = await apiClient.get('/business-units');
        const businessUnits = businessUnitsResponse.data.data || [];

        // Calculate statistics
        const totalProjects = projects.length;
        const pipelineProjects = projects.filter((p: any) => p.status === 'Pipeline').length;
        const wonProjects = projects.filter((p: any) => p.status === 'Won').length;
        const lostProjects = projects.filter((p: any) => p.status === 'Lost').length;
        const missedProjects = projects.filter((p: any) => p.status === 'Missed').length;

        const totalValue = projects.reduce((sum: number, p: any) => sum + (p.estimatedValue || 0), 0);
        const wonValue = projects
          .filter((p: any) => p.status === 'Won')
          .reduce((sum: number, p: any) => sum + (p.actualValue || p.estimatedValue || 0), 0);
        const lostValue = projects
          .filter((p: any) => p.status === 'Lost')
          .reduce((sum: number, p: any) => sum + (p.estimatedValue || 0), 0);

        const winRate = totalProjects > 0 ? (wonProjects / (wonProjects + lostProjects)) * 100 : 0;
        const averageDealSize = wonProjects > 0 ? wonValue / wonProjects : 0;

        setStats({
          totalProjects,
          pipelineProjects,
          wonProjects,
          lostProjects,
          missedProjects,
          totalValue,
          wonValue,
          lostValue,
          winRate,
          averageDealSize,
          activeBusinessUnits: businessUnits.length
        });
      }
    } catch (err: any) {
      setError(err.response?.data?.message || 'Failed to fetch dashboard statistics');
    } finally {
      setLoading(false);
    }
  };

  return { stats, loading, error, refreshStats: fetchDashboardStats };
}

export function useProjectDistribution() {
  const [distribution, setDistribution] = useState<ProjectStatusDistribution[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetchDistribution();
  }, []);

  const fetchDistribution = async () => {
    try {
      const response = await apiClient.get('/projects?page=1&pageSize=1000');
      
      if (response.data.isSuccess && response.data.data) {
        const projects = response.data.data.items || [];
        
        // Group by status
        const statusGroups = projects.reduce((acc: any, project: any) => {
          const status = project.status;
          if (!acc[status]) {
            acc[status] = { count: 0, value: 0 };
          }
          acc[status].count++;
          acc[status].value += project.estimatedValue || 0;
          return acc;
        }, {});

        const total = projects.length;
        const distributionData = Object.entries(statusGroups).map(([status, data]: [string, any]) => ({
          status,
          count: data.count,
          value: data.value,
          percentage: total > 0 ? (data.count / total) * 100 : 0
        }));

        setDistribution(distributionData);
      }
    } catch (error) {
      console.error('Failed to fetch project distribution:', error);
    } finally {
      setLoading(false);
    }
  };

  return { distribution, loading };
}

export function useMonthlyTrends() {
  const [trends, setTrends] = useState<MonthlyTrend[]>([]);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetchTrends();
  }, []);

  const fetchTrends = async () => {
    try {
      const response = await apiClient.get('/projects?page=1&pageSize=1000');
      
      if (response.data.isSuccess && response.data.data) {
        const projects = response.data.data.items || [];
        
        // Group by month for the last 6 months
        const monthlyData: { [key: string]: MonthlyTrend } = {};
        const now = new Date();
        
        // Initialize last 6 months
        for (let i = 5; i >= 0; i--) {
          const date = new Date(now.getFullYear(), now.getMonth() - i, 1);
          const key = `${date.getFullYear()}-${date.getMonth()}`;
          monthlyData[key] = {
            month: date.toLocaleDateString('en-US', { month: 'short' }),
            year: date.getFullYear(),
            won: 0,
            lost: 0,
            pipeline: 0,
            totalValue: 0
          };
        }

        // Populate with actual data
        projects.forEach((project: any) => {
          const createdDate = new Date(project.createdAt);
          const key = `${createdDate.getFullYear()}-${createdDate.getMonth()}`;
          
          if (monthlyData[key]) {
            if (project.status === 'Won') monthlyData[key].won++;
            else if (project.status === 'Lost') monthlyData[key].lost++;
            else if (project.status === 'Pipeline') monthlyData[key].pipeline++;
            
            monthlyData[key].totalValue += project.estimatedValue || 0;
          }
        });

        setTrends(Object.values(monthlyData));
      }
    } catch (error) {
      console.error('Failed to fetch monthly trends:', error);
    } finally {
      setLoading(false);
    }
  };

  return { trends, loading };
}
