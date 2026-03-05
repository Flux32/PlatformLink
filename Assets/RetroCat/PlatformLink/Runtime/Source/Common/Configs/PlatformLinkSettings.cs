using System;
using UnityEngine;

public class PlatformLinkSettings : ScriptableObject
{ 
    [SerializeField] private EditorSettings _editor = new EditorSettings();
    [SerializeField] private AndroidSettings _android = new AndroidSettings();
    [SerializeField] private YandexSettings _yandex = new YandexSettings();

    public AndroidSettings Android => _android;
    public EditorSettings Editor => _editor;
    public YandexSettings Yandex => _yandex;
    public LeaderboardEditorSettings EditorLeaderboard => _editor.Leaderboard;
    public PlatformEditorSettings EditorPlatform => _editor.Platform;

    private static PlatformLinkSettings s_instance;

    private const string DefaultConfigPath = "Configs/PlatformLinkConfig";
    private const string ProjectConfigPath = "ProjectConfigs/PlatformLinkConfig";

    public static PlatformLinkSettings Instance
    {
        get
        {
            if (s_instance == null)
            {
                s_instance = Resources.Load<PlatformLinkSettings>(ProjectConfigPath);
                if (s_instance == null)
                {
                    s_instance = Resources.Load<PlatformLinkSettings>(DefaultConfigPath);
                }

                if (s_instance == null)
                    throw new InvalidOperationException("PlatformLinkSettings asset not found. Create it via Window/PlatformLink → Create Project Settings Asset.");
            }

            return s_instance;
        }
    }
}
