using System;
using PlatformLink.PluginDebug;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Advertisement;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Environment;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Leaderboards;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Player;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Purchases;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.RemoteConfig;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Social;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Storage;
using UnityEngine;
using ILogger = PlatformLink.PluginDebug.ILogger;
using PlatformLink.Lifecycle;

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
        public static ISocial Social => Instance._social;
        public static IRemoteConfig RemoteConfig => Instance._remoteConfig;
        //public static IPlayer Player => Instance._player;
        public static event Action Initilized;
        public static bool IsInitialized { get; private set; }
        
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
        private ISocial _social;
        private IRemoteConfig _remoteConfig;
        
#if UNITY_WEBGL //TODO: Remove
        [DllImport("__Internal")]
        private static extern void jslib_convertString(string data);
#endif

        public static void Initialize(Action onCompleted)
        {
            Instance.Init(onCompleted);
        }

        private PLink() { }

        private void Init(Action onCompleted)
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
            _social = moduleFactory.CreateSocial();
            _remoteConfig = moduleFactory.CreateRemoteConfig();
            
#if UNITY_WEBGL && !UNITY_EDITOR
            YandexCore core = PlatformLinkObject.AddComponent<YandexCore>();
            
            core.Initialize(() =>
            {
                InitializeRemoteConfig(() => OnInitialized(onCompleted));
            });
#else
            InitializeRemoteConfig(() => OnInitialized(onCompleted));
#endif
        }

        private void OnInitialized(Action callback)
        {
            IsInitialized = true;
            Initilized?.Invoke();
            callback?.Invoke();
        }

        private void InitializeRemoteConfig(Action onCompleted)
        {
            if (_remoteConfig is IRemoteConfigLoader loader)
            {
                loader.Load(_ => onCompleted?.Invoke());
                return;
            }

            if (_remoteConfig is IInitializable initializable)
            {
                initializable.Initialize();
            }

            onCompleted?.Invoke();
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
