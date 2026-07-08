'use client';

import { useState, useEffect } from 'react';
import { motion, AnimatePresence } from 'framer-motion';
import { 
  GraduationCap, 
  Shield, 
  Users, 
  UserCheck, 
  BookOpen, 
  CreditCard, 
  Calendar, 
  MessageSquare, 
  Bell, 
  ClipboardList, 
  ArrowRight,
  Sun,
  Moon,
  Database,
  Layers,
  Award
} from 'lucide-react';
import { useTheme } from 'next-themes';
import Link from 'next/link';

const STATS = [
  { label: 'Active Students', value: '2,500+', icon: Users, color: 'text-blue-500 bg-blue-500/10' },
  { label: 'Qualified Teachers', value: '150+', icon: UserCheck, color: 'text-emerald-500 bg-emerald-500/10' },
  { label: 'Core ERP Modules', value: '12+', icon: Layers, color: 'text-purple-500 bg-purple-500/10' },
  { label: 'Database Uptime', value: '99.9%', icon: Database, color: 'text-amber-500 bg-amber-500/10' },
];

const FEATURES = [
  {
    title: 'Admissions & Profiles',
    description: 'Complete student and teacher lifecycle management with detailed profiles, document attachments, and search.',
    icon: GraduationCap,
    color: 'from-blue-500 to-cyan-500',
  },
  {
    title: 'Attendance Tracker',
    description: 'Daily attendance marking, real-time absence alerts, and automated monthly analytics reports.',
    icon: ClipboardList,
    color: 'from-emerald-500 to-teal-500',
  },
  {
    title: 'Fee Management',
    description: 'Custom fee structures, online invoice generation, payment tracking, and digital PDF receipts.',
    icon: CreditCard,
    color: 'from-amber-500 to-orange-500',
  },
  {
    title: 'Class Schedules',
    description: 'Dynamic timetables for classes, sections, and subjects, ensuring optimal teacher-hour distribution.',
    icon: Calendar,
    color: 'from-purple-500 to-indigo-500',
  },
  {
    title: 'LMS & Assignments',
    description: 'Digital library management, book issues/returns, student homework assignments, and teacher grading.',
    icon: BookOpen,
    color: 'from-pink-500 to-rose-500',
  },
  {
    title: 'Communication Hub',
    description: 'Real-time internal messages and instant portal notifications keeping parents, teachers, and admins synced.',
    icon: MessageSquare,
    color: 'from-violet-500 to-fuchsia-500',
  },
];

const DEMO_ROLES = [
  {
    role: 'Super Admin',
    email: 'superadmin@schoolerp.com',
    password: 'Admin@123',
    desc: 'Full access to schools, configuration settings, and database management.',
    color: 'from-purple-500 to-indigo-600',
    icon: Shield,
  },
  {
    role: 'School Admin',
    email: 'admin@schoolerp.com',
    password: 'Admin@123',
    desc: 'Manage school branches, student admissions, and staff onboarding.',
    color: 'from-blue-500 to-cyan-600',
    icon: UserCheck,
  },
  {
    role: 'Principal',
    email: 'principal@schoolerp.com',
    password: 'Admin@123',
    desc: 'Monitor school operations, announcements, performance indices, and analytics.',
    color: 'from-rose-500 to-pink-600',
    icon: Award,
  },
  {
    role: 'Teacher',
    email: 'priya.sharma@schoolerp.com',
    password: 'Teacher@123',
    desc: 'Mark attendance, allocate assignments, update grades, and message parents.',
    color: 'from-emerald-500 to-teal-600',
    icon: ClipboardList,
  },
  {
    role: 'Student',
    email: 'arjun.mehta@student.schoolerp.com',
    password: 'Student@123',
    desc: 'View personal timetable, submit assignments, track attendance, and check fees.',
    color: 'from-amber-500 to-orange-600',
    icon: GraduationCap,
  },
  {
    role: 'Parent',
    email: 'suresh.mehta@parent.schoolerp.com',
    password: 'Parent@123',
    desc: 'Track student attendance, view reports, check pending fees, and message teachers.',
    color: 'from-violet-500 to-fuchsia-600',
    icon: Users,
  },
  {
    role: 'Accountant',
    email: 'accountant@schoolerp.com',
    password: 'Admin@123',
    desc: 'Collect fees, manage structure categories, and issue payment receipts.',
    color: 'from-lime-500 to-emerald-600',
    icon: CreditCard,
  },
  {
    role: 'Librarian',
    email: 'librarian@schoolerp.com',
    password: 'Admin@123',
    desc: 'Manage book inventory, track issues/returns, and penalize overdue returns.',
    color: 'from-sky-500 to-blue-600',
    icon: BookOpen,
  },
];

export default function HomePage() {
  const { theme, setTheme } = useTheme();
  const [mounted, setMounted] = useState(false);
  const backgroundImages = [
    '/school_arch.png',
    '/school_courtyard.jpg',
    '/school_building.jpg',
    '/school_aerial.jpg'
  ];

  const [bgIndex, setBgIndex] = useState(0);

  useEffect(() => {
    setMounted(true);
    const timer = setInterval(() => {
      setBgIndex((prev) => (prev + 1) % backgroundImages.length);
    }, 5000);
    return () => clearInterval(timer);
  }, []);

  if (!mounted) return null;

  return (
    <div className="min-h-screen bg-slate-50 dark:bg-slate-950 text-slate-900 dark:text-slate-100 transition-colors duration-300">
      
      {/* Background Orbs */}
      <div className="absolute top-0 left-1/4 w-[500px] h-[500px] rounded-full bg-blue-500/5 dark:bg-blue-500/10 blur-[100px] -z-10 pointer-events-none" />
      <div className="absolute top-1/3 right-1/4 w-[600px] h-[600px] rounded-full bg-purple-500/5 dark:bg-purple-500/10 blur-[120px] -z-10 pointer-events-none" />

      {/* Sticky Header */}
      <header className="sticky top-0 z-50 w-full backdrop-blur-md bg-white/70 dark:bg-slate-950/70 border-b border-slate-200/50 dark:border-slate-800/50 transition-colors">
        <div className="max-w-7xl mx-auto px-6 h-16 flex items-center justify-between">
          <Link href="/" className="flex items-center gap-3">
            <div className="w-10 h-10 rounded-xl overflow-hidden flex items-center justify-center bg-white shadow-md shadow-slate-200/50 dark:shadow-none border border-slate-100 dark:border-slate-800">
              <img src="/mcc_logo.png" alt="MCC Logo" className="w-8 h-8 object-contain" />
            </div>
            <div>
              <span className="font-bold text-lg leading-none tracking-tight block">MCC Campus</span>
              <span className="text-[10px] text-slate-500 dark:text-slate-400 font-semibold tracking-wider uppercase block">Matric. Hr. Sec. School</span>
            </div>
          </Link>

          <nav className="flex items-center gap-4">
            {/* Theme Toggle */}
            <button
              onClick={() => setTheme(theme === 'dark' ? 'light' : 'dark')}
              className="p-2.5 rounded-xl border border-slate-200 dark:border-slate-800 hover:bg-slate-100 dark:hover:bg-slate-900 transition-all text-slate-500 dark:text-slate-400"
              aria-label="Toggle Theme"
            >
              {theme === 'dark' ? <Sun className="w-4 h-4" /> : <Moon className="w-4 h-4" />}
            </button>

            <Link
              href="/login"
              className="btn-primary"
            >
              Enter Portal <ArrowRight className="w-4 h-4" />
            </Link>
          </nav>
        </div>
      </header>

      {/* Hero Section with Background Slideshow */}
      <section className="max-w-7xl mx-auto px-6 pt-20 pb-16 text-center relative overflow-hidden rounded-3xl border border-slate-200/50 dark:border-slate-800/50 my-6 shadow-sm bg-white/20 dark:bg-slate-900/10">
        
        {/* Background Slideshow */}
        <div className="absolute inset-0 -z-10 w-full h-full overflow-hidden">
          <AnimatePresence mode="popLayout">
            <motion.div
              key={bgIndex}
              initial={{ opacity: 0, scale: 1.05 }}
              animate={{ opacity: 0.18, scale: 1 }}
              exit={{ opacity: 0 }}
              transition={{ duration: 1.5, ease: 'easeInOut' }}
              className="absolute inset-0 w-full h-full bg-cover bg-center"
              style={{ backgroundImage: `url(${backgroundImages[bgIndex]})` }}
            />
          </AnimatePresence>
          {/* Blur & Overlay */}
          <div className="absolute inset-0 bg-gradient-to-b from-slate-50/70 via-slate-50/90 to-slate-50 dark:from-slate-950/70 dark:via-slate-950/90 dark:to-slate-950 backdrop-blur-[1px]" />
        </div>

        <motion.div
          initial={{ opacity: 0, y: 30 }}
          animate={{ opacity: 1, y: 0 }}
          transition={{ duration: 0.8 }}
          className="max-w-4xl mx-auto"
        >
          <span className="px-4 py-1.5 rounded-full text-xs font-semibold uppercase tracking-wider bg-blue-100 dark:bg-blue-900/30 text-blue-700 dark:text-blue-300 border border-blue-200/50 dark:border-blue-800/30 inline-block mb-6">
            ✨ Complete School Operating System
          </span>
          <h1 className="text-5xl md:text-6xl font-black tracking-tight leading-tight mb-8">
            Empowering Schools with a <br/>
            <span className="bg-gradient-to-r from-blue-400 via-blue-500 to-rose-400 bg-clip-text text-transparent">
              Unified Digital Ecosystem
            </span>
          </h1>
          <p className="text-lg md:text-xl text-slate-500 dark:text-slate-400 leading-relaxed max-w-2xl mx-auto mb-10">
            A comprehensive, high-fidelity platform to manage students, timetables, attendance, grades, invoicing, and messaging in real-time.
          </p>

          <div className="flex flex-wrap items-center justify-center gap-4">
            <a
              href="#quick-login"
              className="px-6 py-3.5 text-sm font-semibold bg-gradient-to-r from-blue-500 to-blue-600 hover:from-blue-600 hover:to-blue-700 text-white rounded-xl shadow-lg shadow-blue-500/20 transition-all flex items-center gap-2"
            >
              Quick Start Demo <ArrowRight className="w-4 h-4" />
            </a>
            <a
              href="#features"
              className="btn-secondary px-6 py-3.5"
            >
              Explore Modules
            </a>
          </div>
        </motion.div>
      </section>

      {/* Stats Counter Section */}
      <section className="max-w-7xl mx-auto px-6 py-8">
        <div className="grid grid-cols-2 lg:grid-cols-4 gap-4">
          {STATS.map((stat, i) => (
            <motion.div
              key={stat.label}
              initial={{ opacity: 0, y: 20 }}
              animate={{ opacity: 1, y: 0 }}
              transition={{ duration: 0.5, delay: i * 0.1 }}
              className="card p-6 flex items-center gap-4"
            >
              <div className={`p-3 rounded-2xl ${stat.color}`}>
                <stat.icon className="w-6 h-6" />
              </div>
              <div>
                <div className="text-2xl font-bold tracking-tight">{stat.value}</div>
                <div className="text-xs text-slate-500 dark:text-slate-400 font-medium">{stat.label}</div>
              </div>
            </motion.div>
          ))}
        </div>
      </section>

      {/* Features Grid Section */}
      <section id="features" className="max-w-7xl mx-auto px-6 py-20 scroll-mt-20">
        <div className="text-center max-w-2xl mx-auto mb-16">
          <h2 className="text-3xl md:text-4xl font-extrabold tracking-tight mb-4">Core Platform Modules</h2>
          <p className="text-slate-500 dark:text-slate-400">
            Engineered with modern architecture to cover all institutional needs under a single responsive dashboard.
          </p>
        </div>

        <div className="grid md:grid-cols-2 lg:grid-cols-3 gap-6">
          {FEATURES.map((feat, i) => (
            <motion.div
              key={feat.title}
              initial={{ opacity: 0, y: 30 }}
              whileInView={{ opacity: 1, y: 0 }}
              viewport={{ once: true }}
              transition={{ duration: 0.5, delay: i * 0.05 }}
              className="card p-6 hover:shadow-md transition-all group flex flex-col justify-between"
            >
              <div>
                <div className={`w-12 h-12 rounded-2xl bg-gradient-to-tr ${feat.color} flex items-center justify-center shadow-lg shadow-blue-500/10 mb-6 group-hover:scale-110 transition-transform duration-300`}>
                  <feat.icon className="w-6 h-6 text-white" />
                </div>
                <h3 className="text-lg font-bold mb-3">{feat.title}</h3>
                <p className="text-sm text-slate-500 dark:text-slate-400 leading-relaxed">
                  {feat.description}
                </p>
              </div>
            </motion.div>
          ))}
        </div>
      </section>

      {/* Quick Login Section */}
      <section id="quick-login" className="max-w-7xl mx-auto px-6 py-20 border-t border-slate-200/50 dark:border-slate-800/50 scroll-mt-20">
        <div className="text-center max-w-2xl mx-auto mb-16">
          <span className="text-xs font-bold text-blue-600 dark:text-blue-400 uppercase tracking-wider mb-2 block">Developer Sandboxes</span>
          <h2 className="text-3xl md:text-4xl font-extrabold tracking-tight mb-4">Quick Demo Login Hub</h2>
          <p className="text-slate-500 dark:text-slate-400">
            Click on any role block below to automatically open the portal login page, populated with demo credentials.
          </p>
        </div>

        <div className="grid sm:grid-cols-2 lg:grid-cols-4 gap-6">
          {DEMO_ROLES.map((role, i) => (
            <motion.div
              key={role.role}
              initial={{ opacity: 0, scale: 0.95 }}
              whileInView={{ opacity: 1, scale: 1 }}
              viewport={{ once: true }}
              transition={{ duration: 0.5, delay: i * 0.05 }}
              className="card p-6 flex flex-col justify-between border-slate-200/60 dark:border-slate-800/60 hover:border-slate-300 dark:hover:border-slate-700 hover:shadow-lg transition-all duration-300 relative overflow-hidden group"
            >
              {/* Subtle background color aura on card hover */}
              <div className="absolute top-0 right-0 w-24 h-24 bg-gradient-to-bl from-slate-100 dark:from-slate-800 to-transparent opacity-20 -z-10 group-hover:scale-150 transition-transform duration-500" />
              
              <div>
                <div className={`w-10 h-10 rounded-xl bg-gradient-to-tr ${role.color} flex items-center justify-center text-white mb-5`}>
                  <role.icon className="w-5 h-5" />
                </div>
                <h3 className="text-lg font-bold mb-2 group-hover:text-blue-600 dark:group-hover:text-blue-400 transition-colors">
                  {role.role}
                </h3>
                <p className="text-xs text-slate-400 dark:text-slate-500 leading-relaxed mb-6">
                  {role.desc}
                </p>
              </div>

              <div className="space-y-4">
                <div className="p-3.5 rounded-xl bg-slate-100/80 dark:bg-slate-900/80 border border-slate-200/50 dark:border-slate-800/50 font-mono text-[11px] space-y-1.5">
                  <div className="flex justify-between items-center">
                    <span className="text-slate-400">Email:</span>
                    <span className="font-semibold text-slate-700 dark:text-slate-300 select-all truncate max-w-[130px]" title={role.email}>
                      {role.email}
                    </span>
                  </div>
                  <div className="flex justify-between items-center">
                    <span className="text-slate-400">Password:</span>
                    <span className="font-semibold text-slate-700 dark:text-slate-300 select-all">
                      {role.password}
                    </span>
                  </div>
                </div>

                <Link
                  href={`/login?email=${encodeURIComponent(role.email)}&password=${encodeURIComponent(role.password)}`}
                  className="w-full py-2.5 px-4 text-xs font-semibold rounded-xl text-center block bg-slate-100 hover:bg-blue-600 hover:text-white dark:bg-slate-900 dark:hover:bg-blue-600 text-slate-700 dark:text-slate-300 transition-all border border-slate-200 dark:border-slate-800 hover:border-blue-600 dark:hover:border-blue-600 shadow-sm"
                >
                  Login as {role.role}
                </Link>
              </div>
            </motion.div>
          ))}
        </div>
      </section>

      {/* Footer */}
      <footer className="bg-white dark:bg-slate-950 border-t border-slate-200/50 dark:border-slate-800/50 py-12 transition-colors">
        <div className="max-w-7xl mx-auto px-6 flex flex-col md:flex-row items-center justify-between gap-6">
          <div className="flex items-center gap-3">
            <div className="w-8 h-8 rounded-lg overflow-hidden flex items-center justify-center bg-white border border-slate-100 dark:border-slate-800">
              <img src="/mcc_logo.png" alt="MCC Logo" className="w-6 h-6 object-contain" />
            </div>
            <span className="font-bold text-base">MCC Campus</span>
          </div>
          <p className="text-xs text-slate-400 dark:text-slate-500">
            © {new Date().getFullYear()} MCC Campus Ecosystem. All rights reserved. Registered Sandbox Environment.
          </p>
        </div>
      </footer>

    </div>
  );
}
