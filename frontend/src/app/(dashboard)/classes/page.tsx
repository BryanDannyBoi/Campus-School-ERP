'use client';
import { motion } from 'framer-motion';
export default function Page() {
  return (
    <div>
      <motion.div initial={{ opacity: 0 }} animate={{ opacity: 1 }} className="mb-6">
        <h1 className="text-2xl font-bold text-slate-900 dark:text-white capitalize">classes</h1>
        <p className="text-sm text-slate-500 mt-0.5">This module is coming in Phase 2.</p>
      </motion.div>
      <div className="card p-16 text-center text-slate-400">Phase 2 module - coming soon!</div>
    </div>
  );
}