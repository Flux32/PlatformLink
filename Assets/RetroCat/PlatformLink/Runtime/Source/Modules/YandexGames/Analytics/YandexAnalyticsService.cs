using System.Runtime.InteropServices;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Analytics;
using UnityEngine;

namespace RetroCat.PlatformLink.Runtime.Source.Modules.YandexGames.Analytics
{
    public class YandexAnalyticsService : MonoBehaviour, IAnalyticsService
    {
        [DllImport("__Internal")]
        private static extern void jslib_sendGameReadyMessage();
        
        public void SendGameReady()
        {
            jslib_sendGameReadyMessage();
        }
    }
}