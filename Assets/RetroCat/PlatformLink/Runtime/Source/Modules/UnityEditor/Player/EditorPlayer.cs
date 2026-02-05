#if UNITY_EDITOR
using System;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Player;
using ILogger = PlatformLink.PluginDebug.ILogger;

namespace PlatformLink.Platform.UnityEditor
{
    public class EditorPlayer : IPlayer
    {
        public bool Authorized { get; private set; }
        public string Name { get; private set; }

        private readonly ILogger _logger;

        public EditorPlayer(ILogger logger)
        {
            _logger = logger;
        }

        public EditorPlayer(string name, bool autorized)
        {
            Name = name;
            Authorized = autorized;
        }

        public void Authorize(Action<bool> onCompleted)
        {
            Authorized = true;
            _logger?.Log("Editor player authorized.");
            onCompleted?.Invoke(true);
        }
    }
}
#endif
