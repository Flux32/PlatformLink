using PlatformLink.Common;

namespace PlatformLink.Platform.YandexGames
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
    }
}