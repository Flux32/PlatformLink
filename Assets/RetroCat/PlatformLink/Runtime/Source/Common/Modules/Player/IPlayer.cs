using System;

namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules.Player
{
    public interface IPlayer
    {
        public bool Authorized { get; }
        public string Name { get; }
        public void Authorize(Action<bool> onCompleted);
    }
}