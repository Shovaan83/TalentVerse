"use client";

import { useRouter } from "next/navigation";
import AuthLayout from "../components/AuthLayout";
import Enable2FA from "../components/Enable2FA";
import { useEffect, useState } from "react";

export default function Setup2FAPage() {
  const router = useRouter();
  const [isAuthenticated, setIsAuthenticated] = useState(false);

  useEffect(() => {
    const token = localStorage.getItem("token");
    if (!token) {
      router.push("/login");
    } else {
      setIsAuthenticated(true);
    }
  }, [router]);

  if (!isAuthenticated) {
    return null;
  }

  return (
    <AuthLayout
      title="Secure Your Account"
      subtitle="Two-factor authentication adds an extra layer of security to your TalentVerse account."
    >
      <Enable2FA
        onSuccess={() => router.push("/profile")}
        onSkip={() => router.push("/profile")}
      />
    </AuthLayout>
  );
}
