"use client";

import { motion, AnimatePresence } from "framer-motion";
import { Mail, CheckCircle, AlertCircle, Loader2 } from "lucide-react";

interface EmailStatusNotificationProps {
  status: "idle" | "sending" | "sent" | "error";
  message?: string;
  email?: string;
}

export default function EmailStatusNotification({
  status,
  message,
  email,
}: EmailStatusNotificationProps) {
  if (status === "idle") return null;

  const getIcon = () => {
    switch (status) {
      case "sending":
        return <Loader2 className="w-5 h-5 animate-spin text-blue-600" />;
      case "sent":
        return <CheckCircle className="w-5 h-5 text-emerald-600" />;
      case "error":
        return <AlertCircle className="w-5 h-5 text-red-600" />;
      default:
        return <Mail className="w-5 h-5 text-gray-600" />;
    }
  };

  const getBackgroundColor = () => {
    switch (status) {
      case "sending":
        return "bg-blue-50 border-blue-200";
      case "sent":
        return "bg-emerald-50 border-emerald-200";
      case "error":
        return "bg-red-50 border-red-200";
      default:
        return "bg-gray-50 border-gray-200";
    }
  };

  const getTextColor = () => {
    switch (status) {
      case "sending":
        return "text-blue-800";
      case "sent":
        return "text-emerald-800";
      case "error":
        return "text-red-800";
      default:
        return "text-gray-800";
    }
  };

  const getTitle = () => {
    switch (status) {
      case "sending":
        return "Sending email...";
      case "sent":
        return "Email sent successfully!";
      case "error":
        return "Failed to send email";
      default:
        return "";
    }
  };

  const getDescription = () => {
    if (message) return message;
    
    switch (status) {
      case "sending":
        return `Sending verification code to ${email || "your email"}...`;
      case "sent":
        return `Check ${email || "your email"} for the verification code.`;
      case "error":
        return "There was an error sending the email. Please try again.";
      default:
        return "";
    }
  };

  return (
    <AnimatePresence>
      <motion.div
        initial={{ opacity: 0, y: -10 }}
        animate={{ opacity: 1, y: 0 }}
        exit={{ opacity: 0, y: -10 }}
        className={`flex items-start gap-3 p-4 border rounded-xl ${getBackgroundColor()}`}
      >
        <div className="flex-shrink-0 mt-0.5">{getIcon()}</div>
        <div className="flex-1">
          <h4 className={`font-semibold ${getTextColor()}`}>{getTitle()}</h4>
          <p className={`text-sm mt-1 ${getTextColor()} opacity-90`}>
            {getDescription()}
          </p>
        </div>
      </motion.div>
    </AnimatePresence>
  );
}
