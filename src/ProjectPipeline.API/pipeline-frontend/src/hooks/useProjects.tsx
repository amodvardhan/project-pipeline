'use client';

import { useState, useEffect } from 'react';
import { Project, ApiResponse, PaginatedResult } from '@/types';
import apiClient from '@/lib/api';

export function useProjects(page: number = 1, pageSize: number = 10, status?: string, businessUnitId?: number) {
  const [projects, setProjects] = useState<Project[]>([]); // Initialize as empty array
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [totalCount, setTotalCount] = useState(0);
  const [totalPages, setTotalPages] = useState(0);

  useEffect(() => {
    fetchProjects();
  }, [page, pageSize, status, businessUnitId]);

  const fetchProjects = async () => {
    try {
      setLoading(true);
      setError(null);

      let url = `/projects?page=${page}&pageSize=${pageSize}`;
      
      // If filtering by status
      if (status && status !== 'all') {
        url = `/projects/status/${status}`;
      }
      
      // If filtering by business unit
      if (businessUnitId) {
        url = `/projects/business-unit/${businessUnitId}`;
      }

      const response = await apiClient.get(url);
      
      if (response.data.isSuccess && response.data.data) {
        // Check if it's a paginated result or direct array
        if (Array.isArray(response.data.data)) {
          // Direct array (from status or business unit filtering)
          setProjects(response.data.data);
          setTotalCount(response.data.data.length);
          setTotalPages(1);
        } else if (response.data.data.items) {
          // Paginated result
          const paginatedData = response.data.data as PaginatedResult<Project>;
          setProjects(paginatedData.items || []);
          setTotalCount(paginatedData.totalCount || 0);
          setTotalPages(paginatedData.totalPages || 1);
        } else {
          // Fallback
          setProjects([]);
          setTotalCount(0);
          setTotalPages(1);
        }
      } else {
        setProjects([]);
        setTotalCount(0);
        setTotalPages(1);
      }
    } catch (err: any) {
      console.error('API Error:', err);
      setError(err.response?.data?.message || 'Failed to fetch projects');
      setProjects([]); // Ensure it's always an array
      setTotalCount(0);
      setTotalPages(1);
    } finally {
      setLoading(false);
    }
  };

  const refreshProjects = () => {
    fetchProjects();
  };

  return {
    projects: projects || [], // Ensure it's never undefined
    loading,
    error,
    totalCount,
    totalPages,
    refreshProjects
  };
}

export function useBusinessUnits() {
  const [businessUnits, setBusinessUnits] = useState<any[]>([]); // Initialize as empty array
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    fetchBusinessUnits();
  }, []);

  const fetchBusinessUnits = async () => {
    try {
      const response = await apiClient.get('/business-units');
      if (response.data.isSuccess && response.data.data) {
        setBusinessUnits(Array.isArray(response.data.data) ? response.data.data : []);
      } else {
        setBusinessUnits([]);
      }
    } catch (error) {
      console.error('Failed to fetch business units:', error);
      setBusinessUnits([]); // Ensure it's always an array
    } finally {
      setLoading(false);
    }
  };

  return { businessUnits: businessUnits || [], loading }; // Ensure it's never undefined
}
