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
        private const string MetrikaCounterVariableName = "__platformLinkYandexMetrikaCounterId";
        public const string YandexMetrikaScriptFileName = "YandexMetrika.js";

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

        public static string InjectYandexMetrikaScriptReference(string html, string scriptPath)
        {
            if (string.IsNullOrEmpty(html))
            {
                return html;
            }

            if (string.IsNullOrWhiteSpace(scriptPath))
            {
                return html;
            }

            string normalizedScriptPath = scriptPath.Trim().Replace("\\", "/");
            string snippet = BuildYandexMetrikaScriptReferenceHtml(normalizedScriptPath);

            int startIndex = html.IndexOf(MetrikaStartMarker, System.StringComparison.Ordinal);
            int endIndex = html.IndexOf(MetrikaEndMarker, System.StringComparison.Ordinal);

            if (startIndex >= 0 && endIndex > startIndex)
            {
                int removeLength = endIndex + MetrikaEndMarker.Length - startIndex;
                return html.Remove(startIndex, removeLength).Insert(startIndex, snippet);
            }

            if (html.Contains(normalizedScriptPath, System.StringComparison.OrdinalIgnoreCase))
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

        public static string BuildYandexMetrikaScriptReferenceHtml(string scriptPath)
        {
            return
$@"{MetrikaStartMarker}
<script type=""text/javascript"" src=""{scriptPath}""></script>
{MetrikaEndMarker}";
        }

        public static string BuildYandexMetrikaScript(string counterId)
        {
            return
$@"(function(windowObject, documentObject) {{
    const counterId = {counterId};
    windowObject.{MetrikaCounterVariableName} = counterId;

    (function(m,e,t,r,i,k,a){{
        m[i]=m[i]||function(){{(m[i].a=m[i].a||[]).push(arguments)}};
        m[i].l=1*new Date();
        for (var j = 0; j < documentObject.scripts.length; j++) {{if (documentObject.scripts[j].src === r) {{ return; }}}}
        k=e.createElement(t),a=e.getElementsByTagName(t)[0],k.async=1,k.src=r,a.parentNode.insertBefore(k,a)
    }})(windowObject, documentObject,'script','https://mc.yandex.ru/metrika/tag.js?id=' + counterId, 'ym');

    windowObject.ym(counterId, 'init', {{ssr:true, clickmap:true, referrer: documentObject.referrer, url: windowObject.location.href, accurateTrackBounce:true, trackLinks:true}});
}})(window, document);
";
        }
    }
}
