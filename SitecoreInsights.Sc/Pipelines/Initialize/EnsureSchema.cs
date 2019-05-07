using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Sitecore.Diagnostics;
using Sitecore.Pipelines;

namespace SitecoreInsights.Sc.Pipelines.Initialize
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class EnsureSchema
    {
        private readonly IAnalyticsViewRepository _analyticsViewRepository;

        public EnsureSchema(IAnalyticsViewRepository analyticsViewRepository)
        {
            _analyticsViewRepository = analyticsViewRepository;
        }

        public void Process(PipelineArgs args)
        {
            Task.Run(() => ProcessInternal());
        }

        private void ProcessInternal()
        {
            try
            {
                _analyticsViewRepository.EnsureSchema();
                AnalyticsViewContext.Enabled = true;
            }
            catch (Exception ex)
            {
                Log.Error("Cannot create AnalyticsViews schema", ex, this);
            }
        }
    }
}