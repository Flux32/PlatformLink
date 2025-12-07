using System;
using UnityEngine.Networking;

namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules.Social
{
    public class Social : ISocial
    {
        public string CreateShareLink(SocialPlatform platform, string url, string comment)
        {
            switch (platform)
            {
                case SocialPlatform.VK:
                    return BuildVkShareLink(url, comment);
                default:
                    throw new ArgumentOutOfRangeException(nameof(platform), platform, "Unsupported social platform");
            }
        }

        private static string BuildVkShareLink(string url, string comment)
        {
            string encodedUrl = UnityWebRequest.EscapeURL(url ?? string.Empty);
            string encodedComment = UnityWebRequest.EscapeURL(comment ?? string.Empty);
            return $"https://vk.com/share.php?url={encodedUrl}&comment={encodedComment}";
        }
    }
}
