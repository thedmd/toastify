﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using JetBrains.Annotations;
using log4net;
using Microsoft.Win32;
using NativeWindows = ToastifyAPI.Native.Windows;

namespace ToastifyAPI
{
    public static partial class Spotify
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Spotify));

        #region Static Fields and Properties

        /// <summary>
        ///     List of names the Spotify main window had across different versions of the software.
        /// </summary>
        private static readonly List<string> spotifyMainWindowNames = new List<string>
        {
            "SpotifyMainWindow",
            "Chrome_WidgetWin_0", // Since v1.0.75.483.g7ff4a0dc
            "Chrome_WidgetWin_1", // Since v1.123.773.0
        };

        public static string ProcessName { get; } = "spotify";

        #endregion

        #region Static Members

        [NotNull]
        public static string GetSpotifyPath()
        {
            string spotifyPath = GetSpotifyPath_platform();
            if (string.IsNullOrWhiteSpace(spotifyPath))
            {
                spotifyPath = GetSpotifyPath_common();
                if (string.IsNullOrWhiteSpace(spotifyPath) || !File.Exists(spotifyPath))
                    throw new FileNotFoundException("Could not find spotify executable", "Spotify.exe");
            }

            return spotifyPath;
        }

        [CanBeNull]
        private static string GetSpotifyPath_common()
        {
            string spotifyPath = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Spotify", string.Empty, string.Empty) as string;

            // Try the Uninstall keys
            if (string.IsNullOrWhiteSpace(spotifyPath))
                spotifyPath = Registry.GetValue(@"HKEY_CURRENT_USER\Software\Microsoft\Windows\CurrentVersion\Uninstall\Spotify", "InstallLocation", string.Empty) as string;

            if (string.IsNullOrWhiteSpace(spotifyPath))
                spotifyPath = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Spotify", "InstallLocation", string.Empty) as string;

            if (string.IsNullOrWhiteSpace(spotifyPath))
                spotifyPath = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\WOW6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Spotify", "InstallLocation", string.Empty) as string;

            return !string.IsNullOrWhiteSpace(spotifyPath) ? Path.Combine(spotifyPath, "Spotify.exe") : spotifyPath;
        }

        [CanBeNull]
        public static Process FindSpotifyProcess()
        {
            if (logger.IsDebugEnabled)
                logger.Debug("Looking for Spotify process...");

            List<Process> spotifyProcesses = Process.GetProcessesByName(ProcessName).ToList();
            List<Process> windowedProcesses = spotifyProcesses.Where(p => p.MainWindowHandle != IntPtr.Zero).ToList();

            if (windowedProcesses.Count > 1)
            {
                IEnumerable<string> classNames = windowedProcesses.Select(p => $"\"{NativeWindows.GetClassName(p.MainWindowHandle)}\"");
                logger.Warn($"More than one ({windowedProcesses.Count}) \"{ProcessName}\" process has a non-null main window: {string.Join(", ", classNames)}");
            }

            Process process = windowedProcesses.FirstOrDefault();

            // If none of the Spotify processes found has a valid MainWindowHandle,
            // then Spotify has probably been minimized to the tray: we need to check every window.
            if (process == null)
            {
                foreach (Process p in spotifyProcesses)
                {
                    if (IsMainSpotifyProcess((uint)p.Id))
                        return p;
                }
            }

            return process;
        }

        public static IntPtr GetMainWindowHandle(uint pid)
        {
            if (pid == 0)
                return IntPtr.Zero;

            List<IntPtr> windows = NativeWindows.GetProcessWindows(pid);
            List<IntPtr> possibleMainWindows = windows.Where(h =>
            {
                string className = NativeWindows.GetClassName(h);
                string windowName = NativeWindows.GetWindowTitle(h);
                return !string.IsNullOrWhiteSpace(windowName) && spotifyMainWindowNames.Contains(className);
            }).ToList();

            if (possibleMainWindows.Count > 1)
            {
                IEnumerable<string> classNames = possibleMainWindows.Select(h => $"\"{NativeWindows.GetClassName(h)}\"");
                logger.Warn($"More than one ({possibleMainWindows.Count}) possible main windows located for Spotify: {string.Join(", ", classNames)}");
            }

            return possibleMainWindows.FirstOrDefault();
        }

        public static bool IsMainSpotifyProcess(uint pid)
        {
            List<IntPtr> windows = NativeWindows.GetProcessWindows(pid);
            IntPtr hWnd = windows.FirstOrDefault(h => spotifyMainWindowNames.Contains(NativeWindows.GetClassName(h)));
            return hWnd != IntPtr.Zero;
        }

        [CanBeNull]
        public static string GetSpotifyVersion()
        {
            string exePath = null;
            try
            {
                exePath = GetSpotifyPath();
            }
            catch
            {
                // ignore
            }

            return exePath != null && File.Exists(exePath) ? FileVersionInfo.GetVersionInfo(exePath).FileVersion : null;
        }

        #endregion
    }
}