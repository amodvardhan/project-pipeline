export interface User {
  id: string;
  email: string;
  firstName: string;
  lastName: string;
  department?: string;
  designation?: string;
  businessUnitId?: number;
  businessUnitName?: string;
  phoneNumber?: string;
  isActive: boolean;
  createdAt: string;
  lastLoginDate?: string;
}

export interface Project {
  id: number;
  name: string;
  description?: string;
  clientName: string;
  estimatedValue?: number;
  actualValue?: number;
  status: string;
  statusReason?: string;
  startDate?: string;
  endDate?: string;
  expectedClosureDate?: string;
  businessUnitId: number;
  businessUnitName?: string;
  technology: string;
  projectType: string;
  profilesSubmitted: number;
  profilesShortlisted: number;
  profilesSelected: number;
  createdAt: string;
  createdBy: string;
  createdByName?: string;
}

export interface BusinessUnit {
  id: number;
  name: string;
  code: string;
  description?: string;
  headOfUnit?: string;
  isActive: boolean;
}

export interface ApiResponse<T> {
  isSuccess: boolean;
  message: string;
  data?: T;
  errors?: string[];
  timestamp: string;
}

export interface PaginatedResult<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

export interface LoginRequest {
  email: string;
  password: string;
  rememberMe?: boolean;
}

export interface AuthResponse {
  token: string;
  refreshToken: string;
  expiresAt: string;
  user: User;
  isSuccess: boolean;
  message: string;
}
