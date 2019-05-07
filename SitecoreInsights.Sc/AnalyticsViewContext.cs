using System;
using System.Diagnostics;

namespace SitecoreInsights.Sc
{
    public static class AnalyticsViewContext
    {
        private const string ViewContextKey = "SitecoreInsights.AnalyticsView";
        private const string StopwatchContextKey = "SitecoreInsights.Stopwatch";

        public static bool Enabled { get; set; }

        public static AnalyticsView View => (AnalyticsView)Sitecore.Context.Items[ViewContextKey];

        public static Stopwatch Stopwatch => (Stopwatch)Sitecore.Context.Items[StopwatchContextKey];

        public static void Start()
        {
            Sitecore.Context.Items[StopwatchContextKey] = Stopwatch.StartNew();
            var analyticsView = new AnalyticsView
            {
                Date = DateTime.UtcNow
            };

            Sitecore.Context.Items[ViewContextKey] = analyticsView;
        }
    }
}