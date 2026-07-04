'use client';

import { useEffect, useState, useCallback } from 'react';
import { motion } from 'framer-motion';
import { Plus, Search, Filter, Eye, Edit, Trash2, UserPlus } from 'lucide-react';
import Link from 'next/link';
import api from '@/lib/api';
import { Student, PagedResult } from '@/types';
import { Badge, Avatar, EmptyState, LoadingSpinner } from '@/components/ui/Common';
import { formatDate, studentStatusMap, genderMap } from '@/lib/utils';
import toast from 'react-hot-toast';

export default function StudentsPage() {
  const [students, setStudents] = useState<Student[]>([]);
  const [total, setTotal] = useState(0);
  const [page, setPage] = useState(1);
  const [pageSize] = useState(10);
  const [search, setSearch] = useState('');
  const [loading, setLoading] = useState(true);
  const [deleting, setDeleting] = useState<string | null>(null);

  const fetchStudents = useCallback(async () => {
    setLoading(true);
    try {
      const res = await api.get('/students', {
        params: { page, pageSize, search: search || undefined }
      });
      const data: PagedResult<Student> = res.data.data;
      setStudents(data.items);
      setTotal(data.totalCount);
    } catch { toast.error('Failed to load students'); }
    finally { setLoading(false); }
  }, [page, pageSize, search]);

  useEffect(() => { fetchStudents(); }, [fetchStudents]);

  const handleDelete = async (id: string, name: string) => {
    if (!confirm(`Are you sure you want to delete ${name}?`)) return;
    setDeleting(id);
    try {
      await api.delete(`/students/${id}`);
      toast.success('Student deleted successfully');
      fetchStudents();
    } catch { toast.error('Failed to delete student'); }
    finally { setDeleting(null); }
  };

  const totalPages = Math.ceil(total / pageSize);

  return (
    <div>
      {/* Header */}
      <motion.div initial={{ opacity: 0, y: -10 }} animate={{ opacity: 1, y: 0 }} className="flex items-center justify-between mb-6">
        <div>
          <h1 className="text-2xl font-bold text-slate-900 dark:text-white">Students</h1>
          <p className="text-sm text-slate-500 dark:text-slate-400 mt-0.5">
            {total.toLocaleString()} students enrolled
          </p>
        </div>
        <Link href="/students/new">
          <motion.button whileHover={{ scale: 1.02 }} whileTap={{ scale: 0.98 }} id="add-student-btn" className="btn-primary">
            <UserPlus className="w-4 h-4" /> Admit Student
          </motion.button>
        </Link>
      </motion.div>

      {/* Search & Filters */}
      <motion.div initial={{ opacity: 0 }} animate={{ opacity: 1 }} transition={{ delay: 0.1 }} className="card p-4 mb-4">
        <div className="flex flex-col sm:flex-row gap-3">
          <div className="relative flex-1">
            <Search className="absolute left-3.5 top-1/2 -translate-y-1/2 w-4 h-4 text-slate-400" />
            <input
              id="student-search"
              type="text"
              placeholder="Search by name, email, or admission number..."
              value={search}
              onChange={e => { setSearch(e.target.value); setPage(1); }}
              className="form-input pl-10 w-full"
            />
          </div>
          <button className="btn-secondary">
            <Filter className="w-4 h-4" /> Filter
          </button>
        </div>
      </motion.div>

      {/* Table */}
      <motion.div initial={{ opacity: 0, y: 10 }} animate={{ opacity: 1, y: 0 }} transition={{ delay: 0.2 }} className="card overflow-hidden">
        {loading ? (
          <LoadingSpinner />
        ) : students.length === 0 ? (
          <EmptyState
            icon={UserPlus}
            title="No students found"
            description={search ? `No students match "${search}"` : 'Get started by admitting your first student'}
            action={
              <Link href="/students/new">
                <button className="btn-primary">Admit First Student</button>
              </Link>
            }
          />
        ) : (
          <>
            <div className="overflow-x-auto">
              <table className="w-full data-table">
                <thead>
                  <tr>
                    <th>Student</th>
                    <th>Admission No.</th>
                    <th>Class</th>
                    <th>Gender</th>
                    <th>Status</th>
                    <th>Admission Date</th>
                    <th>Actions</th>
                  </tr>
                </thead>
                <tbody>
                  {students.map((student, i) => {
                    const status = studentStatusMap[student.status];
                    return (
                      <motion.tr
                        key={student.id}
                        initial={{ opacity: 0, x: -10 }}
                        animate={{ opacity: 1, x: 0 }}
                        transition={{ delay: i * 0.03 }}
                      >
                        <td>
                          <div className="flex items-center gap-3">
                            <Avatar name={student.fullName} src={student.profilePhoto} size="sm" />
                            <div>
                              <div className="font-medium text-slate-900 dark:text-white text-sm">{student.fullName}</div>
                              <div className="text-xs text-slate-400">{student.email}</div>
                            </div>
                          </div>
                        </td>
                        <td><span className="font-mono text-xs bg-slate-100 dark:bg-slate-800 px-2 py-0.5 rounded">{student.admissionNumber}</span></td>
                        <td>
                          {student.className ? (
                            <span className="text-sm text-slate-700 dark:text-slate-300">
                              {student.className}{student.sectionName && ` - ${student.sectionName}`}
                            </span>
                          ) : (
                            <span className="text-slate-400 text-sm">—</span>
                          )}
                        </td>
                        <td><span className="text-sm text-slate-600 dark:text-slate-400">{genderMap[student.gender] || '—'}</span></td>
                        <td><Badge variant={status?.color?.replace('badge-', '') as any}>{status?.label || 'Unknown'}</Badge></td>
                        <td><span className="text-sm text-slate-500">{formatDate(student.admissionDate)}</span></td>
                        <td>
                          <div className="flex items-center gap-1">
                            <Link href={`/students/${student.id}`}>
                              <button id={`view-student-${student.id}`} className="w-8 h-8 rounded-lg hover:bg-blue-50 dark:hover:bg-blue-950/30 text-slate-400 hover:text-blue-600 flex items-center justify-center transition-colors">
                                <Eye className="w-4 h-4" />
                              </button>
                            </Link>
                            <Link href={`/students/${student.id}/edit`}>
                              <button id={`edit-student-${student.id}`} className="w-8 h-8 rounded-lg hover:bg-emerald-50 dark:hover:bg-emerald-950/30 text-slate-400 hover:text-emerald-600 flex items-center justify-center transition-colors">
                                <Edit className="w-4 h-4" />
                              </button>
                            </Link>
                            <button
                              id={`delete-student-${student.id}`}
                              onClick={() => handleDelete(student.id, student.fullName)}
                              disabled={deleting === student.id}
                              className="w-8 h-8 rounded-lg hover:bg-red-50 dark:hover:bg-red-950/30 text-slate-400 hover:text-red-600 flex items-center justify-center transition-colors disabled:opacity-50"
                            >
                              <Trash2 className="w-4 h-4" />
                            </button>
                          </div>
                        </td>
                      </motion.tr>
                    );
                  })}
                </tbody>
              </table>
            </div>

            {/* Pagination */}
            <div className="flex items-center justify-between px-4 py-3 border-t border-slate-100 dark:border-slate-800">
              <p className="text-sm text-slate-500">
                Showing {((page - 1) * pageSize) + 1}–{Math.min(page * pageSize, total)} of {total}
              </p>
              <div className="flex gap-2">
                <button
                  onClick={() => setPage(p => Math.max(1, p - 1))}
                  disabled={page === 1}
                  className="btn-secondary py-1.5 text-xs disabled:opacity-50"
                >
                  Previous
                </button>
                <span className="flex items-center px-3 text-sm text-slate-600 dark:text-slate-400">
                  {page} / {totalPages}
                </span>
                <button
                  onClick={() => setPage(p => Math.min(totalPages, p + 1))}
                  disabled={page >= totalPages}
                  className="btn-secondary py-1.5 text-xs disabled:opacity-50"
                >
                  Next
                </button>
              </div>
            </div>
          </>
        )}
      </motion.div>
    </div>
  );
}
