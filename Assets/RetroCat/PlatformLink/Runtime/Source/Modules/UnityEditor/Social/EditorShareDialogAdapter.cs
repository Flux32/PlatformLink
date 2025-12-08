#if UNITY_EDITOR
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Social;
using ILogger = PlatformLink.PluginDebug.ILogger;

namespace RetroCat.PlatformLink.Runtime.Source.Modules.UnityEditor.Social
{
    public class EditorShareDialogAdapter : IShareDialogAdapter
    {
        private readonly ILogger _logger;

        public EditorShareDialogAdapter(ILogger logger)
        {
            _logger = logger;
        }

        public bool IsAvailable => false;

        public void Show(ShareRequest request)
        {
            _logger?.LogWarning("Native share dialog is not available in the Unity Editor.");
        }
    }
}
#endif
