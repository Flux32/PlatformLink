#if UNITY_EDITOR
using System;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Platform;
using UnityEngine;
using ILogger = PlatformLink.PluginDebug.ILogger;

namespace PlatformLink.Platform.UnityEditor
{
    public class EditorPlatform : IPlatform
    {
        public bool Authorized { get; private set; }

        private readonly ILogger _logger;
        private static readonly AvailableGame[] DefaultGames =
        {
            new AvailableGame("puzzle", "Puzzle", string.Empty, string.Empty, string.Empty),
            new AvailableGame("match3", "Match 3", string.Empty, string.Empty, string.Empty),
            new AvailableGame("race", "Race", string.Empty, string.Empty, string.Empty),
            new AvailableGame("zombie", "Zombie", string.Empty, string.Empty, string.Empty),
            new AvailableGame("card", "Card", string.Empty, string.Empty, string.Empty),
        };

        public EditorPlatform(ILogger logger, PlatformEditorSettings settings)
        {
            _logger = logger;
            Authorized = settings.Authorized;
        }

        public void OpenLink(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                _logger.LogWarning("editor platform open link failed. Url is empty.");
                return;
            }

            Application.OpenURL(url);
        }

        public void Authorize(Action<bool> onCompleted)
        {
            Authorized = true;
            _logger?.Log("Editor platform authorized.");
            onCompleted?.Invoke(true);
        }

        public void GetAllGames(Action<bool, AvailableGames> onCompleted)
        {
            try
            {
                var settings = PlatformLinkSettings.Instance.Editor?.PlatformGames;
                var developerUrl = settings?.DeveloperUrl ?? string.Empty;

                var configuredGames = settings?.Games;
                if (configuredGames == null || configuredGames.Length == 0)
                {
                    onCompleted?.Invoke(true, new AvailableGames(DefaultGames, developerUrl));
                    return;
                }

                var games = new AvailableGame[configuredGames.Length];
                for (int i = 0; i < configuredGames.Length; i++)
                {
                    var g = configuredGames[i];
                    games[i] = new AvailableGame(
                        g?.AppId ?? string.Empty,
                        g?.Title ?? string.Empty,
                        g?.Url ?? string.Empty,
                        g?.CoverUrl ?? string.Empty,
                        g?.IconUrl ?? string.Empty);
                }

                onCompleted?.Invoke(true, new AvailableGames(games, developerUrl));
            }
            catch (Exception exception)
            {
                _logger?.Log($"Editor platform GetAllGames failed, using defaults. {exception.Message}");
                onCompleted?.Invoke(true, new AvailableGames(DefaultGames, string.Empty));
            }
        }
    }
}
#endif
