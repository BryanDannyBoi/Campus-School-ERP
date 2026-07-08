'use client';

import { useEffect, useState } from 'react';
import { motion } from 'framer-motion';
import {
  Users, UserCheck, UserCircle, BookOpen, DollarSign,
  TrendingUp, Calendar, Bell, Activity, ChevronRight, ArrowUpRight
} from 'lucide-react';
import { AreaChart, Area, BarChart, Bar, PieChart, Pie, Cell, XAxis, YAxis, CartesianGrid, Tooltip, ResponsiveContainer, Legend } from 'recharts';
import { StatsCard } from '@/components/ui/StatsCard';
import { CardSkeleton, LoadingSpinner } from '@/components/ui/Common';
import { useAuth } from '@/contexts/AuthContext';
import api from '@/lib/api';
import { DashboardStats } from '@/types';
import { formatCurrency, formatDate, timeAgo } from '@/lib/utils';
import Link from 'next/link';

const PIE_COLORS = ['#F36D48', '#10b981', '#f59e0b', '#ef4444', '#8b5cf6'];

export default function DashboardPage() {
  const { user } = useAuth();
  const [stats, setStats] = useState<DashboardStats | null>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    api.get('/dashboard/stats').then(res => {
      setStats(res.data.data);
    }).catch(() => {}).finally(() => setLoading(false));
  }, []);

  if (loading) {
    return (
      <div>
        <div className="mb-6">
          <div className="skeleton h-7 w-64 mb-2" />
          <div className="skeleton h-4 w-40" />
        </div>
        <div className="grid grid-cols-1 sm:grid-cols-2 xl:grid-cols-4 gap-4 mb-6">
          {[...Array(4)].map((_, i) => <CardSkeleton key={i} />)}
        </div>
        <LoadingSpinner />
      </div>
    );
  }

  const statsCards = [
    {
      title: 'Total Students',
      value: stats?.totalStudents.toLocaleString() || '0',
      subtitle: 'Enrolled this year',
      icon: Users,
      gradient: 'bg-blue-600',
      trend: { value: 12, positive: true },
    },
    {
      title: 'Total Teachers',
      value: stats?.totalTeachers.toLocaleString() || '0',
      subtitle: 'Active faculty',
      icon: UserCheck,
      gradient: 'bg-emerald-600',
      trend: { value: 5, positive: true },
    },
    {
      title: 'Parents Registered',
      value: stats?.totalParents.toLocaleString() || '0',
      subtitle: 'Active accounts',
      icon: UserCircle,
      gradient: 'bg-purple-600',
    },
    {
      title: 'Total Classes',
      value: stats?.totalClasses.toLocaleString() || '0',
      subtitle: 'Academic year 2025',
      icon: BookOpen,
      gradient: 'bg-orange-500',
    },
    {
      title: 'Fee Collected',
      value: formatCurrency(stats?.totalFeeCollected || 0),
      subtitle: 'This quarter',
      icon: DollarSign,
      gradient: 'bg-teal-600',
      trend: { value: 8, positive: true },
    },
    {
      title: 'Fee Pending',
      value: formatCurrency(stats?.totalFeePending || 0),
      subtitle: 'Outstanding amount',
      icon: TrendingUp,
      gradient: 'bg-red-500',
      trend: { value: 3, positive: false },
    },
    {
      title: 'Attendance Rate',
      value: `${stats?.attendancePercentage || 0}%`,
      subtitle: 'Last 30 days',
      icon: Activity,
      gradient: 'bg-indigo-600',
      trend: { value: 2, positive: true },
    },
    {
      title: 'Books Issued',
      value: stats?.booksIssued || 0,
      subtitle: 'Currently borrowed',
      icon: BookOpen,
      gradient: 'bg-pink-600',
    },
  ];

  return (
    <div>
      {/* Header Greeting Banner */}
      <motion.div
        initial={{ opacity: 0, y: -10 }}
        animate={{ opacity: 1, y: 0 }}
        className="mb-6 overflow-hidden rounded-2xl relative border border-slate-200 dark:border-slate-800 shadow-sm"
      >
        <div 
          className="absolute inset-0 bg-cover bg-center" 
          style={{ 
            backgroundImage: 'linear-gradient(to right, rgba(36, 29, 26, 0.95), rgba(75, 16, 3, 0.45)), url("/school_arch.png")' 
          }} 
        />
        <div className="relative z-10 p-6 md:p-8 text-white">
          <span className="bg-blue-600/80 backdrop-blur-sm text-[10px] font-semibold px-2.5 py-1 rounded-full uppercase tracking-wider">
            MCC Campus Portal
          </span>
          <h1 className="text-2xl md:text-3xl font-bold mt-3">
            Good morning, {user?.firstName}! 👋
          </h1>
          <p className="text-slate-200 text-sm mt-1.5 max-w-md leading-relaxed">
            Welcome back to your dashboard. Here is a summary of activities, statistics, and pending alerts for the school today.
          </p>
        </div>
      </motion.div>

      {/* Stats Grid */}
      <div className="grid grid-cols-1 sm:grid-cols-2 xl:grid-cols-4 gap-4 mb-6">
        {statsCards.map((card, i) => (
          <StatsCard key={card.title} {...card} index={i} />
        ))}
      </div>

      {/* Charts Row */}
      <div className="grid grid-cols-1 xl:grid-cols-3 gap-4 mb-6">
        {/* Attendance Chart */}
        <motion.div
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ delay: 0.3 }}
          className="card p-5 xl:col-span-2"
        >
          <div className="flex items-center justify-between mb-5">
            <div>
              <h2 className="font-semibold text-slate-900 dark:text-white">Monthly Attendance</h2>
              <p className="text-xs text-slate-400 mt-0.5">Last 6 months overview</p>
            </div>
          </div>
          <ResponsiveContainer width="100%" height={220}>
            <AreaChart data={stats?.monthlyAttendance || []} margin={{ top: 5, right: 10, bottom: 5, left: -20 }}>
              <defs>
                <linearGradient id="presentGrad" x1="0" y1="0" x2="0" y2="1">
                  <stop offset="5%" stopColor="#F36D48" stopOpacity={0.3} />
                  <stop offset="95%" stopColor="#F36D48" stopOpacity={0} />
                </linearGradient>
                <linearGradient id="absentGrad" x1="0" y1="0" x2="0" y2="1">
                  <stop offset="5%" stopColor="#ef4444" stopOpacity={0.3} />
                  <stop offset="95%" stopColor="#ef4444" stopOpacity={0} />
                </linearGradient>
              </defs>
              <CartesianGrid strokeDasharray="3 3" stroke="#f1f5f9" />
              <XAxis dataKey="month" tick={{ fontSize: 11, fill: '#94a3b8' }} />
              <YAxis tick={{ fontSize: 11, fill: '#94a3b8' }} />
              <Tooltip contentStyle={{ borderRadius: '12px', border: 'none', boxShadow: '0 10px 25px -5px rgba(0,0,0,0.1)' }} />
              <Legend wrapperStyle={{ fontSize: '12px' }} />
              <Area type="monotone" dataKey="present" name="Present" stroke="#F36D48" fill="url(#presentGrad)" strokeWidth={2} />
              <Area type="monotone" dataKey="absent" name="Absent" stroke="#ef4444" fill="url(#absentGrad)" strokeWidth={2} />
            </AreaChart>
          </ResponsiveContainer>
        </motion.div>

        {/* Fee Chart */}
        <motion.div
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ delay: 0.4 }}
          className="card p-5"
        >
          <div className="mb-5">
            <h2 className="font-semibold text-slate-900 dark:text-white">Fee Collection</h2>
            <p className="text-xs text-slate-400 mt-0.5">Last 4 months</p>
          </div>
          <ResponsiveContainer width="100%" height={220}>
            <BarChart data={stats?.feeChart || []} margin={{ top: 5, right: 10, bottom: 5, left: -20 }}>
              <CartesianGrid strokeDasharray="3 3" stroke="#f1f5f9" />
              <XAxis dataKey="month" tick={{ fontSize: 11, fill: '#94a3b8' }} />
              <YAxis tick={{ fontSize: 11, fill: '#94a3b8' }} />
              <Tooltip contentStyle={{ borderRadius: '12px', border: 'none', boxShadow: '0 10px 25px -5px rgba(0,0,0,0.1)' }} />
              <Bar dataKey="collected" name="Collected" fill="#F36D48" radius={[4, 4, 0, 0]} />
            </BarChart>
          </ResponsiveContainer>
        </motion.div>
      </div>

      {/* Bottom Row */}
      <div className="grid grid-cols-1 xl:grid-cols-3 gap-4">
        {/* Recent Activity */}
        <motion.div
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ delay: 0.5 }}
          className="card p-5 flex flex-col justify-between"
        >
          <div>
            <div className="flex items-center justify-between mb-4">
              <h2 className="font-semibold text-slate-900 dark:text-white">Recent Activity</h2>
              <Link href="/reports" className="text-xs text-blue-600 hover:text-blue-700 flex items-center gap-1">
                View all <ChevronRight className="w-3 h-3" />
              </Link>
            </div>
            <div className="space-y-3">
              {(stats?.recentActivities || []).slice(0, 6).map((activity, i) => (
                <div key={i} className="flex items-start gap-3">
                  <div className="w-8 h-8 rounded-full bg-blue-100 dark:bg-blue-950/50 flex items-center justify-center flex-shrink-0 mt-0.5">
                    <Activity className="w-4 h-4 text-blue-600 dark:text-blue-400" />
                  </div>
                  <div className="flex-1 min-w-0">
                    <div className="text-sm text-slate-700 dark:text-slate-300 truncate">{activity.action}</div>
                    <div className="text-xs text-slate-400">{activity.user} · {timeAgo(activity.time)}</div>
                  </div>
                </div>
              ))}
              {(!stats?.recentActivities || stats.recentActivities.length === 0) && (
                <p className="text-sm text-slate-400 text-center py-4">No recent activity</p>
              )}
            </div>
          </div>
        </motion.div>
 
        {/* Quick Actions */}
        <motion.div
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ delay: 0.6 }}
          className="card p-5"
        >
          <h2 className="font-semibold text-slate-900 dark:text-white mb-4">Quick Actions</h2>
          <div className="grid grid-cols-2 gap-3">
            {[
              { label: 'Admit Student', href: '/students/new', color: 'bg-blue-50 dark:bg-blue-950/30 text-blue-600 dark:text-blue-400 hover:bg-blue-100 dark:hover:bg-blue-950/50', icon: Users },
              { label: 'Add Teacher', href: '/teachers/new', color: 'bg-emerald-50 dark:bg-emerald-950/30 text-emerald-600 dark:text-emerald-400 hover:bg-emerald-100 dark:hover:bg-emerald-950/50', icon: UserCheck },
              { label: 'Mark Attendance', href: '/attendance', color: 'bg-purple-50 dark:bg-purple-950/30 text-purple-600 dark:text-purple-400 hover:bg-purple-100 dark:hover:bg-purple-950/50', icon: Activity },
              { label: 'Collect Fee', href: '/fees', color: 'bg-orange-50 dark:bg-orange-950/30 text-orange-600 dark:text-orange-400 hover:bg-orange-100 dark:hover:bg-orange-950/50', icon: DollarSign },
              { label: 'Send Notice', href: '/notifications', color: 'bg-pink-50 dark:bg-pink-950/30 text-pink-600 dark:text-pink-400 hover:bg-pink-100 dark:hover:bg-pink-950/50', icon: Bell },
              { label: 'View Reports', href: '/reports', color: 'bg-indigo-50 dark:bg-indigo-950/30 text-indigo-600 dark:text-indigo-400 hover:bg-indigo-100 dark:hover:bg-indigo-950/50', icon: TrendingUp },
            ].map(({ label, href, color, icon: Icon }) => (
              <Link key={label} href={href}>
                <motion.div whileHover={{ scale: 1.02 }} whileTap={{ scale: 0.98 }}
                  className={`flex items-center gap-3 p-4 rounded-xl transition-colors cursor-pointer ${color}`}>
                  <Icon className="w-5 h-5 flex-shrink-0" />
                  <span className="text-sm font-medium">{label}</span>
                </motion.div>
              </Link>
            ))}
          </div>
        </motion.div>

        {/* Campus Gallery Card */}
        <motion.div
          initial={{ opacity: 0, y: 20 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ delay: 0.7 }}
          className="card overflow-hidden flex flex-col justify-between"
        >
          <div>
            <div className="relative h-40 overflow-hidden">
              <img src="/school_courtyard.jpg" alt="MCC Courtyard" className="w-full h-full object-cover transition-transform duration-500 hover:scale-105" />
              <div className="absolute top-3 left-3 bg-black/60 backdrop-blur-sm text-[10px] text-white font-bold px-2 py-0.5 rounded-full uppercase tracking-wider">
                Campus View
              </div>
            </div>
            <div className="p-5">
              <h2 className="font-semibold text-slate-900 dark:text-white mb-2">MCC Campus Highlights</h2>
              <p className="text-xs text-slate-500 dark:text-slate-400 leading-relaxed">
                Our campus features state-of-the-art academic blocks, lush green lawns, and recreational zones designed for a holistic educational experience.
              </p>
            </div>
          </div>
          <div className="p-5 pt-0 flex gap-2">
            <img src="/school_building.jpg" alt="Building" className="w-1/2 h-16 object-cover rounded-lg border border-slate-100 dark:border-slate-800" />
            <img src="/school_aerial.jpg" alt="Aerial" className="w-1/2 h-16 object-cover rounded-lg border border-slate-100 dark:border-slate-800" />
          </div>
        </motion.div>
      </div>
    </div>
  );
}
