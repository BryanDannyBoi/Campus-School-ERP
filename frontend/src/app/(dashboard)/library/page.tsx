'use client';
import { motion } from 'framer-motion';
import { Library, BookOpen, Search } from 'lucide-react';
import { EmptyState } from '@/components/ui/Common';

export default function LibraryPage() {
  return (
    <div>
      <motion.div initial={{ opacity: 0, y: -10 }} animate={{ opacity: 1, y: 0 }} className="mb-6">
        <h1 className="text-2xl font-bold text-slate-900 dark:text-white">Library</h1>
        <p className="text-sm text-slate-500 mt-0.5">Manage books, issues, and returns</p>
      </motion.div>
      <div className="card">
        <EmptyState icon={Library} title="Library Module" description="Coming soon — Phase 2 will include book catalog, issue management, and fine calculation." />
      </div>
    </div>
  );
}
