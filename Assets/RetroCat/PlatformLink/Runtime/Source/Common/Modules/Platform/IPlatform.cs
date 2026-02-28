using System;

namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules.Platform
{
    public interface IPlatform
    {
        bool Authorized { get; }
        void OpenLink(string url);
        void Authorize(Action<bool> onCompleted);
        void GetAllGames(Action<bool, AvailableGames> onCompleted);
    }
}
