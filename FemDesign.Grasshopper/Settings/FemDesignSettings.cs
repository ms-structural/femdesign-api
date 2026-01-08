// https://strusoft.com/
using Grasshopper;
using System;

namespace FemDesign.Grasshopper
{
    /// <summary>
    /// Global settings for FEM-Design Grasshopper components.
    /// </summary>
    public static class FemDesignSettings
    {
        private const string ASYNC_MODE_KEY = "FemDesign_AsyncMode";

        /// <summary>
        /// Gets or sets whether async (non-blocking) mode is enabled for FEM-Design components.
        /// When true, components will run in the background without blocking the Grasshopper UI.
        /// When false, components run synchronously and block the UI until complete.
        /// </summary>
        public static bool AsyncModeEnabled
        {
            get => Instances.Settings.GetValue(ASYNC_MODE_KEY, true);
            set
            {
                Instances.Settings.SetValue(ASYNC_MODE_KEY, value);
                RaiseAsyncModeChanged();
            }
        }

        /// <summary>
        /// Event raised when the async mode setting changes.
        /// </summary>
        public static event Action AsyncModeChanged;

        internal static void RaiseAsyncModeChanged()
        {
            AsyncModeChanged?.Invoke();
        }
    }
}

