using PlatformLink.PluginDebug;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Analytics;

namespace RetroCat.PlatformLink.Runtime.Source.Modules.UnityEditor.Analytics
{
    public class EditorAnalyticsService : IAnalyticsService
    {
        private readonly ILogger _logger;
        
        public EditorAnalyticsService(ILogger logger)
        {
            _logger = logger;
        }
        
        public void SendGameReady()
        {
            _logger.Log("game ready has been sent");
        }
    }
}
