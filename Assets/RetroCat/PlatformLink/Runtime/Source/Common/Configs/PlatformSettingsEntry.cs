using System;
using UnityEngine;

public enum PlatformSettingsType
{
    Editor = 0,
    Android = 1,
    Ios = 2,
    YandexGames = 3
}

[Serializable]
public class PlatformSettingsEntry
{
    [SerializeField] private string _name;
    [SerializeField] private PlatformSettingsType _type;
    [SerializeField] private EditorSettings _editorSettings = new EditorSettings();
    [SerializeField] private AndroidSettings _androidSettings = new AndroidSettings();
    [SerializeField] private IosSettings _iosSettings = new IosSettings();
    [SerializeField] private YandexSettings _yandexSettings = new YandexSettings();

    public string Name => _name;
    public PlatformSettingsType Type => _type;
    public EditorSettings EditorSettings => _editorSettings;
    public AndroidSettings AndroidSettings => _androidSettings;
    public IosSettings IosSettings => _iosSettings;
    public YandexSettings YandexSettings => _yandexSettings;

    public PlatformSettingsEntry()
    {
    }

    public PlatformSettingsEntry(
        PlatformSettingsType type,
        string name,
        EditorSettings editorSettings = null,
        AndroidSettings androidSettings = null,
        IosSettings iosSettings = null,
        YandexSettings yandexSettings = null)
    {
        _type = type;
        _name = name;
        _editorSettings = editorSettings ?? new EditorSettings();
        _androidSettings = androidSettings ?? new AndroidSettings();
        _iosSettings = iosSettings ?? new IosSettings();
        _yandexSettings = yandexSettings ?? new YandexSettings();
    }

    public PlatformSettingsEntry Clone()
    {
        string json = JsonUtility.ToJson(this);
        return JsonUtility.FromJson<PlatformSettingsEntry>(json);
    }

    public void Rename(string name)
    {
        _name = name;
    }
}
