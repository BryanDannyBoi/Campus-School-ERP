'use client';

import { useState } from 'react';
import Link from 'next/link';
import { usePathname } from 'next/navigation';
import { motion, AnimatePresence } from 'framer-motion';
import {
  GraduationCap, LayoutDashboard, Users, UserCheck, UserCircle,
  BookOpen, Calendar, ClipboardList, FileText, DollarSign,
  Library, Bell, MessageSquare, BarChart3, Settings, ChevronLeft,
  ChevronRight, Building2, BookMarked, Clock
} from 'lucide-react';
import { useAuth } from '@/contexts/AuthContext';
import { UserRole } from '@/types';
import { cn } from '@/lib/utils';

interface NavItem {
  label: string;
  href: string;
  icon: React.ElementType;
  roles?: UserRole[];
  badge?: number;
}

const navItems: NavItem[] = [
  { label: 'Dashboard', href: '/dashboard', icon: LayoutDashboard },
  { label: 'Students', href: '/students', icon: Users, roles: ['SuperAdmin', 'SchoolAdmin', 'Principal', 'Teacher', 'Accountant'] },
  { label: 'Teachers', href: '/teachers', icon: UserCheck, roles: ['SuperAdmin', 'SchoolAdmin', 'Principal'] },
  { label: 'Parents', href: '/parents', icon: UserCircle, roles: ['SuperAdmin', 'SchoolAdmin', 'Principal', 'Teacher'] },
  { label: 'Departments', href: '/departments', icon: Building2, roles: ['SuperAdmin', 'SchoolAdmin', 'Principal'] },
  { label: 'Classes', href: '/classes', icon: BookOpen, roles: ['SuperAdmin', 'SchoolAdmin', 'Principal', 'Teacher'] },
  { label: 'Subjects', href: '/subjects', icon: BookMarked, roles: ['SuperAdmin', 'SchoolAdmin', 'Principal', 'Teacher'] },
  { label: 'Timetable', href: '/timetable', icon: Clock, roles: ['SuperAdmin', 'SchoolAdmin', 'Principal', 'Teacher', 'Student'] },
  { label: 'Attendance', href: '/attendance', icon: ClipboardList, roles: ['SuperAdmin', 'SchoolAdmin', 'Principal', 'Teacher', 'Student', 'Parent'] },
  { label: 'Assignments', href: '/assignments', icon: FileText, roles: ['SuperAdmin', 'SchoolAdmin', 'Principal', 'Teacher', 'Student'] },
  { label: 'Examinations', href: '/exams', icon: Calendar, roles: ['SuperAdmin', 'SchoolAdmin', 'Principal', 'Teacher', 'Student', 'Parent'] },
  { label: 'Fee Management', href: '/fees', icon: DollarSign, roles: ['SuperAdmin', 'SchoolAdmin', 'Principal', 'Accountant', 'Student', 'Parent'] },
  { label: 'Library', href: '/library', icon: Library, roles: ['SuperAdmin', 'SchoolAdmin', 'Principal', 'Librarian', 'Student', 'Teacher'] },
  { label: 'Notifications', href: '/notifications', icon: Bell },
  { label: 'Messages', href: '/messages', icon: MessageSquare },
  { label: 'Reports', href: '/reports', icon: BarChart3, roles: ['SuperAdmin', 'SchoolAdmin', 'Principal', 'Accountant'] },
  { label: 'Settings', href: '/settings', icon: Settings, roles: ['SuperAdmin', 'SchoolAdmin'] },
];

interface SidebarProps {
  collapsed: boolean;
  onToggle: () => void;
}

export default function Sidebar({ collapsed, onToggle }: SidebarProps) {
  const pathname = usePathname();
  const { user } = useAuth();

  const filteredItems = navItems.filter(item =>
    !item.roles || (user && item.roles.includes(user.role))
  );

  return (
    <motion.aside
      animate={{ width: collapsed ? 72 : 256 }}
      transition={{ duration: 0.3, ease: 'easeInOut' }}
      className="fixed left-0 top-0 h-full z-30 bg-white dark:bg-slate-900 border-r border-slate-200 dark:border-slate-800 flex flex-col overflow-hidden"
    >
      {/* Logo */}
      <div className="flex items-center justify-between px-4 py-5 border-b border-slate-200 dark:border-slate-800">
        <AnimatePresence mode="wait">
          {!collapsed && (
            <motion.div
              key="logo"
              initial={{ opacity: 0, x: -10 }}
              animate={{ opacity: 1, x: 0 }}
              exit={{ opacity: 0, x: -10 }}
              className="flex items-center gap-3"
            >
              <div className="w-9 h-9 rounded-xl overflow-hidden flex items-center justify-center flex-shrink-0 bg-white border border-slate-100 dark:border-slate-800 p-0.5">
                <img src="/mcc_logo.png" alt="MCC Logo" className="w-full h-full object-contain" />
              </div>
              <div>
                <div className="font-bold text-slate-900 dark:text-white text-sm leading-none">MCC Campus</div>
                <div className="text-slate-400 text-xs">Matric. Hr. Sec. School</div>
              </div>
            </motion.div>
          )}
        </AnimatePresence>
        {collapsed && (
          <div className="w-9 h-9 rounded-xl overflow-hidden flex items-center justify-center mx-auto bg-white border border-slate-100 dark:border-slate-800 p-0.5">
            <img src="/mcc_logo.png" alt="MCC Logo" className="w-full h-full object-contain" />
          </div>
        )}
        <button
          onClick={onToggle}
          className={cn(
            "w-6 h-6 rounded-full bg-slate-100 dark:bg-slate-800 hover:bg-slate-200 dark:hover:bg-slate-700 flex items-center justify-center text-slate-500 transition-all",
            collapsed && "mx-auto"
          )}
        >
          {collapsed ? <ChevronRight className="w-3.5 h-3.5" /> : <ChevronLeft className="w-3.5 h-3.5" />}
        </button>
      </div>

      {/* Navigation */}
      <nav className="flex-1 overflow-y-auto py-4 px-2 space-y-0.5">
        {filteredItems.map((item) => {
          const isActive = pathname === item.href || pathname.startsWith(item.href + '/');
          const Icon = item.icon;

          return (
            <Link key={item.href} href={item.href} id={`nav-${item.label.toLowerCase().replace(/\s/g, '-')}`}>
              <motion.div
                whileHover={{ x: collapsed ? 0 : 2 }}
                className={cn(
                  'sidebar-link',
                  isActive && 'active',
                  collapsed && 'justify-center px-0 py-3'
                )}
              >
                <div className={cn(
                  "flex-shrink-0",
                  isActive ? "text-blue-600 dark:text-blue-400" : "text-slate-500 dark:text-slate-400"
                )}>
                  <Icon className="w-5 h-5" />
                </div>
                <AnimatePresence>
                  {!collapsed && (
                    <motion.span
                      initial={{ opacity: 0 }}
                      animate={{ opacity: 1 }}
                      exit={{ opacity: 0 }}
                      className="flex-1 truncate"
                    >
                      {item.label}
                    </motion.span>
                  )}
                </AnimatePresence>
                {!collapsed && item.badge && (
                  <span className="px-1.5 py-0.5 text-xs font-medium bg-blue-100 text-blue-600 rounded-full">
                    {item.badge}
                  </span>
                )}
              </motion.div>
            </Link>
          );
        })}
      </nav>

      {/* User info at bottom */}
      {user && (
        <div className={cn(
          "border-t border-slate-200 dark:border-slate-800 p-3",
          collapsed ? "flex justify-center" : ""
        )}>
          {!collapsed ? (
            <div className="flex items-center gap-3 px-2 py-2 rounded-xl hover:bg-slate-50 dark:hover:bg-slate-800 transition-colors">
              <div className="w-8 h-8 rounded-full bg-blue-100 dark:bg-blue-950 flex items-center justify-center flex-shrink-0">
                <span className="text-blue-600 dark:text-blue-400 text-xs font-bold">
                  {user.firstName[0]}{user.lastName[0]}
                </span>
              </div>
              <div className="flex-1 min-w-0">
                <div className="text-sm font-medium text-slate-900 dark:text-white truncate">
                  {user.firstName} {user.lastName}
                </div>
                <div className="text-xs text-slate-500 truncate">{user.role}</div>
              </div>
            </div>
          ) : (
            <div className="w-8 h-8 rounded-full bg-blue-100 dark:bg-blue-950 flex items-center justify-center">
              <span className="text-blue-600 dark:text-blue-400 text-xs font-bold">
                {user.firstName[0]}{user.lastName[0]}
              </span>
            </div>
          )}
        </div>
      )}
    </motion.aside>
  );
}
