'use client';

import { useEffect, useState, useCallback } from 'react';
import { motion } from 'framer-motion';
import { Plus, Search, Filter, Eye, Edit, UserCheck } from 'lucide-react';
import Link from 'next/link';
import api from '@/lib/api';
import { Teacher, PagedResult } from '@/types';
import { Badge, Avatar, EmptyState, LoadingSpinner } from '@/components/ui/Common';
import { formatDate, teacherStatusMap } from '@/lib/utils';
import toast from 'react-hot-toast';

export default function TeachersPage() {
  const [teachers, setTeachers] = useState<Teacher[]>([]);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [search, setSearch] = useState('');
  const [loading, setLoading] = useState(true);

  const fetchTeachers = useCallback(async () => {
    setLoading(true);
    try {
      const res = await api.get('/teachers', { params: { page, pageSize: 10, search: search || undefined } });
      const data: PagedResult<Teacher> = res.data.data;
      setTeachers(data.items);
      setTotal(data.totalCount);
    } catch { toast.error('Failed to load teachers'); }
    finally { setLoading(false); }
  }, [page, search]);

  useEffect(() => { fetchTeachers(); }, [fetchTeachers]);

  return (
    <div>
      <motion.div initial={{ opacity: 0, y: -10 }} animate={{ opacity: 1, y: 0 }} className="flex items-center justify-between mb-6">
        <div>
          <h1 className="text-2xl font-bold text-slate-900 dark:text-white">Teachers</h1>
          <p className="text-sm text-slate-500 mt-0.5">{total} faculty members</p>
        </div>
        <Link href="/teachers/new">
          <button id="add-teacher-btn" className="btn-primary">
            <Plus className="w-4 h-4" /> Add Teacher
          </button>
        </Link>
      </motion.div>

      <motion.div initial={{ opacity: 0 }} animate={{ opacity: 1 }} transition={{ delay: 0.1 }} className="card p-4 mb-4">
        <div className="flex gap-3">
          <div className="relative flex-1">
            <Search className="absolute left-3.5 top-1/2 -translate-y-1/2 w-4 h-4 text-slate-400" />
            <input
              id="teacher-search"
              type="text"
              placeholder="Search teachers..."
              value={search}
              onChange={e => { setSearch(e.target.value); setPage(1); }}
              className="form-input pl-10 w-full"
            />
          </div>
          <button className="btn-secondary"><Filter className="w-4 h-4" /> Filter</button>
        </div>
      </motion.div>

      <motion.div initial={{ opacity: 0, y: 10 }} animate={{ opacity: 1, y: 0 }} transition={{ delay: 0.2 }} className="card overflow-hidden">
        {loading ? <LoadingSpinner /> : teachers.length === 0 ? (
          <EmptyState icon={UserCheck} title="No teachers found" description="Add your first teacher to get started" />
        ) : (
          <div className="overflow-x-auto">
            <table className="w-full data-table">
              <thead>
                <tr>
                  <th>Teacher</th>
                  <th>Employee ID</th>
                  <th>Department</th>
                  <th>Specialization</th>
                  <th>Joining Date</th>
                  <th>Status</th>
                  <th>Actions</th>
                </tr>
              </thead>
              <tbody>
                {teachers.map((t, i) => {
                  const status = teacherStatusMap[t.status];
                  return (
                    <motion.tr key={t.id} initial={{ opacity: 0, x: -10 }} animate={{ opacity: 1, x: 0 }} transition={{ delay: i * 0.03 }}>
                      <td>
                        <div className="flex items-center gap-3">
                          <Avatar name={t.fullName} src={t.profilePhoto} size="sm" />
                          <div>
                            <div className="font-medium text-slate-900 dark:text-white text-sm">{t.fullName}</div>
                            <div className="text-xs text-slate-400">{t.email}</div>
                          </div>
                        </div>
                      </td>
                      <td><span className="font-mono text-xs bg-slate-100 dark:bg-slate-800 px-2 py-0.5 rounded">{t.employeeId}</span></td>
                      <td><span className="text-sm text-slate-600 dark:text-slate-400">{t.departmentName || '—'}</span></td>
                      <td><span className="text-sm text-slate-600 dark:text-slate-400">{t.specialization || '—'}</span></td>
                      <td><span className="text-sm text-slate-500">{formatDate(t.joiningDate)}</span></td>
                      <td><Badge variant={status?.color?.replace('badge-', '') as any}>{status?.label}</Badge></td>
                      <td>
                        <div className="flex gap-1">
                          <Link href={`/teachers/${t.id}`}>
                            <button id={`view-teacher-${t.id}`} className="w-8 h-8 rounded-lg hover:bg-blue-50 text-slate-400 hover:text-blue-600 flex items-center justify-center transition-colors">
                              <Eye className="w-4 h-4" />
                            </button>
                          </Link>
                          <Link href={`/teachers/${t.id}/edit`}>
                            <button id={`edit-teacher-${t.id}`} className="w-8 h-8 rounded-lg hover:bg-emerald-50 text-slate-400 hover:text-emerald-600 flex items-center justify-center transition-colors">
                              <Edit className="w-4 h-4" />
                            </button>
                          </Link>
                        </div>
                      </td>
                    </motion.tr>
                  );
                })}
              </tbody>
            </table>
          </div>
        )}
      </motion.div>
    </div>
  );
}
