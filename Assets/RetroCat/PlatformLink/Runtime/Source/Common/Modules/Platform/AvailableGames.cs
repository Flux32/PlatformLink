using System;

namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules.Platform
{
    [Serializable]
    public class AvailableGames
    {
        public AvailableGame[] Games { get; }
        public string DeveloperUrl { get; }

        public AvailableGames(AvailableGame[] games, string developerUrl)
        {
            Games = games;
            DeveloperUrl = developerUrl;
        }
    }
}
