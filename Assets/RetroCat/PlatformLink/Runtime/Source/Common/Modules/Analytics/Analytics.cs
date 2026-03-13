using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using PlatformLink.PluginDebug;

namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules.Analytics
{
    public class Analytics : IAnalytics
    {
        private const string MetrikaStartMarker = "<!-- Yandex.Metrika counter -->";
        private const string MetrikaEndMarker = "<!-- /Yandex.Metrika counter -->";
        private const string MetrikaTagUrl = "mc.yandex.ru/metrika/tag.js";

        private readonly ILogger _logger;
        private readonly IAnalyticsAdapter[] _analyticsAdapters;
        
        private bool _isGameReadySent;
        
        public Analytics(ILogger logger, IEnumerable<IAnalyticsAdapter> analyticsAdapters)
        {
            _logger = logger;
            _analyticsAdapters = analyticsAdapters.ToArray();
        }
        
        public void SendGameReady()
        {
            if (_isGameReadySent)
            {
                _logger.LogError("game ready had already been sent.");
                return;
            }
                
            _isGameReadySent = true;
            foreach (IAnalyticsAdapter analyticsAdapter in _analyticsAdapters)
            {
                analyticsAdapter.SendGameReady();
            }
        }

        public void SendEvent(string eventName)
        {
            if (string.IsNullOrWhiteSpace(eventName))
            {
                _logger.LogError("event name is empty.");
                return;
            }

            string normalizedEventName = eventName.Trim();
            foreach (IAnalyticsAdapter analyticsAdapter in _analyticsAdapters)
            {
                analyticsAdapter.SendEvent(normalizedEventName);
            }
        }

        public void SendEvent(string eventName, string eventDataJson)
        {
            if (string.IsNullOrWhiteSpace(eventDataJson))
            {
                SendEvent(eventName);
                return;
            }

            if (string.IsNullOrWhiteSpace(eventName))
            {
                _logger.LogError("event name is empty.");
                return;
            }

            string normalizedEventName = eventName.Trim();
            string normalizedEventDataJson = eventDataJson.Trim();
            
            foreach (IAnalyticsAdapter analyticsAdapter in _analyticsAdapters)
            {
                analyticsAdapter.SendEvent(normalizedEventName, normalizedEventDataJson);
            }
        }

        public static bool TryNormalizeYandexMetrikaId(string rawCounterId, out string counterId)
        {
            counterId = string.Empty;
            if (string.IsNullOrWhiteSpace(rawCounterId))
            {
                return false;
            }

            string trimmed = rawCounterId.Trim();
            if (trimmed.All(char.IsDigit) == false)
            {
                return false;
            }

            counterId = trimmed;
            return true;
        }

        public static string InjectYandexMetrikaCounter(string html, string counterId)
        {
            if (string.IsNullOrEmpty(html))
            {
                return html;
            }

            string snippet = BuildYandexMetrikaCounterHtml(counterId);

            int startIndex = html.IndexOf(MetrikaStartMarker, System.StringComparison.Ordinal);
            int endIndex = html.IndexOf(MetrikaEndMarker, System.StringComparison.Ordinal);
            
            if (startIndex >= 0 && endIndex > startIndex)
            {
                int removeLength = endIndex + MetrikaEndMarker.Length - startIndex;
                return html.Remove(startIndex, removeLength).Insert(startIndex, snippet);
            }

            if (html.Contains(MetrikaTagUrl, System.StringComparison.Ordinal))
            {
                return html;
            }

            Match headMatch = Regex.Match(html, "<head\\b[^>]*>", RegexOptions.IgnoreCase);
            
            if (headMatch.Success)
            {
                int insertIndex = headMatch.Index + headMatch.Length;
                return html.Insert(insertIndex, "\n" + snippet + "\n");
            }

            Match bodyMatch = Regex.Match(html, "<body\\b[^>]*>", RegexOptions.IgnoreCase);
            
            if (bodyMatch.Success)
            {
                int insertIndex = bodyMatch.Index + bodyMatch.Length;
                return html.Insert(insertIndex, "\n" + snippet + "\n");
            }

            return snippet + "\n" + html;
        }

        //TODO: reflex or another module
        public static string BuildYandexMetrikaCounterHtml(string counterId)
        {
            return
$@"{MetrikaStartMarker}
<script type=""text/javascript"">
    window.__platformLinkMetrikaCounterId = {counterId};

    (function(m,e,t,r,i,k,a){{
        m[i]=m[i]||function(){{(m[i].a=m[i].a||[]).push(arguments)}};
        m[i].l=1*new Date();
        for (var j = 0; j < document.scripts.length; j++) {{if (document.scripts[j].src === r) {{ return; }}}}
        k=e.createElement(t),a=e.getElementsByTagName(t)[0],k.async=1,k.src=r,a.parentNode.insertBefore(k,a)
    }})(window, document,'script','https://mc.yandex.ru/metrika/tag.js?id={counterId}', 'ym');

    ym({counterId}, 'init', {{ssr:true, webvisor:false, clickmap:true, ecommerce:""dataLayer"", referrer: document.referrer, url: location.href, accurateTrackBounce:true, trackLinks:true}});
</script>
<noscript><div><img src=""https://mc.yandex.ru/watch/{counterId}"" style=""position:absolute; left:-9999px;"" alt="""" /></div></noscript>
{MetrikaEndMarker}";
        }
    }
}
