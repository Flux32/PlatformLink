using System;
using System.Collections.Generic;
using UnityEngine;

public class PlatformLinkSettings : ScriptableObject
{
    [SerializeField] private List<PlatformSettingsEntry> _platforms = new List<PlatformSettingsEntry>();
    [SerializeField, HideInInspector] private int _activePlatformIndex = -1;
    [SerializeField, HideInInspector] private EditorSettings _editor = new EditorSettings();
    [SerializeField, HideInInspector] private AndroidSettings _android = new AndroidSettings();
    [SerializeField, HideInInspector] private IosSettings _ios = new IosSettings();
    [SerializeField, HideInInspector] private YandexSettings _yandex = new YandexSettings();

    public int ActivePlatformIndex => _activePlatformIndex;

    public void SetActivePlatformIndex(int index)
    {
        _activePlatformIndex = index;
    }

    public PlatformSettingsEntry GetActivePlatformEntry()
    {
        EnsurePlatformEntries();

        if (_activePlatformIndex >= 0 && _activePlatformIndex < _platforms.Count)
            return _platforms[_activePlatformIndex];

        return null;
    }

    public IReadOnlyList<PlatformSettingsEntry> Platforms
    {
        get
        {
            EnsurePlatformEntries();
            return _platforms;
        }
    }
    public AndroidSettings Android => GetPrimaryAndroidSettings();
    public IosSettings Ios => GetPrimaryIosSettings();
    public EditorSettings Editor => GetPrimaryEditorSettings();
    public YandexSettings Yandex => GetPrimaryYandexSettings();
    public LeaderboardEditorSettings EditorLeaderboard => Editor.Leaderboard;
    public PlatformEditorSettings EditorPlatform => Editor.Platform;

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
                    s_instance = Resources.Load<PlatformLinkSettings>(DefaultConfigPath);

                if (s_instance == null)
                    throw new InvalidOperationException("PlatformLinkSettings asset not found. Create it via Window/PlatformLink → Create Project Settings Asset.");
            }

            return s_instance;
        }
    }

    private void OnEnable()
    {
        EnsurePlatformEntries();
    }

    public int AddPlatform(PlatformSettingsType type)
    {
        EnsurePlatformEntries();

        if (type == PlatformSettingsType.Editor)
        {
            int editorIndex = FindFirstIndex(PlatformSettingsType.Editor);
            if (editorIndex >= 0)
                return editorIndex;
        }

        string uniqueName = GenerateUniqueName(GetDefaultPlatformName(type));
        _platforms.Add(new PlatformSettingsEntry(type, uniqueName));
        SyncLegacyReferences();
        return _platforms.Count - 1;
    }

    public int DuplicatePlatform(int index)
    {
        EnsurePlatformEntries();

        if (CanDuplicatePlatform(index) == false)
            return index;

        PlatformSettingsEntry source = _platforms[index];
        PlatformSettingsEntry duplicatedEntry = source.Clone();
        duplicatedEntry.Rename(GenerateUniqueName($"{source.Name} Copy"));
        _platforms.Insert(index + 1, duplicatedEntry);
        SyncLegacyReferences();
        return index + 1;
    }

    public bool RemovePlatform(int index)
    {
        EnsurePlatformEntries();

        if (CanRemovePlatform(index) == false)
            return false;

        _platforms.RemoveAt(index);
        SyncLegacyReferences();
        return true;
    }

    public void RenamePlatform(int index, string platformName)
    {
        EnsurePlatformEntries();

        if (IsValidIndex(index) == false)
            return;

        PlatformSettingsEntry entry = _platforms[index];
        string fallbackName = GetDefaultPlatformName(entry.Type);
        string name = NormalizeName(platformName, fallbackName);
        entry.Rename(name);
        SyncLegacyReferences();
    }

    public bool CanDuplicatePlatform(int index)
    {
        return IsValidIndex(index) && _platforms[index].Type != PlatformSettingsType.Editor;
    }

    public bool CanRemovePlatform(int index)
    {
        return IsValidIndex(index) && _platforms[index].Type != PlatformSettingsType.Editor;
    }

    public static string GetPlatformDisplayName(PlatformSettingsType type)
    {
        switch (type)
        {
            case PlatformSettingsType.Editor:
                return "Editor";
            case PlatformSettingsType.Android:
                return "Android";
            case PlatformSettingsType.Ios:
                return "iOS";
            case PlatformSettingsType.YandexGames:
                return "Yandex Games";
            default:
                return type.ToString();
        }
    }

    public static string GetPlatformTypeTag(PlatformSettingsType type)
    {
        switch (type)
        {
            case PlatformSettingsType.Editor:
                return "Editor";
            case PlatformSettingsType.Android:
                return "Android";
            case PlatformSettingsType.Ios:
                return "iOS";
            case PlatformSettingsType.YandexGames:
                return "YandexGames";
            default:
                return type.ToString();
        }
    }

    private void EnsurePlatformEntries()
    {
        if (_platforms == null)
            _platforms = new List<PlatformSettingsEntry>();

        if (_platforms.Count == 0)
        {
            _platforms.Add(new PlatformSettingsEntry(PlatformSettingsType.Editor, "Editor", _editor));
            _platforms.Add(new PlatformSettingsEntry(PlatformSettingsType.Android, "Android", androidSettings: _android));
            _platforms.Add(new PlatformSettingsEntry(PlatformSettingsType.YandexGames, "Yandex Games", yandexSettings: _yandex));
        }

        if (FindFirstIndex(PlatformSettingsType.Editor) < 0)
            _platforms.Insert(0, new PlatformSettingsEntry(PlatformSettingsType.Editor, "Editor", _editor));

        for (int i = 0; i < _platforms.Count; i++)
        {
            PlatformSettingsEntry entry = _platforms[i];
            string defaultName = GetDefaultPlatformName(entry.Type);
            string normalizedName = NormalizeName(entry.Name, defaultName);
            entry.Rename(normalizedName);
        }

        SyncLegacyReferences();
    }

    private void SyncLegacyReferences()
    {
        PlatformSettingsEntry editorEntry = FindFirstEntry(PlatformSettingsType.Editor);
        PlatformSettingsEntry androidEntry = FindFirstEntry(PlatformSettingsType.Android);
        PlatformSettingsEntry iosEntry = FindFirstEntry(PlatformSettingsType.Ios);
        PlatformSettingsEntry yandexEntry = FindFirstEntry(PlatformSettingsType.YandexGames);

        _editor = editorEntry != null ? editorEntry.EditorSettings : new EditorSettings();
        _android = androidEntry != null ? androidEntry.AndroidSettings : new AndroidSettings();
        _ios = iosEntry != null ? iosEntry.IosSettings : new IosSettings();
        _yandex = yandexEntry != null ? yandexEntry.YandexSettings : new YandexSettings();
    }

    private bool IsValidIndex(int index)
    {
        return index >= 0 && index < _platforms.Count;
    }

    private int FindFirstIndex(PlatformSettingsType type)
    {
        for (int i = 0; i < _platforms.Count; i++)
        {
            if (_platforms[i].Type == type)
                return i;
        }

        return -1;
    }

    private PlatformSettingsEntry FindFirstEntry(PlatformSettingsType type)
    {
        int index = FindFirstIndex(type);
        return index >= 0 ? _platforms[index] : null;
    }

    private string GenerateUniqueName(string baseName)
    {
        string normalizedBaseName = NormalizeName(baseName, "Platform");

        if (ContainsName(normalizedBaseName) == false)
            return normalizedBaseName;

        int suffix = 2;
        while (true)
        {
            string candidate = $"{normalizedBaseName} ({suffix})";
            if (ContainsName(candidate) == false)
                return candidate;

            suffix++;
        }
    }

    private bool ContainsName(string name)
    {
        for (int i = 0; i < _platforms.Count; i++)
        {
            if (string.Equals(_platforms[i].Name, name, StringComparison.OrdinalIgnoreCase))
                return true;
        }

        return false;
    }

    private static string GetDefaultPlatformName(PlatformSettingsType type)
    {
        return GetPlatformTypeTag(type);
    }

    private static string NormalizeName(string name, string fallback)
    {
        return string.IsNullOrWhiteSpace(name) ? fallback : name.Trim();
    }

    private EditorSettings GetPrimaryEditorSettings()
    {
        EnsurePlatformEntries();
        PlatformSettingsEntry entry = FindFirstEntry(PlatformSettingsType.Editor);
        return entry != null ? entry.EditorSettings : _editor;
    }

    private AndroidSettings GetPrimaryAndroidSettings()
    {
        EnsurePlatformEntries();
        PlatformSettingsEntry entry = FindFirstEntry(PlatformSettingsType.Android);
        return entry != null ? entry.AndroidSettings : _android;
    }

    private IosSettings GetPrimaryIosSettings()
    {
        EnsurePlatformEntries();
        PlatformSettingsEntry entry = FindFirstEntry(PlatformSettingsType.Ios);
        return entry != null ? entry.IosSettings : _ios;
    }

    private YandexSettings GetPrimaryYandexSettings()
    {
        EnsurePlatformEntries();
        PlatformSettingsEntry entry = FindFirstEntry(PlatformSettingsType.YandexGames);
        return entry != null ? entry.YandexSettings : _yandex;
    }
}
