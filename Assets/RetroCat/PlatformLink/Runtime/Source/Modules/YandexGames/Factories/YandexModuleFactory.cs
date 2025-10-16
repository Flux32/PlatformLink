using PlatformLink;
using PlatformLink.Platform.YandexGames;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Advertisement;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Environment;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Purchases;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Storage;
using RetroCat.PlatformLink.Runtime.Source.Modules.YandexGames.Purchases;

namespace RetroCat.PlatformLink.Runtime.Source.Modules.YandexGames.Factories
{
    public class YandexModuleFactory : IModuleFactory
    {
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
    }
}