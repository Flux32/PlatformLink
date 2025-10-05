using PlatformLink.Common;

public interface IModuleFactory
{
    IInterstitialAd CreateInterstitialAd();
    IRewardedAd CreateRewardedAd();
    IEnvironment CreateEnvironment();
    IStorage CreateStorage();
}