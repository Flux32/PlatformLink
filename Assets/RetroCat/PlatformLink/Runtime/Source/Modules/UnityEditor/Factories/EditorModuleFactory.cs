#if UNITY_EDITOR
using PlatformLink.Platform.UnityEditor;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Analytics;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Device;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Environment;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Leaderboards;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Platform;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Purchases;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.RemoteConfig;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Social;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Storage;
using RetroCat.PlatformLink.Runtime.Source.Modules.UnityEditor.Analytics;
using RetroCat.PlatformLink.Runtime.Source.Modules.UnityEditor.Leaderboards;
using RetroCat.PlatformLink.Runtime.Source.Modules.UnityEditor.Purchases;
using RetroCat.PlatformLink.Runtime.Source.Modules.UnityEditor.RemoteConfig;
using RetroCat.PlatformLink.Runtime.Source.Modules.UnityEditor.Social;
using UnityEngine;
using DeviceType = RetroCat.PlatformLink.Runtime.Source.Common.Modules.Environment.DeviceType;
using ILogger = PlatformLink.PluginDebug.ILogger;

namespace RetroCat.PlatformLink.Runtime.Source.Modules.UnityEditor.Factories
{
    public class EditorModuleFactory : IModuleFactory
    {
        private readonly ILogger _logger;

        public EditorModuleFactory(ILogger logger)
        {
            _logger = logger;
        }

        private EditorSettings EditorSettings => PlatformLinkSettings.Instance.Editor;

        public IEnvironment CreateEnvironment()
        {
            return new EditorEnvironment(EditorSettings.Environment);
        }

        public IStorage CreateStorage()
        {
            return new EditorStorage(_logger, EditorSettings.Storage.SaveFilePath);
        }

        public IPurchases CreatePurchases()
        {
            return new EditorPurchases(_logger, EditorSettings.Purchases);
        }

        public IAnalytics CreateAnalytics()
        {
            return new Common.Modules.Analytics.Analytics(_logger, new IAnalyticsAdapter[]
                { new EditorAnalyticsAdapter(_logger) });
        }

        public ILeaderboard CreateLeaderboard()
        {
            return new EditorLeaderboard(_logger, EditorSettings.Leaderboard);
        }

        public ISocial CreateSocial()
        {
            IShareDialogAdapter shareDialogAdapter = new EditorShareDialogAdapter(_logger);
            return new Common.Modules.Social.Social(shareDialogAdapter);
        }

        public IPlatform CreatePlatform()
        {
            return new EditorPlatform(_logger, EditorSettings.Platform);
        }

        public IRemoteConfig CreateRemoteConfig()
        {
            return new EditorRemoteConfig(_logger);
        }

        public IDevice CreateDevice()
        {
            return new DefaultDevice(_logger);
        }
    }
}
#endif
