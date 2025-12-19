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
                case SocialPlatform.OK:
                    return BuildOdnoklassnikiShareLink(request.Url, request.Text);
                case SocialPlatform.Telegram:
                    return BuildTelegramShareLink(request.Url, request.Text);
                case SocialPlatform.WhatsApp:
                    return BuildWhatsAppShareLink(request.Url, request.Text);
                case SocialPlatform.Facebook:
                    return BuildFacebookShareLink(request.Url, request.Text);
                default:
                    throw new ArgumentOutOfRangeException(nameof(platform), platform, "Unsupported social platform");
            }
        }

        public void ShowShareDialog(ShareRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (!IsShareDialogAvailable())
                throw new InvalidOperationException("Native share dialog is not available on this platform.");

            _shareDialogAdapter.Show(request);
        }

        public bool IsShareDialogAvailable()
        {
            return _shareDialogAdapter.IsAvailable;
        }

        private static string BuildVkShareLink(string url, string comment)
        {
            string encodedUrl = UnityWebRequest.EscapeURL(url ?? string.Empty);
            string encodedComment = UnityWebRequest.EscapeURL(comment ?? string.Empty);
            return $"https://vk.com/share.php?url={encodedUrl}&comment={encodedComment}";
        }

        private static string BuildOdnoklassnikiShareLink(string url, string title)
        {
            string encodedUrl = UnityWebRequest.EscapeURL(url ?? string.Empty);
            string encodedTitle = UnityWebRequest.EscapeURL(title ?? string.Empty);
            return $"https://connect.ok.ru/offer?url={encodedUrl}&title={encodedTitle}";
        }

        private static string BuildTelegramShareLink(string url, string text)
        {
            string encodedUrl = UnityWebRequest.EscapeURL(url ?? string.Empty);
            string encodedText = UnityWebRequest.EscapeURL(text ?? string.Empty);
            return $"https://t.me/share/url?url={encodedUrl}&text={encodedText}";
        }

        private static string BuildWhatsAppShareLink(string url, string text)
        {
            string combinedText = BuildCombinedShareText(url, text);
            string encodedText = UnityWebRequest.EscapeURL(combinedText);
            return $"https://api.whatsapp.com/send?text={encodedText}";
        }

        private static string BuildFacebookShareLink(string url, string quote)
        {
            string encodedUrl = UnityWebRequest.EscapeURL(url ?? string.Empty);
            string encodedQuote = UnityWebRequest.EscapeURL(quote ?? string.Empty);
            return $"https://www.facebook.com/sharer/sharer.php?u={encodedUrl}&quote={encodedQuote}";
        }

        private static string BuildCombinedShareText(string url, string text)
        {
            if (string.IsNullOrEmpty(url))
                return text ?? string.Empty;
            if (string.IsNullOrEmpty(text))
                return url;

            return $"{text} {url}";
        }
    }
}
