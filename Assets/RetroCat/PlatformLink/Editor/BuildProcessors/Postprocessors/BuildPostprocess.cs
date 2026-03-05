using UnityEditor;
using UnityEditor.Callbacks;
using PlatformLink.PluginDebug;
using RetroCat.PlatformLink.Runtime.Source.Common.Debug;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Analytics;
using System.IO;

public class BuildPostprocess
{
    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        if (target == BuildTarget.WebGL)
        {
            ILogger logger = new PLinkLogger();

            TryInjectYandexMetrika(pathToBuiltProject, logger);

            BuildZipArchiving zipPacker = new BuildZipArchiving(logger);
            zipPacker.Archive(pathToBuiltProject);
        }
    }

    private static void TryInjectYandexMetrika(string buildPath, ILogger logger)
    {
        YandexSettings yandexSettings = PlatformLinkSettings.Instance.Yandex;
        if (yandexSettings == null || yandexSettings.EnableYandexMetrika == false)
        {
            return;
        }

        if (Analytics.TryNormalizeYandexMetrikaId(yandexSettings.YandexMetrikaId, out string counterId) == false)
        {
            logger.LogWarning("Yandex Metrika is enabled, but Counter ID is empty or invalid. Injection skipped.");
            return;
        }

        string[] htmlPaths = Directory.GetFiles(buildPath, "*.html", SearchOption.AllDirectories);
        if (htmlPaths.Length == 0)
        {
            logger.LogWarning("No HTML files found in WebGL build output. Yandex Metrika injection skipped.");
            return;
        }

        int injectedFiles = 0;
        foreach (string htmlPath in htmlPaths)
        {
            try
            {
                string html = File.ReadAllText(htmlPath);
                string updatedHtml = Analytics.InjectYandexMetrikaCounter(html, counterId);
                if (updatedHtml == html)
                {
                    continue;
                }

                File.WriteAllText(htmlPath, updatedHtml);
                injectedFiles++;
            }
            catch (System.Exception exception)
            {
                logger.LogWarning($"Failed to update '{htmlPath}'. {exception.Message}");
            }
        }

        logger.Log($"Yandex Metrika counter injected into {injectedFiles}/{htmlPaths.Length} HTML file(s). Counter ID: {counterId}");
    }
}
