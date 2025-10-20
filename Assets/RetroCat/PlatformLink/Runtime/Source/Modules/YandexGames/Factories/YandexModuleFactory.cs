using PlatformLink;
using PlatformLink.Platform.YandexGames;
using PlatformLink.PluginDebug;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Advertisement;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Analytics;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Environment;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Leaderboards;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Purchases;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Storage;
using RetroCat.PlatformLink.Runtime.Source.Modules.YandexGames.Analytics;
using RetroCat.PlatformLink.Runtime.Source.Modules.YandexGames.Leaderboards;
using RetroCat.PlatformLink.Runtime.Source.Modules.YandexGames.Purchases;

namespace RetroCat.PlatformLink.Runtime.Source.Modules.YandexGames.Factories
{
    public class YandexModuleFactory : IModuleFactory
    {
        private readonly ILogger _logger;

        public YandexModuleFactory(ILogger logger)
        {
            _logger = logger;
        }
        
        public IEnvironment CreateEnvironment()
        {
            return PlatformLinkObject.AddComponent<YandexEnvironment>();
        }

        public IInterstitialAd CreateInterstitialAd()
        {
            return PlatformLinkObject.AddComponent<YandexInterstitialAd>();
        }

        public IRewardedAd CreateRewardedAd()
        {
            return PlatformLinkObject.AddComponent<YandexRewardedAd>();
        }

        public IStorage CreateStorage()
        {
            return PlatformLinkObject.AddComponent<YandexStorage>();
        }

        public IPurchases CreatePurchases()
        {
            return PlatformLinkObject.AddComponent<YandexPurchases>();
        }
        
        public IAnalytics CreateAnalytics()
        {
            YandexAnalyticsService yandexAnalyticsService = PlatformLinkObject.AddComponent<YandexAnalyticsService>();
            return new Common.Modules.Analytics.Analytics(_logger, new IAnalyticsService[] { yandexAnalyticsService });
        }
        
        public ILeaderboard CreateLeaderboard()
        {
            return PlatformLinkObject.AddComponent<YandexLeaderboard>();
        }
    }
}