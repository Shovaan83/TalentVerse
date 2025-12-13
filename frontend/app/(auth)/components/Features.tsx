'use client';

import React from 'react';
import { motion } from 'framer-motion';
import { Wallet, ShieldCheck, Star, Calendar, MessageSquare, CheckCircle2 } from 'lucide-react';

export const Features: React.FC = () => {
  return (
    <section className="py-24 bg-white" id="features">
      <div className="container mx-auto px-6">
        <div className="text-center max-w-2xl mx-auto mb-16">
          <h2 className="font-heading text-3xl md:text-4xl font-bold text-gray-900 mb-4">
            Everything you need to <span className="text-emerald-600">swap confidently.</span>
          </h2>
          <p className="text-gray-600 text-lg">
            We've built a robust ecosystem to ensure every trade is fair, transparent, and rewarding.
          </p>
        </div>

        {/* Bento Grid */}
        <div className="grid grid-cols-1 md:grid-cols-3 md:grid-rows-2 gap-6 h-auto md:h-[600px]">
          
          {/* Box 1: Virtual Economy */}
          <motion.div 
            whileHover={{ scale: 1.02 }}
            className="md:col-span-2 bg-gray-50 rounded-3xl p-8 flex flex-col justify-between relative overflow-hidden group"
          >
            <div className="z-10 relative">
              <div className="bg-white w-12 h-12 rounded-xl flex items-center justify-center shadow-sm mb-6 text-emerald-600">
                <Wallet size={24} />
              </div>
              <h3 className="font-heading text-2xl font-bold text-gray-900 mb-2">The Virtual Economy</h3>
              <p className="text-gray-600 max-w-sm">
                Earn 'Swap Credits' for every hour you teach. Use them to learn anything from anyone. No currency conversion fees, just pure value.
              </p>
            </div>
            
            {/* Abstract Visual */}
            <div className="absolute right-0 bottom-0 w-64 h-48 opacity-50 group-hover:opacity-100 transition-opacity duration-500">
                <div className="bg-white rounded-tl-2xl shadow-xl p-4 absolute right-0 bottom-0 w-full h-full border-t border-l border-gray-100">
                    <div className="flex items-center justify-between mb-4 border-b pb-2">
                        <span className="text-xs font-bold text-gray-400">YOUR WALLET</span>
                        <span className="text-xs font-bold text-emerald-600">+250 Credits</span>
                    </div>
                    <div className="space-y-3">
                        {[1, 2].map(i => (
                            <div key={i} className="flex items-center gap-3">
                                <div className="w-8 h-8 rounded-full bg-emerald-100 flex items-center justify-center text-emerald-600 text-xs font-bold">IN</div>
                                <div>
                                    <div className="h-2 w-24 bg-gray-200 rounded mb-1"></div>
                                    <div className="h-2 w-16 bg-gray-100 rounded"></div>
                                </div>
                            </div>
                        ))}
                    </div>
                </div>
            </div>
          </motion.div>

          {/* Box 2: Real-Time Chat */}
          <motion.div 
             whileHover={{ scale: 1.02 }}
            className="md:col-span-1 md:row-span-2 bg-emerald-950 rounded-3xl p-8 flex flex-col relative overflow-hidden"
          >
            <div className="bg-white/10 w-12 h-12 rounded-xl flex items-center justify-center backdrop-blur-md mb-6 text-white">
                <MessageSquare size={24} />
            </div>
            <h3 className="font-heading text-2xl font-bold text-white mb-2">Real-Time Sync</h3>
            <p className="text-emerald-200/80 mb-8 text-sm">
              Chat, negotiate terms, and schedule sessions directly within the platform.
            </p>

            {/* Chat Visual */}
            <div className="flex-1 bg-white/5 rounded-t-2xl border-t border-x border-white/10 p-4 space-y-4 backdrop-blur-sm">
                <div className="flex gap-2">
                    <div className="w-6 h-6 rounded-full bg-orange-500 flex-shrink-0"></div>
                    <div className="bg-white/10 rounded-2xl rounded-tl-none p-3 text-xs text-emerald-50">
                        Hi! I'd love to swap my SEO skills for your piano lessons.
                    </div>
                </div>
                <div className="flex gap-2 flex-row-reverse">
                    <div className="w-6 h-6 rounded-full bg-emerald-500 flex-shrink-0"></div>
                    <div className="bg-emerald-600 text-white rounded-2xl rounded-tr-none p-3 text-xs shadow-lg">
                        Sounds great! Are you free this Tuesday?
                    </div>
                </div>
                <div className="mt-4">
                    <button className="w-full bg-orange-600 hover:bg-orange-500 text-white text-xs font-bold py-3 rounded-xl flex items-center justify-center gap-2 transition-colors">
                        <Calendar size={14} />
                        Schedule Meeting
                    </button>
                </div>
            </div>
          </motion.div>

          {/* Box 3: Verified Talent */}
          <motion.div 
             whileHover={{ scale: 1.02 }}
            className="md:col-span-2 bg-white border border-gray-100 shadow-xl shadow-gray-100/50 rounded-3xl p-8 flex flex-col sm:flex-row items-center justify-between gap-8 relative overflow-hidden"
          >
             <div className="z-10 flex-1">
                <div className="bg-orange-100 w-12 h-12 rounded-xl flex items-center justify-center mb-6 text-orange-600">
                    <ShieldCheck size={24} />
                </div>
                <h3 className="font-heading text-2xl font-bold text-gray-900 mb-2">Verified Talent</h3>
                <p className="text-gray-600">
                    Badge-verified experts. Peer-reviewed skills. We ensure you're learning from the best.
                </p>
             </div>
             
             <div className="z-10 bg-white p-4 rounded-2xl shadow-lg border border-gray-50 flex items-center gap-4 min-w-[240px]">
                <div className="text-center border-r border-gray-100 pr-4">
                    <p className="text-3xl font-bold text-gray-900">4.9</p>
                    <div className="flex text-amber-400 text-xs">
                        {[1,2,3,4,5].map(i => <Star key={i} size={12} fill="currentColor" />)}
                    </div>
                </div>
                <div className="space-y-1">
                    <div className="flex items-center gap-2 text-sm font-medium text-gray-700">
                        <CheckCircle2 size={16} className="text-emerald-500" />
                        ID Verified
                    </div>
                    <div className="flex items-center gap-2 text-sm font-medium text-gray-700">
                        <CheckCircle2 size={16} className="text-emerald-500" />
                        Skill Tested
                    </div>
                </div>
             </div>

             {/* Background Decoration */}
             <div className="absolute top-0 right-0 w-full h-full bg-gradient-to-bl from-orange-50/50 to-transparent pointer-events-none" />
          </motion.div>

        </div>
      </div>
    </section>
  );
};
