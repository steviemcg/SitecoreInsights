using Sitecore;
using Sitecore.Pipelines.HttpRequest;

namespace SitecoreInsights.Sc.Pipelines.HttpRequestBegin
{
    [UsedImplicitly]
    public class AnalyticsViewStart : HttpRequestProcessor
    {
        public override void Process(HttpRequestArgs args)
        {
            AnalyticsViewContext.Start();
        }
    }
}