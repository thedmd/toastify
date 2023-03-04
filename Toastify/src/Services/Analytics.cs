using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net;
using log4net;
using Microsoft.Win32;
using Toastify.Core;
using Toastify.Model;

namespace Toastify.Services
{
    // ReSharper disable once PartialTypeWithSinglePart
    public static partial class Analytics
    {
        private static readonly ILog logger = LogManager.GetLogger(typeof(Analytics));

        #region Static Fields and Properties

        public static bool AnalyticsEnabled
        {
            get { return false; }
        }

        // ReSharper disable once UnusedAutoPropertyAccessor.Local
        private static string TrackingId { get; set; }

        #endregion

        #region Static Members

        internal static void Init()
        {
        }

        public static void Stop()
        {
            if (logger.IsDebugEnabled)
                logger.Debug("Analytics service terminated.");
        }

        // ReSharper disable once PartialMethodWithSinglePart
        static partial void SetTrackingId();

        #endregion

        public enum ToastifyEventCategory
        {
            General,
            Action
        }

        /// <summary>
        ///     Poor mans enum -> expanded string.
        ///     Once I've been using this for a while I may change this to a pure enum if
        ///     spaces in names prove to be annoying for querying / sorting the data
        /// </summary>
        public static class ToastifyEvent
        {
            #region Static Fields and Properties

            public static string Exception { get; } = "Exception";

            public static string AppLaunch { get; } = "Toastify.AppLaunched";
            public static string AppTermination { get; } = "Toastify.AppTermination";
            public static string SettingsLaunched { get; } = "Toastify.SettingsLaunched";

            #endregion

            public static class Action
            {
                #region Static Fields and Properties

                public static string Mute { get; } = "Toastify.Action.Mute";
                public static string VolumeDown { get; } = "Toastify.Action.VolumeDown";
                public static string VolumeUp { get; } = "Toasitfy.Action.VolumeUp";
                public static string ShowToast { get; } = "Toastify.Action.ShowToast";
                public static string ShowSpotify { get; } = "Toastify.Action.ShowSpotify";
                public static string CopyTrackInfo { get; } = "Toastify.Action.CopyTrackInfo";
                public static string PasteTrackInfo { get; } = "Toastify.Action.PasteTrackInfo";
                public static string FastForward { get; } = "Toastify.Action.FastForward";
                public static string Rewind { get; } = "Toastify.Action.Rewind";
                public static string Default { get; } = "Toastify.Action.";

                #endregion
            }
        }

        #region TrackPageHit

        public static void TrackPageHit(string documentPath)
        {
            TrackPageHit(documentPath, null, true);
        }

        public static void TrackPageHit(string documentPath, string title)
        {
            TrackPageHit(documentPath, title, true);
        }

        public static void TrackPageHit(string documentPath, bool interactive)
        {
            TrackPageHit(documentPath, null, interactive);
        }

        public static void TrackPageHit(string documentPath, string title, bool interactive)
        {
            if (!AnalyticsEnabled)
                return;

            if (logger.IsDebugEnabled)
                logger.Debug($"[Analytics] PageHit: ni={!interactive}, dp=\"{documentPath}\", dt=\"{title}\"");
        }

        #endregion TrackPageHit

        #region TrackEvent

        public static void TrackEvent(ToastifyEventCategory eventCategory, string eventAction)
        {
            TrackEvent(eventCategory, eventAction, null, -1);
        }

        public static void TrackEvent(ToastifyEventCategory eventCategory, string eventAction, string eventLabel)
        {
            TrackEvent(eventCategory, eventAction, eventLabel, -1);
        }

        public static void TrackEvent(ToastifyEventCategory eventCategory, string eventAction, int eventValue)
        {
            TrackEvent(eventCategory, eventAction, null, eventValue);
        }

        public static void TrackEvent(ToastifyEventCategory eventCategory, string eventAction, string eventLabel, int eventValue)
        {
            TrackEvent(eventCategory, eventAction, eventLabel, eventValue, null);
        }

        private static void TrackEvent(ToastifyEventCategory eventCategory, string eventAction, string eventLabel, int eventValue, IEnumerable<object> extraParameters)
        {
            if (!AnalyticsEnabled)
                return;

            if (logger.IsDebugEnabled)
                logger.Debug($"[Analytics] Event: ec=\"{eventCategory}\", ea=\"{eventAction}\", el=\"{eventLabel}\", ev=\"{eventValue}\"");
        }

        #endregion TrackEvent

        #region TrackException

        public static void TrackException(Exception exception)
        {
            TrackException(exception, false);
        }

        public static void TrackException(Exception exception, bool fatal)
        {
        }

        #endregion TrackException
    }
}