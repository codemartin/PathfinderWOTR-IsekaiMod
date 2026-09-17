using HarmonyLib;
using Owlcat.Runtime.Core.Logging;
using System;
using static IsekaiMod.Main;

namespace IsekaiMod.Utilities {
    /// <summary>
    /// The game routes exceptions from ability delivery, buff application and similar paths through
    /// PFLog.Default.Exception and carries on, so an ability that throws mid-delivery simply appears to do
    /// nothing. The game's own log is usually off on modded installs, which hides the cause entirely.
    /// This mirrors those exceptions into the mod log, throttled, so they can be read.
    /// </summary>
    [HarmonyPatch(typeof(LogChannel), nameof(LogChannel.Exception), new Type[] { typeof(Exception), typeof(string), typeof(object[]) })]
    internal static class SwallowedExceptionDiagnostics {
        private const int MaxPerMinute = 20;
        private static DateTime windowStartUtc = DateTime.UtcNow;
        private static int loggedInWindow;

        [HarmonyPrefix]
        private static void Prefix(Exception ex, string message) {
            try {
                if (ex == null) return;
                DateTime now = DateTime.UtcNow;
                if (now - windowStartUtc > TimeSpan.FromMinutes(1)) {
                    windowStartUtc = now;
                    loggedInWindow = 0;
                }
                if (loggedInWindow >= MaxPerMinute) return;
                loggedInWindow++;
                IsekaiContext.Logger.Log($"Game-swallowed exception{(string.IsNullOrEmpty(message) ? "" : " (" + message + ")")}: {ex.GetType().Name}: {ex.Message}\n{ex.StackTrace}");
            } catch (Exception) {
                // diagnostics only
            }
        }
    }
}
