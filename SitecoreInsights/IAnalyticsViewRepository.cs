namespace SitecoreInsights
{
    public interface IAnalyticsViewRepository
    {
        void Add(AnalyticsView view);

        void EnsureSchema();
    }
}