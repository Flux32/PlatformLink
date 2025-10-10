#if UNITY_EDITOR
using log4net.Core;
using System;
using PlatformLink.Common;

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
            onCompleted?.Invoke(true);
            Authorized = true;
        }
    }
}
#endif