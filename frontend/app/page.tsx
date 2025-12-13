'use client';

import React from 'react';
import { Navbar } from './(auth)/components/Navbar';
import { Hero } from './(auth)/components/Hero';
import { Features } from './(auth)/components/Features';
import { HowItWorks } from './(auth)/components/HowItWorks';
import { CTA } from './(auth)/components/CTA';
import { Footer } from './(auth)/components/Footer';

const App: React.FC = () => {
  return (
    <div className="min-h-screen bg-white text-slate-900 selection:bg-emerald-200 selection:text-emerald-950">
      <Navbar />
      <main>
        <Hero />
        <Features />
        <HowItWorks />
        <CTA />
      </main>
      <Footer />
    </div>
  );
};

export default App;
