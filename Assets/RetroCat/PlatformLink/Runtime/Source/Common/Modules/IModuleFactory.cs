using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Advertisement;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Environment;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Purchases;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Storage;

namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules
{
    public interface IModuleFactory
    {
        IInterstitialAd CreateInterstitialAd();
        IRewardedAd CreateRewardedAd();
        IEnvironment CreateEnvironment();
        IStorage CreateStorage();
        IPurchases CreatePurchases();
    }
}