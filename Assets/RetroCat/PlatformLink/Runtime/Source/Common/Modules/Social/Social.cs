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
            string normalizedText = NormalizeText(request.Text);
            
            switch (platform)
            {
                case SocialPlatform.VK:
                    return BuildVkShareLink(request.Url, normalizedText);
                case SocialPlatform.OK:
                    return BuildOdnoklassnikiShareLink(request.Url, normalizedText);
                case SocialPlatform.Telegram:
                    return BuildTelegramShareLink(request.Url, normalizedText);
                case SocialPlatform.WhatsApp:
                    return BuildWhatsAppShareLink(request.Url, normalizedText);
                case SocialPlatform.Facebook:
                    return BuildFacebookShareLink(request.Url, normalizedText);
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

        private string NormalizeText(string s)
        {
            if (string.IsNullOrEmpty(s)) 
                return string.Empty;

            s = s.Replace("\r\n", "\n").Replace("\r", "\n");
            return s.Replace("\n", "\r\n");
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
