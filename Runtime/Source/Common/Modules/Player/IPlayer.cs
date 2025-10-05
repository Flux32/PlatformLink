using System;

namespace PlatformLink.Common
{
    public interface IPlayer
    {
        public bool Authorized { get; }
        public string Name { get; }
        public void Authorize(Action<bool> onCompleted);
    }
}