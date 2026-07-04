'use client';

import { useState, useEffect } from 'react';
import { motion, AnimatePresence } from 'framer-motion';
import { Bell, Moon, Sun, Search, LogOut, Menu, X, User, Settings } from 'lucide-react';
import { useTheme } from 'next-themes';
import { useAuth } from '@/contexts/AuthContext';
import { cn, getInitials } from '@/lib/utils';
import api from '@/lib/api';
import { Notification } from '@/types';
import { formatDistanceToNow } from 'date-fns';

interface TopbarProps {
  onMenuToggle: () => void;
  sidebarCollapsed: boolean;
}

export default function Topbar({ onMenuToggle, sidebarCollapsed }: TopbarProps) {
  const { theme, setTheme } = useTheme();
  const { user, logout } = useAuth();
  const [showNotifications, setShowNotifications] = useState(false);
  const [showUserMenu, setShowUserMenu] = useState(false);
  const [notifications, setNotifications] = useState<Notification[]>([]);
  const [mounted, setMounted] = useState(false);

  useEffect(() => { setMounted(true); }, []);

  useEffect(() => {
    if (showNotifications) {
      api.get('/dashboard/notifications').then(res => {
        setNotifications(res.data.data || []);
      }).catch(() => {});
    }
  }, [showNotifications]);

  const unread = notifications.filter(n => !n.isRead).length;

  return (
    <header className={cn(
      "fixed top-0 right-0 z-20 h-16 bg-white dark:bg-slate-900 border-b border-slate-200 dark:border-slate-800 flex items-center justify-between px-6 transition-all duration-300",
      sidebarCollapsed ? "left-[72px]" : "left-[256px]"
    )}>
      {/* Left: Title / Menu */}
      <div className="flex items-center gap-4">
        <button id="mobile-menu-toggle" onClick={onMenuToggle}
          className="w-9 h-9 rounded-xl hover:bg-slate-100 dark:hover:bg-slate-800 flex items-center justify-center text-slate-500 transition-colors lg:hidden">
          <Menu className="w-5 h-5" />
        </button>
        <h1 className="text-sm font-semibold text-slate-900 dark:text-white hidden sm:block">
          School ERP System
        </h1>
      </div>

      {/* Right: Actions */}
      <div className="flex items-center gap-2">
        {/* Dark Mode Toggle */}
        {mounted && (
          <motion.button
            whileTap={{ scale: 0.9 }}
            id="theme-toggle"
            onClick={() => setTheme(theme === 'dark' ? 'light' : 'dark')}
            className="w-9 h-9 rounded-xl hover:bg-slate-100 dark:hover:bg-slate-800 flex items-center justify-center text-slate-500 dark:text-slate-400 transition-colors"
          >
            {theme === 'dark' ? <Sun className="w-5 h-5" /> : <Moon className="w-5 h-5" />}
          </motion.button>
        )}

        {/* Notifications */}
        <div className="relative">
          <motion.button
            whileTap={{ scale: 0.9 }}
            id="notifications-toggle"
            onClick={() => { setShowNotifications(v => !v); setShowUserMenu(false); }}
            className="w-9 h-9 rounded-xl hover:bg-slate-100 dark:hover:bg-slate-800 flex items-center justify-center text-slate-500 dark:text-slate-400 transition-colors relative"
          >
            <Bell className="w-5 h-5" />
            {unread > 0 && (
              <span className="absolute top-1.5 right-1.5 w-2 h-2 bg-red-500 rounded-full" />
            )}
          </motion.button>

          <AnimatePresence>
            {showNotifications && (
              <motion.div
                initial={{ opacity: 0, y: 8, scale: 0.96 }}
                animate={{ opacity: 1, y: 0, scale: 1 }}
                exit={{ opacity: 0, y: 8, scale: 0.96 }}
                transition={{ duration: 0.2 }}
                className="absolute right-0 mt-2 w-80 card shadow-xl overflow-hidden z-50"
              >
                <div className="px-4 py-3 border-b border-slate-100 dark:border-slate-800 flex items-center justify-between">
                  <h3 className="font-semibold text-slate-900 dark:text-white">Notifications</h3>
                  <button onClick={() => setShowNotifications(false)}>
                    <X className="w-4 h-4 text-slate-400" />
                  </button>
                </div>
                <div className="max-h-80 overflow-y-auto divide-y divide-slate-100 dark:divide-slate-800">
                  {notifications.length === 0 ? (
                    <div className="px-4 py-8 text-center text-slate-400 text-sm">No notifications</div>
                  ) : notifications.map((n) => (
                    <div key={n.id} className="px-4 py-3 hover:bg-slate-50 dark:hover:bg-slate-800/50 cursor-pointer">
                      <div className="text-sm font-medium text-slate-900 dark:text-white">{n.title}</div>
                      <div className="text-xs text-slate-500 mt-0.5 line-clamp-2">{n.message}</div>
                      <div className="text-xs text-slate-400 mt-1">
                        {formatDistanceToNow(new Date(n.createdAt), { addSuffix: true })}
                      </div>
                    </div>
                  ))}
                </div>
              </motion.div>
            )}
          </AnimatePresence>
        </div>

        {/* User Menu */}
        <div className="relative">
          <motion.button
            whileTap={{ scale: 0.95 }}
            id="user-menu-toggle"
            onClick={() => { setShowUserMenu(v => !v); setShowNotifications(false); }}
            className="flex items-center gap-2.5 pl-2 pr-3 py-1.5 rounded-xl hover:bg-slate-100 dark:hover:bg-slate-800 transition-colors"
          >
            <div className="w-7 h-7 rounded-full bg-blue-100 dark:bg-blue-950 flex items-center justify-center">
              <span className="text-blue-600 dark:text-blue-400 text-xs font-bold">
                {user ? getInitials(`${user.firstName} ${user.lastName}`) : '?'}
              </span>
            </div>
            <span className="text-sm font-medium text-slate-700 dark:text-slate-300 hidden sm:block">
              {user?.firstName}
            </span>
          </motion.button>

          <AnimatePresence>
            {showUserMenu && (
              <motion.div
                initial={{ opacity: 0, y: 8, scale: 0.96 }}
                animate={{ opacity: 1, y: 0, scale: 1 }}
                exit={{ opacity: 0, y: 8, scale: 0.96 }}
                transition={{ duration: 0.2 }}
                className="absolute right-0 mt-2 w-56 card shadow-xl z-50 overflow-hidden"
              >
                <div className="px-4 py-3 border-b border-slate-100 dark:border-slate-800">
                  <div className="text-sm font-semibold text-slate-900 dark:text-white">
                    {user?.firstName} {user?.lastName}
                  </div>
                  <div className="text-xs text-slate-500">{user?.email}</div>
                  <span className="badge badge-blue mt-1.5">{user?.role}</span>
                </div>
                <div className="py-1">
                  <button className="w-full flex items-center gap-3 px-4 py-2.5 text-sm text-slate-600 dark:text-slate-400 hover:bg-slate-50 dark:hover:bg-slate-800 transition-colors">
                    <User className="w-4 h-4" /> My Profile
                  </button>
                  <button className="w-full flex items-center gap-3 px-4 py-2.5 text-sm text-slate-600 dark:text-slate-400 hover:bg-slate-50 dark:hover:bg-slate-800 transition-colors">
                    <Settings className="w-4 h-4" /> Settings
                  </button>
                  <div className="border-t border-slate-100 dark:border-slate-800 mt-1 pt-1">
                    <button
                      id="logout-btn"
                      onClick={logout}
                      className="w-full flex items-center gap-3 px-4 py-2.5 text-sm text-red-600 hover:bg-red-50 dark:hover:bg-red-950/30 transition-colors"
                    >
                      <LogOut className="w-4 h-4" /> Sign Out
                    </button>
                  </div>
                </div>
              </motion.div>
            )}
          </AnimatePresence>
        </div>
      </div>
    </header>
  );
}
