'use client';

import { useEffect, useState } from 'react';
import { motion } from 'framer-motion';
import { useRouter } from 'next/navigation';
import {
  ArrowLeft, User, Mail, Phone, MapPin, Calendar, BookOpen,
  Activity, DollarSign, FileText, Edit, Printer
} from 'lucide-react';
import api from '@/lib/api';
import { Student } from '@/types';
import { Avatar, Badge, LoadingSpinner } from '@/components/ui/Common';
import { formatDate, studentStatusMap, genderMap } from '@/lib/utils';
import Link from 'next/link';
import toast from 'react-hot-toast';

export default function StudentDetailPage({ params }: { params: { id: string } }) {
  const router = useRouter();
  const [student, setStudent] = useState<Student | null>(null);
  const [attendance, setAttendance] = useState<any>(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    Promise.all([
      api.get(`/students/${params.id}`),
      api.get(`/students/${params.id}/attendance`)
    ]).then(([studentRes, attRes]) => {
      setStudent(studentRes.data.data);
      setAttendance(attRes.data.data);
    }).catch(() => {
      toast.error('Failed to load student');
      router.push('/students');
    }).finally(() => setLoading(false));
  }, [params.id, router]);

  if (loading) return <LoadingSpinner size="lg" className="py-20" />;
  if (!student) return null;

  const status = studentStatusMap[student.status];

  const infoItems = [
    { label: 'Full Name', value: student.fullName, icon: User },
    { label: 'Email', value: student.email, icon: Mail },
    { label: 'Phone', value: student.phone || '—', icon: Phone },
    { label: 'Date of Birth', value: student.dateOfBirth ? formatDate(student.dateOfBirth) : '—', icon: Calendar },
    { label: 'Gender', value: genderMap[student.gender] || '—', icon: User },
    { label: 'Blood Group', value: student.bloodGroup || '—', icon: Activity },
    { label: 'Address', value: [student.address, student.city, student.state].filter(Boolean).join(', ') || '—', icon: MapPin },
    { label: 'Class', value: student.className ? `${student.className}${student.sectionName ? ' - ' + student.sectionName : ''}` : '—', icon: BookOpen },
  ];

  return (
    <div>
      {/* Header */}
      <motion.div initial={{ opacity: 0, y: -10 }} animate={{ opacity: 1, y: 0 }} className="flex items-center justify-between mb-6">
        <div className="flex items-center gap-4">
          <button onClick={() => router.back()}
            className="w-9 h-9 rounded-xl hover:bg-slate-100 dark:hover:bg-slate-800 flex items-center justify-center text-slate-500 transition-colors">
            <ArrowLeft className="w-5 h-5" />
          </button>
          <div>
            <h1 className="text-2xl font-bold text-slate-900 dark:text-white">Student Profile</h1>
            <p className="text-sm text-slate-500">{student.admissionNumber}</p>
          </div>
        </div>
        <div className="flex gap-2">
          <button className="btn-secondary">
            <Printer className="w-4 h-4" /> Print ID
          </button>
          <Link href={`/students/${params.id}/edit`}>
            <button id="edit-student-btn" className="btn-primary">
              <Edit className="w-4 h-4" /> Edit
            </button>
          </Link>
        </div>
      </motion.div>

      <div className="grid grid-cols-1 xl:grid-cols-3 gap-4">
        {/* Profile Card */}
        <motion.div initial={{ opacity: 0, x: -20 }} animate={{ opacity: 1, x: 0 }} className="card p-6 text-center">
          <div className="flex justify-center mb-4">
            <Avatar name={student.fullName} src={student.profilePhoto} size="lg" />
          </div>
          <h2 className="text-xl font-bold text-slate-900 dark:text-white mb-1">{student.fullName}</h2>
          <p className="text-sm text-slate-500 mb-3">{student.email}</p>
          <Badge variant={status?.color?.replace('badge-', '') as any} className="mb-4">{status?.label}</Badge>

          <div className="grid grid-cols-2 gap-3 mt-4">
            {[
              { label: 'Attendance', value: attendance ? `${attendance.percentage}%` : '—', color: 'text-emerald-600' },
              { label: 'Class', value: student.className || '—', color: 'text-blue-600' },
              { label: 'Present', value: attendance?.presentDays ?? '—', color: 'text-emerald-600' },
              { label: 'Absent', value: attendance?.absentDays ?? '—', color: 'text-red-600' },
            ].map(({ label, value, color }) => (
              <div key={label} className="bg-slate-50 dark:bg-slate-800/50 rounded-xl p-3">
                <div className={`text-lg font-bold ${color}`}>{value}</div>
                <div className="text-xs text-slate-400">{label}</div>
              </div>
            ))}
          </div>
        </motion.div>

        {/* Details */}
        <motion.div initial={{ opacity: 0, y: 20 }} animate={{ opacity: 1, y: 0 }} transition={{ delay: 0.1 }} className="card p-6 xl:col-span-2">
          <h3 className="font-semibold text-slate-900 dark:text-white mb-4">Personal Information</h3>
          <div className="grid grid-cols-1 sm:grid-cols-2 gap-4">
            {infoItems.map(({ label, value, icon: Icon }) => (
              <div key={label} className="flex items-start gap-3">
                <div className="w-8 h-8 rounded-lg bg-blue-50 dark:bg-blue-950/30 flex items-center justify-center flex-shrink-0">
                  <Icon className="w-4 h-4 text-blue-600 dark:text-blue-400" />
                </div>
                <div>
                  <div className="text-xs text-slate-400 font-medium">{label}</div>
                  <div className="text-sm text-slate-700 dark:text-slate-300 mt-0.5">{value}</div>
                </div>
              </div>
            ))}
          </div>

          {/* Parent Info */}
          {(student.parentName || student.parentPhone) && (
            <div className="mt-6 pt-6 border-t border-slate-100 dark:border-slate-800">
              <h3 className="font-semibold text-slate-900 dark:text-white mb-3">Parent / Guardian</h3>
              <div className="flex items-center gap-3">
                <Avatar name={student.parentName || 'Parent'} size="sm" />
                <div>
                  <div className="text-sm font-medium text-slate-900 dark:text-white">{student.parentName}</div>
                  <div className="text-xs text-slate-400">{student.parentPhone}</div>
                </div>
              </div>
            </div>
          )}
        </motion.div>

        {/* Attendance Breakdown */}
        {attendance && (
          <motion.div initial={{ opacity: 0, y: 20 }} animate={{ opacity: 1, y: 0 }} transition={{ delay: 0.2 }} className="card p-6 xl:col-span-3">
            <h3 className="font-semibold text-slate-900 dark:text-white mb-4">Attendance Summary</h3>
            <div className="grid grid-cols-2 sm:grid-cols-4 gap-4">
              {[
                { label: 'Total Days', value: attendance.totalDays, color: 'bg-blue-50 dark:bg-blue-950/30 text-blue-600 dark:text-blue-400' },
                { label: 'Present', value: attendance.presentDays, color: 'bg-emerald-50 dark:bg-emerald-950/30 text-emerald-600 dark:text-emerald-400' },
                { label: 'Absent', value: attendance.absentDays, color: 'bg-red-50 dark:bg-red-950/30 text-red-600 dark:text-red-400' },
                { label: 'Percentage', value: `${attendance.percentage}%`, color: 'bg-purple-50 dark:bg-purple-950/30 text-purple-600 dark:text-purple-400' },
              ].map(({ label, value, color }) => (
                <div key={label} className={`rounded-2xl p-4 ${color}`}>
                  <div className="text-2xl font-bold">{value}</div>
                  <div className="text-sm opacity-70 mt-0.5">{label}</div>
                </div>
              ))}
            </div>

            {/* Attendance bar */}
            <div className="mt-4">
              <div className="flex justify-between text-xs text-slate-500 mb-1">
                <span>Attendance Rate</span>
                <span>{attendance.percentage}%</span>
              </div>
              <div className="h-2 bg-slate-100 dark:bg-slate-800 rounded-full overflow-hidden">
                <motion.div
                  initial={{ width: 0 }}
                  animate={{ width: `${attendance.percentage}%` }}
                  transition={{ duration: 1, delay: 0.3, ease: 'easeOut' }}
                  className={`h-full rounded-full ${attendance.percentage >= 75 ? 'bg-emerald-500' : attendance.percentage >= 50 ? 'bg-amber-500' : 'bg-red-500'}`}
                />
              </div>
            </div>
          </motion.div>
        )}
      </div>
    </div>
  );
}
