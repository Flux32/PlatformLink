using PlatformLink.PluginDebug;
using PlatformLink.Common;
using UnityEngine;
using ILogger = PlatformLink.PluginDebug.ILogger;

#if UNITY_EDITOR
using PlatformLink.Platform.UnityEditor;
#endif

#if UNITY_WEBGL
using PlatformLink.Platform.YandexGames;
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
        
        //public static IPlayer Player => Instance._player;
        //public static ILeaderboard Leaderboard => Instance._leaderboard;

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

        private readonly IAdvertisement _advertisement;
        private readonly IEnvironment _environment;
        private readonly IStorage _storage;
        private readonly IPlayer _player;
        private readonly IPurchases _purchases;

        //private const string GameReadyMessage = "Sended game ready message";

#if UNITY_WEBGL //TODO: Remove
        [DllImport("__Internal")]
        private static extern void jslib_convertString(string data);
#endif

        public static void Initialize()
        {
            Instance.Init();
        }

        private void Init()
        {
            PlatformLinkObject.Initialize();
        }

        private PLink()
        {
#if UNITY_EDITOR
            IModuleFactory moduleFactory = new EditorModuleFactory();
#elif UNITY_WEBGL
            IModuleFactory moduleFactory = new YandexModuleFactory();
#endif
            IInterstitialAd interstetialAd = moduleFactory.CreateInterstitialAd();
            IRewardedAd rewardedAd = moduleFactory.CreateRewardedAd();
            _advertisement = new Advertisement(interstetialAd, rewardedAd);
            _environment = moduleFactory.CreateEnvironment();
            _storage = moduleFactory.CreateStorage();
            _purchases = moduleFactory.CreatePurchases();
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