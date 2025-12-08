using System;
using UnityEngine.Networking;

namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules.Social
{
    public class Social : ISocial
    {
        private readonly IShareDialogAdapter _shareDialogAdapter;

        public Social(IShareDialogAdapter shareDialogAdapter)
        {
            _shareDialogAdapter = shareDialogAdapter;
        }

        public string CreateShareLink(SocialPlatform platform, ShareRequest request)
        {
            switch (platform)
            {
                case SocialPlatform.VK:
                    return BuildVkShareLink(request.Url, request.Text);
                default:
                    throw new ArgumentOutOfRangeException(nameof(platform), platform, "Unsupported social platform");
            }
        }

        public void ShowShareDialog(ShareRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (!IsNativeShareAvailable())
                throw new InvalidOperationException("Native share dialog is not available on this platform.");

            _shareDialogAdapter.Show(request);
        }

        public bool IsNativeShareAvailable()
        {
            return _shareDialogAdapter.IsAvailable;
        }

        private static string BuildVkShareLink(string url, string comment)
        {
            string encodedUrl = UnityWebRequest.EscapeURL(url ?? string.Empty);
            string encodedComment = UnityWebRequest.EscapeURL(comment ?? string.Empty);
            return $"https://vk.com/share.php?url={encodedUrl}&comment={encodedComment}";
        }
    }
}
