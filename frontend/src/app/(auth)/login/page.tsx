'use client';

import { useState, useEffect } from 'react';
import { motion } from 'framer-motion';
import { Eye, EyeOff, GraduationCap, Lock, Mail, ArrowRight, Loader2 } from 'lucide-react';
import { useAuth } from '@/contexts/AuthContext';
import Link from 'next/link';

const DEMO_CREDENTIALS = [
  { role: 'Super Admin', email: 'superadmin@schoolerp.com', password: 'Admin@123', color: 'from-purple-500 to-purple-700' },
  { role: 'School Admin', email: 'admin@schoolerp.com', password: 'Admin@123', color: 'from-blue-500 to-blue-700' },
  { role: 'Teacher', email: 'priya.sharma@schoolerp.com', password: 'Teacher@123', color: 'from-emerald-500 to-emerald-700' },
  { role: 'Student', email: 'arjun.mehta@student.schoolerp.com', password: 'Student@123', color: 'from-amber-500 to-amber-700' },
];

export default function LoginPage() {
  const { login, isLoading } = useAuth();
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [showPassword, setShowPassword] = useState(false);
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    if (typeof window !== 'undefined') {
      const params = new URLSearchParams(window.location.search);
      const emailParam = params.get('email');
      const passwordParam = params.get('password');
      if (emailParam) setEmail(emailParam);
      if (passwordParam) setPassword(passwordParam);
    }
  }, []);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    setLoading(true);
    try {
      await login(email, password);
    } finally {
      setLoading(false);
    }
  };

  const fillCredentials = (e: string, p: string) => {
    setEmail(e);
    setPassword(p);
  };

  return (
    <div className="min-h-screen flex">
      {/* Left Panel — Branding */}
      <motion.div
        initial={{ x: -60, opacity: 0 }}
        animate={{ x: 0, opacity: 1 }}
        transition={{ duration: 0.6, ease: 'easeOut' }}
        className="hidden lg:flex lg:w-1/2 relative overflow-hidden flex-col justify-between p-12"
        style={{ 
          backgroundImage: 'linear-gradient(135deg, rgba(15, 23, 42, 0.88) 0%, rgba(30, 58, 95, 0.92) 100%), url("/school_aerial.jpg")',
          backgroundSize: 'cover',
          backgroundPosition: 'center'
        }}
      >
        {/* Decorative circles */}
        <div className="absolute top-0 right-0 w-96 h-96 rounded-full opacity-10"
          style={{ background: 'radial-gradient(circle, #3b82f6 0%, transparent 70%)', transform: 'translate(30%, -30%)' }} />
        <div className="absolute bottom-0 left-0 w-80 h-80 rounded-full opacity-10"
          style={{ background: 'radial-gradient(circle, #8b5cf6 0%, transparent 70%)', transform: 'translate(-30%, 30%)' }} />

        {/* Logo */}
        <div className="relative z-10">
          <div className="flex items-center gap-3 mb-12">
            <div className="w-12 h-12 rounded-2xl overflow-hidden flex items-center justify-center shadow-lg shadow-blue-500/30 bg-white p-1">
              <img src="/mcc_logo.png" alt="MCC Logo" className="w-full h-full object-contain" />
            </div>
            <div>
              <h1 className="text-white font-bold text-xl leading-none">MCC Campus</h1>
              <p className="text-blue-300 text-sm">Matric. Hr. Sec. School</p>
            </div>
          </div>

          <motion.div
            initial={{ y: 20, opacity: 0 }}
            animate={{ y: 0, opacity: 1 }}
            transition={{ delay: 0.3, duration: 0.6 }}
          >
            <h2 className="text-4xl font-bold text-white mb-4 leading-tight">
              Empowering<br />
              <span className="text-blue-400">Education</span><br />
              Excellence
            </h2>
            <p className="text-slate-400 text-lg leading-relaxed max-w-md">
              A comprehensive platform to manage every aspect of your school — from student admissions to fee collection, all in one place.
            </p>
          </motion.div>
        </div>

        {/* Stats */}
        <motion.div
          initial={{ y: 20, opacity: 0 }}
          animate={{ y: 0, opacity: 1 }}
          transition={{ delay: 0.5, duration: 0.6 }}
          className="relative z-10 grid grid-cols-3 gap-4"
        >
          {[
            { label: 'Students', value: '2,500+' },
            { label: 'Teachers', value: '150+' },
            { label: 'Modules', value: '12+' },
          ].map((stat) => (
            <div key={stat.label} className="bg-white/5 backdrop-blur-sm rounded-2xl p-4 border border-white/10">
              <div className="text-2xl font-bold text-white">{stat.value}</div>
              <div className="text-slate-400 text-sm">{stat.label}</div>
            </div>
          ))}
        </motion.div>
      </motion.div>

      {/* Right Panel — Login Form */}
      <motion.div
        initial={{ x: 60, opacity: 0 }}
        animate={{ x: 0, opacity: 1 }}
        transition={{ duration: 0.6, ease: 'easeOut' }}
        className="flex-1 flex items-center justify-center p-8 bg-white dark:bg-slate-900"
      >
        <div className="w-full max-w-md">
          {/* Mobile logo */}
          <div className="flex items-center gap-3 mb-8 lg:hidden">
            <div className="w-10 h-10 rounded-xl overflow-hidden flex items-center justify-center bg-white border border-slate-200 shadow-sm p-0.5">
              <img src="/mcc_logo.png" alt="MCC Logo" className="w-full h-full object-contain" />
            </div>
            <span className="font-bold text-xl text-slate-900 dark:text-white">MCC Campus</span>
          </div>

          <div className="mb-8">
            <h2 className="text-3xl font-bold text-slate-900 dark:text-white mb-2">Welcome back</h2>
            <p className="text-slate-500 dark:text-slate-400">Sign in to your account to continue</p>
          </div>

          <form onSubmit={handleSubmit} className="space-y-5">
            {/* Email */}
            <div>
              <label className="block text-sm font-medium text-slate-700 dark:text-slate-300 mb-1.5">Email address</label>
              <div className="relative">
                <Mail className="absolute left-3.5 top-1/2 -translate-y-1/2 w-4 h-4 text-slate-400" />
                <input
                  id="email"
                  type="email"
                  value={email}
                  onChange={e => setEmail(e.target.value)}
                  required
                  placeholder="admin@schoolerp.com"
                  className="form-input pl-10"
                />
              </div>
            </div>

            {/* Password */}
            <div>
              <label className="block text-sm font-medium text-slate-700 dark:text-slate-300 mb-1.5">Password</label>
              <div className="relative">
                <Lock className="absolute left-3.5 top-1/2 -translate-y-1/2 w-4 h-4 text-slate-400" />
                <input
                  id="password"
                  type={showPassword ? 'text' : 'password'}
                  value={password}
                  onChange={e => setPassword(e.target.value)}
                  required
                  placeholder="Enter your password"
                  className="form-input pl-10 pr-10"
                />
                <button type="button" onClick={() => setShowPassword(v => !v)}
                  className="absolute right-3.5 top-1/2 -translate-y-1/2 text-slate-400 hover:text-slate-600">
                  {showPassword ? <EyeOff className="w-4 h-4" /> : <Eye className="w-4 h-4" />}
                </button>
              </div>
            </div>

            {/* Forgot password */}
            <div className="flex justify-end">
              <Link href="/forgot-password" className="text-sm text-blue-600 hover:text-blue-700 font-medium">
                Forgot password?
              </Link>
            </div>

            {/* Submit */}
            <button type="submit" disabled={loading} id="login-btn"
              className="btn-primary w-full justify-center py-3 text-base font-semibold">
              {loading ? <Loader2 className="w-5 h-5 animate-spin" /> : <ArrowRight className="w-5 h-5" />}
              {loading ? 'Signing in...' : 'Sign In'}
            </button>
          </form>

          {/* Demo credentials */}
          <div className="mt-8">
            <p className="text-xs text-slate-400 text-center mb-3 font-medium uppercase tracking-wider">Quick Demo Login</p>
            <div className="grid grid-cols-2 gap-2">
              {DEMO_CREDENTIALS.map((cred) => (
                <motion.button
                  key={cred.role}
                  whileHover={{ scale: 1.02 }}
                  whileTap={{ scale: 0.98 }}
                  onClick={() => fillCredentials(cred.email, cred.password)}
                  className={`text-left p-3 rounded-xl bg-gradient-to-r ${cred.color} text-white`}
                >
                  <div className="text-xs font-bold">{cred.role}</div>
                  <div className="text-xs opacity-70 truncate">{cred.email}</div>
                </motion.button>
              ))}
            </div>
            <p className="text-xs text-slate-400 text-center mt-2">Click a role to auto-fill credentials</p>
          </div>
        </div>
      </motion.div>
    </div>
  );
}
