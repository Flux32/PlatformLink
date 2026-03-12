using PlatformLink.PluginDebug;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Analytics;

namespace RetroCat.PlatformLink.Runtime.Source.Modules.UnityEditor.Analytics
{
    public class EditorAnalyticsAdapter : IAnalyticsAdapter
    {
        private readonly ILogger _logger;
        
        public EditorAnalyticsAdapter(ILogger logger)
        {
            _logger = logger;
        }
        
        public void SendGameReady()
        {
            _logger.Log("game ready has been sent");
        }

        public void SendEvent(string eventName)
        {
            _logger.Log($"analytics event has been sent. event name = {eventName}");
        }

        public void SendEvent(string eventName, string eventDataJson)
        {
            _logger.Log($"analytics event with data has been sent. event name = {eventName}; data = {eventDataJson}");
        }
    }
}
