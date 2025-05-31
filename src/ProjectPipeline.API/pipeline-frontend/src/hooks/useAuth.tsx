'use client';

import { useState, useEffect, createContext, useContext, ReactNode } from 'react';
import { User, LoginRequest, AuthResponse } from '@/types';
import apiClient from '@/lib/api';

interface AuthContextType {
  user: User | null;
  login: (credentials: LoginRequest) => Promise<boolean>;
  logout: () => void;
  loading: boolean;
  isAuthenticated: boolean;
}

const AuthContext = createContext<AuthContextType | undefined>(undefined);

export function AuthProvider({ children }: { children: ReactNode }) {
  const [user, setUser] = useState<User | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    checkAuth();
  }, []);

  const checkAuth = async () => {
    try {
      const token = localStorage.getItem('token');
      if (token) {
        const response = await apiClient.get('/auth/me');
        if (response.data.isSuccess) {
          setUser(response.data.data);
        }
      }
    } catch (error) {
      localStorage.removeItem('token');
    } finally {
      setLoading(false);
    }
  };

  const login = async (credentials: LoginRequest): Promise<boolean> => {
    try {
      const response = await apiClient.post<AuthResponse>('/auth/login', credentials);
      
      if (response.data.isSuccess) {
        localStorage.setItem('token', response.data.token);
        setUser(response.data.user);
        return true;
      }
      return false;
    } catch (error) {
      return false;
    }
  };

  const logout = () => {
    localStorage.removeItem('token');
    setUser(null);
    window.location.href = '/login';
  };

  const value = {
    user,
    login,
    logout,
    loading,
    isAuthenticated: !!user,
  };

  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
}

export function useAuth() {
  const context = useContext(AuthContext);
  if (context === undefined) {
    throw new Error('useAuth must be used within an AuthProvider');
  }
  return context;
}
