using System;
using JetBrains.Annotations;

namespace SitecoreInsights
{
    public class AnalyticsView
    {
        public long Id { get; set; }

        public DateTime Date { get; set; }

        [CanBeNull]
        public string MachineName { get; set; }

        [CanBeNull]
        public string Username { get; set; }

        [CanBeNull]
        public string Site { get; set; }

        [CanBeNull]
        public string SessionId { get; set; }

        [CanBeNull]
        public string Url { get; set; }

        /// <summary>
        /// Duration in milliseconds
        /// </summary>
        public long Duration { get; set; }

        public bool IsError { get; set; }

        [CanBeNull]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Do not track this view
        /// </summary>
        public bool Skip { get; set; }
    }
}