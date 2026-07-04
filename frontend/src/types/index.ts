export interface User {
  id: string;
  firstName: string;
  lastName: string;
  email: string;
  role: UserRole;
  profilePhoto?: string;
  phoneNumber?: string;
  isActive: boolean;
}

export type UserRole =
  | 'SuperAdmin'
  | 'SchoolAdmin'
  | 'Principal'
  | 'Teacher'
  | 'Student'
  | 'Parent'
  | 'Accountant'
  | 'Librarian';

export interface AuthResponse {
  accessToken: string;
  refreshToken: string;
  expiresAt: string;
  user: User;
}

export interface ApiResponse<T> {
  success: boolean;
  message?: string;
  data?: T;
  errors?: string[];
}

export interface PagedResult<T> {
  items: T[];
  totalCount: number;
  page: number;
  pageSize: number;
  totalPages: number;
  hasPrevious: boolean;
  hasNext: boolean;
}

export interface DashboardStats {
  totalStudents: number;
  totalTeachers: number;
  totalParents: number;
  totalClasses: number;
  totalFeeCollected: number;
  totalFeePending: number;
  attendancePercentage: number;
  pendingLeaves: number;
  booksIssued: number;
  monthlyAttendance: MonthlyAttendance[];
  feeChart: FeeCollection[];
  recentActivities: RecentActivity[];
}

export interface MonthlyAttendance {
  month: string;
  present: number;
  absent: number;
}

export interface FeeCollection {
  month: string;
  collected: number;
  pending: number;
}

export interface RecentActivity {
  action: string;
  user: string;
  time: string;
  type: string;
}

export interface Student {
  id: string;
  admissionNumber: string;
  fullName: string;
  email: string;
  phone?: string;
  className?: string;
  sectionName?: string;
  profilePhoto?: string;
  gender: number;
  status: number;
  admissionDate: string;
  dateOfBirth?: string;
  bloodGroup?: string;
  address?: string;
  city?: string;
  state?: string;
  parentName?: string;
  parentPhone?: string;
}

export interface Teacher {
  id: string;
  employeeId: string;
  fullName: string;
  email: string;
  phone?: string;
  departmentName?: string;
  qualification?: string;
  specialization?: string;
  profilePhoto?: string;
  status: number;
  joiningDate: string;
  salary?: number;
  subjects?: string[];
}

export interface Notification {
  id: string;
  title: string;
  message: string;
  type: string;
  isRead: boolean;
  createdAt: string;
}

export interface ClassInfo {
  id: string;
  name: string;
  academicYear: number;
  studentCount: number;
  sections: Section[];
}

export interface Section {
  id: string;
  name: string;
  classId: string;
  classTeacherName?: string;
  maxStudents: number;
  currentStudents: number;
}
