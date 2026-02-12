using System;

namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules.Platform
{
    [Serializable]
    public class AvailableGame
    {
        public string AppId { get; }
        public string Title { get; }
        public string Url { get; }
        public string CoverUrl { get; }
        public string IconUrl { get; }

        public AvailableGame(
            string appId,
            string title,
            string url,
            string coverUrl,
            string iconUrl)
        {
            AppId = appId;
            Title = title;
            Url = url;
            CoverUrl = coverUrl;
            IconUrl = iconUrl;
        }
    }
}
