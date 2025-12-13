'use client';

import React from 'react';
import { motion } from 'framer-motion';
import { ArrowRight, Palette, Music, Repeat } from 'lucide-react';
import Link from 'next/link';

export const Hero: React.FC = () => {
  return (
    <section className="relative w-full min-h-screen pt-28 pb-16 md:pt-0 md:pb-0 flex items-center bg-emerald-950 overflow-hidden">
      {/* Background Gradients */}
      <div className="absolute top-0 right-0 w-1/2 h-full bg-gradient-to-l from-emerald-900/50 to-transparent pointer-events-none" />
      <div className="absolute -bottom-32 -left-32 w-96 h-96 bg-emerald-800/20 rounded-full blur-3xl" />
      
      <div className="container mx-auto px-6 grid md:grid-cols-2 gap-12 items-center z-10">
        
        {/* Left Content */}
        <motion.div
          initial={{ opacity: 0, x: -50 }}
          animate={{ opacity: 1, x: 0 }}
          transition={{ duration: 0.8, ease: "easeOut" }}
          className="space-y-6 md:pr-12"
        >
          {/* <div className="inline-flex items-center space-x-2 bg-white/10 backdrop-blur-sm border border-white/10 rounded-full px-3 py-1">
            <span className="flex h-2 w-2 rounded-full bg-orange-500 animate-pulse"></span>
            <span className="text-emerald-100 text-xs font-medium uppercase tracking-wider">Early Access Live</span>
          </div> */}
          
          <h1 className="font-heading text-4xl md:text-6xl lg:text-7xl font-bold text-white leading-[1.1]">
            Trade Skills. <br />
            <span className="text-emerald-400">Save Money.</span> <br />
            Grow Together.
          </h1>
          
          <p className="text-emerald-100/80 text-lg md:text-xl max-w-lg leading-relaxed">
            The marketplace where your talent is your currency. Swap design for coding, music for math, and build your network without spending a dime.
          </p>
          
          <div className="flex flex-col sm:flex-row gap-4 pt-4">
            <Link href="/register">
              <button className="bg-orange-600 hover:bg-orange-700 text-white font-semibold px-8 py-4 rounded-full transition-all hover:scale-105 hover:shadow-xl hover:shadow-orange-600/20 flex items-center justify-center gap-2">
                Start Swapping
                <ArrowRight size={20} />
              </button>
            </Link>
            <Link href="/login">
              <button className="bg-white/5 hover:bg-white/10 text-white border border-white/10 font-semibold px-8 py-4 rounded-full transition-all backdrop-blur-md">
                View Marketplace
              </button>
            </Link>
          </div>
          
          <div className="pt-8 flex items-center gap-4 text-sm text-emerald-200/60">
            <div className="flex -space-x-2">
              {[1, 2, 3, 4].map((i) => (
                <div key={i} className="w-8 h-8 rounded-full border-2 border-emerald-950 bg-gray-300 overflow-hidden">
                   <img src={`https://picsum.photos/100/100?random=${i}`} alt="User" className="w-full h-full object-cover" />
                </div>
              ))}
            </div>
            <p>Trusted by 1,000+ creators</p>
          </div>
        </motion.div>

        {/* Right Animation */}
        <div className="relative h-[400px] md:h-[600px] w-full flex items-center justify-center perspective-1000">
          {/* Card 1 */}
          <motion.div
            className="absolute z-20 top-1/4 left-4 md:left-12 bg-white p-6 rounded-2xl shadow-2xl border border-emerald-100 w-64 md:w-72"
            initial={{ y: 20, opacity: 0 }}
            animate={{ 
              y: [0, -15, 0],
              opacity: 1
            }}
            transition={{ 
              y: { duration: 6, repeat: Infinity, ease: "easeInOut" },
              opacity: { duration: 0.8 }
            }}
          >
            <div className="flex items-center gap-3 mb-4">
              <img src="https://picsum.photos/100/100?random=10" alt="Shovan Bhattarai" className="w-10 h-10 rounded-full object-cover border-2 border-emerald-500" />
              <div>
                <p className="font-bold text-gray-900 text-sm">Shovan Bhattarai</p>
                <p className="text-xs text-gray-500">Visual Designer</p>
              </div>
            </div>
            <div className="bg-emerald-50 rounded-xl p-3 flex items-center gap-3 mb-2">
              <div className="bg-emerald-200 p-2 rounded-lg text-emerald-800">
                <Palette size={20} />
              </div>
              <div>
                <p className="text-xs text-emerald-600 font-semibold uppercase tracking-wider">I Offer</p>
                <p className="text-sm font-bold text-gray-800">UI/UX Design</p>
              </div>
            </div>
          </motion.div>

          {/* Swap Icon */}
          <motion.div 
            className="absolute z-30 bg-white p-4 rounded-full shadow-xl text-orange-500 border-4 border-emerald-950"
            animate={{ rotate: 360 }}
            transition={{ duration: 20, repeat: Infinity, ease: "linear" }}
          >
            <Repeat size={32} strokeWidth={2.5} />
          </motion.div>

          {/* Card 2 */}
          <motion.div
            className="absolute z-10 bottom-1/4 right-4 md:right-12 bg-white p-6 rounded-2xl shadow-2xl border border-orange-100 w-64 md:w-72"
            initial={{ y: -20, opacity: 0 }}
            animate={{ 
              y: [0, 15, 0],
              opacity: 1
            }}
            transition={{ 
              y: { duration: 7, repeat: Infinity, ease: "easeInOut", delay: 0.5 },
              opacity: { duration: 0.8, delay: 0.3 }
            }}
          >
            <div className="flex items-center gap-3 mb-4 justify-end">
              <div className="text-right">
                <p className="font-bold text-gray-900 text-sm">Yunchuma Bantawa</p>
                <p className="text-xs text-gray-500">Musician</p>
              </div>
              <img src="https://picsum.photos/100/100?random=11" alt="Yunchuma Bantawa" className="w-10 h-10 rounded-full object-cover border-2 border-orange-500" />
            </div>
            <div className="bg-orange-50 rounded-xl p-3 flex items-center gap-3">
              <div>
                <p className="text-xs text-orange-600 font-semibold uppercase tracking-wider text-right">I Want</p>
                <p className="text-sm font-bold text-gray-800 text-right">Guitar Lessons</p>
              </div>
              <div className="bg-orange-200 p-2 rounded-lg text-orange-800">
                <Music size={20} />
              </div>
            </div>
          </motion.div>

          {/* Decorative Elements */}
          <div className="absolute w-full h-full border border-dashed border-white/10 rounded-full animate-[spin_60s_linear_infinite]" />
        </div>
      </div>
    </section>
  );
};
