#if UNITY_EDITOR
using System;
using PlatformLink.Lifecycle;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.RemoteConfig;
using ILogger = PlatformLink.PluginDebug.ILogger;

namespace RetroCat.PlatformLink.Runtime.Source.Modules.UnityEditor.RemoteConfig
{
    public class EditorRemoteConfig : IRemoteConfig, IInitializable, IRemoteConfigLoader
    {
        private readonly ILogger _logger;

        public EditorRemoteConfig(ILogger logger)
        {
            _logger = logger;
        }

        public void Initialize()
        {
        }

        public void Load(Action<bool> onCompleted)
        {
            onCompleted?.Invoke(true);
        }

        public T GetRemoteConfig<T>(string key, T fallbackValue)
        {
            _logger?.LogError($"Remote config is not available in the Unity Editor. Key: {key ?? "<null>"}");
            return fallbackValue;
        }
    }
}
#endif
