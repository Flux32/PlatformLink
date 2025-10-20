using PlatformLink.PluginDebug;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Advertisement;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Environment;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Leaderboards;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Player;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Purchases;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Storage;
using UnityEngine;
using ILogger = PlatformLink.PluginDebug.ILogger;

#if UNITY_EDITOR
using RetroCat.PlatformLink.Runtime.Source.Modules.UnityEditor.Factories;
#endif

#if UNITY_WEBGL
using PlatformLink.Platform.YandexGames;
using RetroCat.PlatformLink.Runtime.Source.Modules.YandexGames.Factories;
#endif

#if UNITY_EDITOR || UNITY_WEBGL
using System.Runtime.InteropServices;
#endif

#if UNITY_ANDROID
using GoogleMobileAds.Api;
using PlatformLink.Platform.Google;
#endif

namespace PlatformLink
{
    public class PLink
    {
        public static IAdvertisement Advertisement => Instance._advertisement;
        public static IEnvironment Environment => Instance._environment;
        public static IStorage Storage => Instance._storage;
        public static IPurchases Purchases => Instance._purchases;
        public static IAnalytics Analytics => Instance._analytics;
        public static ILeaderboard Leaderboard => Instance._leaderboard;
        //public static IPlayer Player => Instance._player;

        private readonly ILogger _logger = new PLinkLogger();

        public static PLink Instance
        {
            get
            {
                if (s_instance == null)
                    s_instance = new PLink();
                
                return s_instance;
            }
        }

        private static PLink s_instance;

        private IAdvertisement _advertisement;
        private IEnvironment _environment;
        private IStorage _storage;
        private IAnalytics _analytics;
        private IPurchases _purchases;
        private IPlayer _player;
        private ILeaderboard _leaderboard;
        
#if UNITY_WEBGL //TODO: Remove
        [DllImport("__Internal")]
        private static extern void jslib_convertString(string data);
#endif

        public static void Initialize() { }

        private PLink()
        {
            PlatformLinkObject.Initialize();

#if UNITY_EDITOR
            IModuleFactory moduleFactory = new EditorModuleFactory(_logger);
#elif UNITY_WEBGL
            IModuleFactory moduleFactory = new YandexModuleFactory(_logger);
#endif
            IInterstitialAd interstetialAd = moduleFactory.CreateInterstitialAd();
            IRewardedAd rewardedAd = moduleFactory.CreateRewardedAd();
            _advertisement = new Advertisement(interstetialAd, rewardedAd);
            _environment = moduleFactory.CreateEnvironment();
            _storage = moduleFactory.CreateStorage();
            _purchases = moduleFactory.CreatePurchases();
            _analytics = moduleFactory.CreateAnalytics();
            _leaderboard = moduleFactory.CreateLeaderboard();
        }
        
#if UNITY_EDITOR        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
        private static void ResetStatics()
        {
            s_instance = null;
            PlatformLinkObject.ClearInstance();
        }
#endif
    }
}