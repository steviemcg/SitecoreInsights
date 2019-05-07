using System;
using System.Threading.Tasks;
using Sitecore;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.HttpRequest;

namespace SitecoreInsights.Sc.Pipelines.HttpRequestProcessed
{
    [UsedImplicitly]
    public class AnalyticsViewEnd : HttpRequestProcessor
    {
        private readonly IAnalyticsViewRepository _analyticsViewRepository;

        public AnalyticsViewEnd(IAnalyticsViewRepository analyticsViewRepository)
        {
            _analyticsViewRepository = analyticsViewRepository ?? throw new ArgumentNullException(nameof(analyticsViewRepository));
        }

        public override void Process(HttpRequestArgs args)
        {
            try
            {
                if (!AnalyticsViewContext.Enabled)
                {
                    return;
                }

                var httpContext = args.HttpContext;

                if (httpContext.Request.QueryString["pg"] != null)
                {
                    return;
                }

                var view = AnalyticsViewContext.View;
                if (view.Skip)
                {
                    return;
                }

                if (view.Site == null && Context.Site == null)
                {
                    return;
                }

                view.Url = view.Url ?? httpContext.Request.RawUrl;
                if (view.Url != null && view.Url.StartsWith("/-/media/"))
                {
                    return;
                }

                view.MachineName = Environment.MachineName;
                view.Username = view.Username ?? Context.User.Name;
                view.Site = view.Site ?? Context.Site.Name;
                view.SessionId = httpContext.Session?.SessionID ?? httpContext.Request.Cookies["ASP.Net_SessionId"]?.Value;
                view.Duration = AnalyticsViewContext.Stopwatch.ElapsedMilliseconds;

                Task.Run(() => _analyticsViewRepository.Add(view));
            }
            catch (Exception ex)
            {
                Log.Error("Error logging AnalyticsView", ex, this);
            }
        }
    }
}