using System.Collections.Generic;
using System.Linq;
using PlatformLink.PluginDebug;

namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules.Analytics
{
    public class Analytics : IAnalytics
    {
        private readonly ILogger _logger;
        private readonly IAnalyticsService[] _analyticsServices;
        
        private bool _isGameReadySent;
        
        public Analytics(ILogger logger, IEnumerable<IAnalyticsService> analyticsService)
        {
            _logger = logger;
            _analyticsServices = analyticsService.ToArray();
        }
        
        public void SendGameReady()
        {
            if (_isGameReadySent)
            {
                _logger.LogError("Game ready had already been sent.");
                return;
            }
                
            _isGameReadySent = true;
            foreach (IAnalyticsService analyticsService in _analyticsServices)
            {
                analyticsService.SendGameReady();
            }
        }
    }
}
