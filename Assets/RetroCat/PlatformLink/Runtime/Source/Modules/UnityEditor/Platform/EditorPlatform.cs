#if UNITY_EDITOR
using System;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Platform;
using ILogger = PlatformLink.PluginDebug.ILogger;

namespace PlatformLink.Platform.UnityEditor
{
    public class EditorPlatform : IPlatform
    {
        public bool Authorized { get; private set; }
        public string Name { get; private set; }

        private readonly ILogger _logger;

        public EditorPlatform(ILogger logger)
        {
            _logger = logger;
        }

        public EditorPlatform(string name, bool autorized)
        {
            Name = name;
            Authorized = autorized;
        }

        public void Authorize(Action<bool> onCompleted)
        {
            Authorized = true;
            _logger?.Log("Editor platform authorized.");
            onCompleted?.Invoke(true);
        }

        public void GetAllGames(Action<bool, AvailableGames> onCompleted)
        {
            _logger?.Log("Editor platform GetAllGames requested. Returning empty list.");
            onCompleted?.Invoke(true, new AvailableGames(Array.Empty<AvailableGame>(), string.Empty));
        }
    }
}
#endif
