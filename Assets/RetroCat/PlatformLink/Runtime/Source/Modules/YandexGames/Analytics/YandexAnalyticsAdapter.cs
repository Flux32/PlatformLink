using System.Runtime.InteropServices;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Analytics;
using UnityEngine;

namespace RetroCat.PlatformLink.Runtime.Source.Modules.YandexGames.Analytics
{
    public class YandexAnalyticsAdapter : MonoBehaviour, IAnalyticsAdapter
    {
        [DllImport("__Internal")]
        private static extern void jslib_sendGameReadyMessage();

        [DllImport("__Internal")]
        private static extern void jslib_sendAnalyticsEvent(string eventName);

        [DllImport("__Internal")]
        private static extern void jslib_sendAnalyticsEventWithData(string eventName, string eventDataJson);
        
        public void SendGameReady()
        {
            jslib_sendGameReadyMessage();
        }

        public void SendEvent(string eventName)
        {
            jslib_sendAnalyticsEvent(eventName);
        }

        public void SendEvent(string eventName, string eventDataJson)
        {
            jslib_sendAnalyticsEventWithData(eventName, eventDataJson);
        }
    }
}
