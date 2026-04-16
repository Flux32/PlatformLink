using System;
using System.Collections.Generic;

namespace RetroCat.PlatformLink.Editor.Modules
{
    public static class ModuleAdapterRegistry
    {
        public readonly struct AdapterDescriptor
        {
            public string DisplayName { get; }
            public PlatformModuleKind Kind { get; }
            public Type ConfigType { get; }
            public Func<IModuleAdapterConfig> CreateConfig { get; }

            public AdapterDescriptor(string displayName, PlatformModuleKind kind, Type configType, Func<IModuleAdapterConfig> createConfig)
            {
                DisplayName = displayName;
                Kind = kind;
                ConfigType = configType;
                CreateConfig = createConfig;
            }
        }

        private static readonly Dictionary<PlatformSettingsType, List<AdapterDescriptor>> s_registry =
            new Dictionary<PlatformSettingsType, List<AdapterDescriptor>>
            {
                {
                    PlatformSettingsType.Editor, new List<AdapterDescriptor>
                    {
                        new AdapterDescriptor("Editor", PlatformModuleKind.InterstitialAd, typeof(EditorInterstitialAdapterConfig), () => new EditorInterstitialAdapterConfig()),
                        new AdapterDescriptor("Editor", PlatformModuleKind.RewardedAd, typeof(EditorRewardedAdapterConfig), () => new EditorRewardedAdapterConfig()),
                    }
                },
                {
                    PlatformSettingsType.YandexGames, new List<AdapterDescriptor>
                    {
                        new AdapterDescriptor("Yandex Games", PlatformModuleKind.InterstitialAd, typeof(YandexGamesInterstitialAdapterConfig), () => new YandexGamesInterstitialAdapterConfig()),
                        new AdapterDescriptor("Yandex Games", PlatformModuleKind.RewardedAd, typeof(YandexGamesRewardedAdapterConfig), () => new YandexGamesRewardedAdapterConfig()),
                    }
                },
                {
                    PlatformSettingsType.Android, new List<AdapterDescriptor>
                    {
                        new AdapterDescriptor("Admob", PlatformModuleKind.InterstitialAd, typeof(AdmobInterstitialAdapterConfig), () => new AdmobInterstitialAdapterConfig()),
                        new AdapterDescriptor("Yandex Mobile Ads", PlatformModuleKind.InterstitialAd, typeof(YandexMobileAdsInterstitialAdapterConfig), () => new YandexMobileAdsInterstitialAdapterConfig()),
                        new AdapterDescriptor("Admob", PlatformModuleKind.RewardedAd, typeof(AdmobRewardedAdapterConfig), () => new AdmobRewardedAdapterConfig()),
                        new AdapterDescriptor("Yandex Mobile Ads", PlatformModuleKind.RewardedAd, typeof(YandexMobileAdsRewardedAdapterConfig), () => new YandexMobileAdsRewardedAdapterConfig()),
                    }
                },
                {
                    PlatformSettingsType.Ios, new List<AdapterDescriptor>
                    {
                        new AdapterDescriptor("Admob", PlatformModuleKind.InterstitialAd, typeof(AdmobInterstitialAdapterConfig), () => new AdmobInterstitialAdapterConfig()),
                        new AdapterDescriptor("Admob", PlatformModuleKind.RewardedAd, typeof(AdmobRewardedAdapterConfig), () => new AdmobRewardedAdapterConfig()),
                    }
                },
            };

        public static IReadOnlyList<AdapterDescriptor> GetAdapters(PlatformSettingsType platformType)
        {
            return s_registry.TryGetValue(platformType, out List<AdapterDescriptor> adapters)
                ? adapters
                : Array.Empty<AdapterDescriptor>();
        }

        public static string GetKindDisplayName(PlatformModuleKind kind)
        {
            switch (kind)
            {
                case PlatformModuleKind.InterstitialAd:
                    return "InterstitialAd";
                case PlatformModuleKind.RewardedAd:
                    return "RewardedAd";
                default:
                    return kind.ToString();
            }
        }
    }
}
