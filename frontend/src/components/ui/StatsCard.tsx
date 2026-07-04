'use client';

import { motion } from 'framer-motion';
import { cn } from '@/lib/utils';

interface StatsCardProps {
  title: string;
  value: string | number;
  subtitle?: string;
  icon: React.ElementType;
  gradient: string;
  trend?: { value: number; positive: boolean };
  index?: number;
}

export function StatsCard({ title, value, subtitle, icon: Icon, gradient, trend, index = 0 }: StatsCardProps) {
  return (
    <motion.div
      initial={{ opacity: 0, y: 20 }}
      animate={{ opacity: 1, y: 0 }}
      transition={{ delay: index * 0.08, duration: 0.4 }}
      whileHover={{ y: -2, transition: { duration: 0.2 } }}
      className="card p-5 overflow-hidden relative"
    >
      {/* Decorative background */}
      <div className={cn("absolute top-0 right-0 w-28 h-28 rounded-full opacity-10 -translate-y-6 translate-x-6", gradient.replace('gradient-', ''))} />

      <div className="relative z-10">
        <div className="flex items-start justify-between mb-4">
          <div>
            <p className="text-sm text-slate-500 dark:text-slate-400 font-medium">{title}</p>
            <p className="text-3xl font-bold text-slate-900 dark:text-white mt-1">{value}</p>
          </div>
          <div className={cn("w-12 h-12 rounded-2xl flex items-center justify-center flex-shrink-0", gradient)}>
            <Icon className="w-6 h-6 text-white" />
          </div>
        </div>
        {(subtitle || trend) && (
          <div className="flex items-center gap-2">
            {trend && (
              <span className={cn("text-xs font-semibold px-1.5 py-0.5 rounded-full",
                trend.positive
                  ? "bg-emerald-100 text-emerald-700 dark:bg-emerald-950/50 dark:text-emerald-400"
                  : "bg-red-100 text-red-700 dark:bg-red-950/50 dark:text-red-400"
              )}>
                {trend.positive ? '+' : ''}{trend.value}%
              </span>
            )}
            {subtitle && <span className="text-xs text-slate-400">{subtitle}</span>}
          </div>
        )}
      </div>
    </motion.div>
  );
}
