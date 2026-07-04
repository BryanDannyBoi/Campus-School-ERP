import { type ClassValue, clsx } from 'clsx';
import { twMerge } from 'tailwind-merge';
import { format, formatDistanceToNow } from 'date-fns';

export function cn(...inputs: ClassValue[]) {
  return twMerge(clsx(inputs));
}

export function formatDate(date: string | Date, fmt = 'MMM dd, yyyy') {
  return format(new Date(date), fmt);
}

export function timeAgo(date: string | Date) {
  return formatDistanceToNow(new Date(date), { addSuffix: true });
}

export function formatCurrency(amount: number, currency = '₹') {
  return `${currency}${amount.toLocaleString('en-IN', { minimumFractionDigits: 0 })}`;
}

export function getInitials(name: string) {
  return name.split(' ').map(n => n[0]).join('').toUpperCase().slice(0, 2);
}

export const genderMap: Record<number, string> = { 1: 'Male', 2: 'Female', 3: 'Other' };
export const studentStatusMap: Record<number, { label: string; color: string }> = {
  1: { label: 'Active', color: 'badge-green' },
  2: { label: 'Inactive', color: 'badge-gray' },
  3: { label: 'Transferred', color: 'badge-yellow' },
  4: { label: 'Graduated', color: 'badge-blue' },
  5: { label: 'Suspended', color: 'badge-red' },
};
export const teacherStatusMap: Record<number, { label: string; color: string }> = {
  1: { label: 'Active', color: 'badge-green' },
  2: { label: 'Inactive', color: 'badge-gray' },
  3: { label: 'On Leave', color: 'badge-yellow' },
  4: { label: 'Resigned', color: 'badge-red' },
};
export const attendanceStatusMap: Record<number, { label: string; color: string }> = {
  1: { label: 'Present', color: 'badge-green' },
  2: { label: 'Absent', color: 'badge-red' },
  3: { label: 'Late', color: 'badge-yellow' },
  4: { label: 'Half Day', color: 'badge-blue' },
};
export const feeStatusMap: Record<number, { label: string; color: string }> = {
  1: { label: 'Pending', color: 'badge-yellow' },
  2: { label: 'Paid', color: 'badge-green' },
  3: { label: 'Partial', color: 'badge-blue' },
  4: { label: 'Overdue', color: 'badge-red' },
  5: { label: 'Waived', color: 'badge-gray' },
};

export const roleColors: Record<string, string> = {
  SuperAdmin: 'badge-purple',
  SchoolAdmin: 'badge-blue',
  Principal: 'badge-blue',
  Teacher: 'badge-green',
  Student: 'badge-yellow',
  Parent: 'badge-gray',
  Accountant: 'badge-blue',
  Librarian: 'badge-green',
};
