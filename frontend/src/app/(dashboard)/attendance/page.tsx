'use client';

import { useEffect, useState } from 'react';
import { motion } from 'framer-motion';
import { CheckCircle2, XCircle, Clock, Save, Calendar } from 'lucide-react';
import api from '@/lib/api';
import { LoadingSpinner } from '@/components/ui/Common';
import { formatDate } from '@/lib/utils';
import toast from 'react-hot-toast';

const STATUS_OPTIONS = [
  { value: 1, label: 'Present', icon: CheckCircle2, color: 'text-emerald-600 bg-emerald-50 dark:bg-emerald-950/30 border-emerald-200 dark:border-emerald-800' },
  { value: 2, label: 'Absent', icon: XCircle, color: 'text-red-600 bg-red-50 dark:bg-red-950/30 border-red-200 dark:border-red-800' },
  { value: 3, label: 'Late', icon: Clock, color: 'text-amber-600 bg-amber-50 dark:bg-amber-950/30 border-amber-200 dark:border-amber-800' },
];

export default function AttendancePage() {
  const [classes, setClasses] = useState<any[]>([]);
  const [selectedClassId, setSelectedClassId] = useState('');
  const [selectedSectionId, setSelectedSectionId] = useState('');
  const [selectedDate, setSelectedDate] = useState(new Date().toISOString().split('T')[0]);
  const [students, setStudents] = useState<any[]>([]);
  const [attendanceMap, setAttendanceMap] = useState<Record<string, number>>({});
  const [loading, setLoading] = useState(false);
  const [saving, setSaving] = useState(false);
  const [loadingClasses, setLoadingClasses] = useState(true);

  useEffect(() => {
    api.get('/classes').then(res => {
      setClasses(res.data.data || []);
    }).finally(() => setLoadingClasses(false));
  }, []);

  useEffect(() => {
    if (!selectedClassId) return;
    setLoading(true);
    const params: any = { date: selectedDate };
    if (selectedSectionId) params.sectionId = selectedSectionId;
    api.get(`/attendance/class/${selectedClassId}`, { params }).then(res => {
      const records = res.data.data || [];
      const map: Record<string, number> = {};
      records.forEach((r: any) => { if (r.studentId) map[r.studentId] = r.status; });
      setAttendanceMap(map);
    });
    api.get('/students', { params: { classId: selectedClassId, sectionId: selectedSectionId || undefined, pageSize: 100 } })
      .then(res => setStudents(res.data.data?.items || []))
      .finally(() => setLoading(false));
  }, [selectedClassId, selectedSectionId, selectedDate]);

  const selectedClass = classes.find(c => c.id === selectedClassId);

  const markAll = (status: number) => {
    const map: Record<string, number> = {};
    students.forEach(s => { map[s.id] = status; });
    setAttendanceMap(map);
  };

  const handleSave = async () => {
    if (!selectedClassId) { toast.error('Please select a class'); return; }
    setSaving(true);
    try {
      await api.post('/attendance/mark', {
        classId: selectedClassId,
        sectionId: selectedSectionId || null,
        date: selectedDate,
        attendances: students.map(s => ({ studentId: s.id, status: attendanceMap[s.id] || 1 }))
      });
      toast.success('Attendance saved successfully!');
    } catch { toast.error('Failed to save attendance'); }
    finally { setSaving(false); }
  };

  return (
    <div>
      <motion.div initial={{ opacity: 0, y: -10 }} animate={{ opacity: 1, y: 0 }} className="mb-6">
        <h1 className="text-2xl font-bold text-slate-900 dark:text-white">Attendance</h1>
        <p className="text-sm text-slate-500 mt-0.5">Mark and manage student attendance</p>
      </motion.div>

      {/* Controls */}
      <motion.div initial={{ opacity: 0 }} animate={{ opacity: 1 }} transition={{ delay: 0.1 }} className="card p-5 mb-4">
        <div className="grid grid-cols-1 sm:grid-cols-3 gap-4">
          <div>
            <label className="block text-sm font-medium text-slate-700 dark:text-slate-300 mb-1.5">Date</label>
            <input id="attendance-date" type="date" value={selectedDate}
              onChange={e => setSelectedDate(e.target.value)}
              className="form-input w-full" max={new Date().toISOString().split('T')[0]} />
          </div>
          <div>
            <label className="block text-sm font-medium text-slate-700 dark:text-slate-300 mb-1.5">Class</label>
            <select id="attendance-class" value={selectedClassId}
              onChange={e => { setSelectedClassId(e.target.value); setSelectedSectionId(''); }}
              className="form-input w-full">
              <option value="">— Select Class —</option>
              {classes.map(c => <option key={c.id} value={c.id}>{c.name}</option>)}
            </select>
          </div>
          <div>
            <label className="block text-sm font-medium text-slate-700 dark:text-slate-300 mb-1.5">Section</label>
            <select id="attendance-section" value={selectedSectionId}
              onChange={e => setSelectedSectionId(e.target.value)}
              className="form-input w-full" disabled={!selectedClassId}>
              <option value="">All Sections</option>
              {selectedClass?.sections?.map((s: any) => (
                <option key={s.id} value={s.id}>Section {s.name}</option>
              ))}
            </select>
          </div>
        </div>
      </motion.div>

      {selectedClassId && (
        <motion.div initial={{ opacity: 0, y: 10 }} animate={{ opacity: 1, y: 0 }} className="card overflow-hidden">
          {/* Toolbar */}
          <div className="flex items-center justify-between p-4 border-b border-slate-100 dark:border-slate-800">
            <div className="flex items-center gap-2">
              <Calendar className="w-4 h-4 text-slate-400" />
              <span className="text-sm font-medium text-slate-700 dark:text-slate-300">
                {formatDate(selectedDate)} — {students.length} students
              </span>
            </div>
            <div className="flex items-center gap-2">
              <button onClick={() => markAll(1)} className="text-xs px-3 py-1.5 rounded-lg bg-emerald-100 text-emerald-700 hover:bg-emerald-200 transition-colors font-medium">
                Mark All Present
              </button>
              <button onClick={handleSave} disabled={saving} id="save-attendance-btn" className="btn-primary py-2">
                <Save className="w-4 h-4" />
                {saving ? 'Saving...' : 'Save Attendance'}
              </button>
            </div>
          </div>

          {loading ? <LoadingSpinner /> : (
            <div className="divide-y divide-slate-100 dark:divide-slate-800">
              {students.map((student, i) => (
                <motion.div key={student.id}
                  initial={{ opacity: 0, x: -10 }}
                  animate={{ opacity: 1, x: 0 }}
                  transition={{ delay: i * 0.02 }}
                  className="flex items-center justify-between px-4 py-3 hover:bg-slate-50/50 dark:hover:bg-slate-800/20"
                >
                  <div className="flex items-center gap-3">
                    <div className="w-8 h-8 rounded-full bg-blue-100 dark:bg-blue-950 flex items-center justify-center text-xs font-bold text-blue-600 dark:text-blue-400">
                      {student.fullName.split(' ').map((n: string) => n[0]).join('').slice(0, 2)}
                    </div>
                    <div>
                      <div className="text-sm font-medium text-slate-900 dark:text-white">{student.fullName}</div>
                      <div className="text-xs text-slate-400">{student.admissionNumber}</div>
                    </div>
                  </div>
                  <div className="flex gap-2">
                    {STATUS_OPTIONS.map(({ value, label, icon: Icon, color }) => (
                      <button
                        key={value}
                        id={`attendance-${student.id}-${label.toLowerCase()}`}
                        onClick={() => setAttendanceMap(prev => ({ ...prev, [student.id]: value }))}
                        className={`flex items-center gap-1.5 px-3 py-1.5 rounded-lg border text-xs font-medium transition-all ${
                          attendanceMap[student.id] === value
                            ? color
                            : 'border-slate-200 dark:border-slate-700 text-slate-400 hover:border-slate-300'
                        }`}
                      >
                        <Icon className="w-3.5 h-3.5" />
                        {label}
                      </button>
                    ))}
                  </div>
                </motion.div>
              ))}
              {students.length === 0 && (
                <div className="py-12 text-center text-slate-400 text-sm">No students found in this class</div>
              )}
            </div>
          )}
        </motion.div>
      )}

      {!selectedClassId && (
        <motion.div initial={{ opacity: 0 }} animate={{ opacity: 1 }} transition={{ delay: 0.2 }}
          className="card p-16 text-center">
          <Calendar className="w-12 h-12 text-slate-300 mx-auto mb-3" />
          <h3 className="text-lg font-semibold text-slate-900 dark:text-white mb-1">Select a Class</h3>
          <p className="text-sm text-slate-400">Choose a class above to mark attendance</p>
        </motion.div>
      )}
    </div>
  );
}
