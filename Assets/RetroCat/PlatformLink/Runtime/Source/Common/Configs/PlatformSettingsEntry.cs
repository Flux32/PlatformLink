using System;
using System.Collections.Generic;
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
    [SerializeField] private List<PlatformModuleEntry> _modules = new List<PlatformModuleEntry>();

    public string Name => _name;
    public PlatformSettingsType Type => _type;
    public EditorSettings EditorSettings => _editorSettings;
    public AndroidSettings AndroidSettings => _androidSettings;
    public IosSettings IosSettings => _iosSettings;
    public YandexSettings YandexSettings => _yandexSettings;
    public IReadOnlyList<PlatformModuleEntry> Modules => _modules;

    public PlatformModuleEntry AddModule(PlatformModuleKind kind)
    {
        PlatformModuleEntry entry = new PlatformModuleEntry(kind);
        _modules.Add(entry);
        return entry;
    }

    public void RemoveModuleAt(int index)
    {
        if (index < 0 || index >= _modules.Count)
            return;

        _modules.RemoveAt(index);
    }

    public bool HasModuleOfKind(PlatformModuleKind kind)
    {
        for (int i = 0; i < _modules.Count; i++)
        {
            if (_modules[i].Kind == kind)
                return true;
        }

        return false;
    }

    public IModuleAdapterConfig FindAdapterConfig(PlatformModuleKind kind)
    {
        for (int i = 0; i < _modules.Count; i++)
        {
            if (_modules[i].Kind == kind)
                return _modules[i].AdapterConfig;
        }

        return null;
    }

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
        PlatformSettingsEntry clone = JsonUtility.FromJson<PlatformSettingsEntry>(json);
        clone._modules = CloneModules();
        return clone;
    }

    private List<PlatformModuleEntry> CloneModules()
    {
        List<PlatformModuleEntry> cloned = new List<PlatformModuleEntry>(_modules.Count);
        for (int i = 0; i < _modules.Count; i++)
        {
            PlatformModuleEntry source = _modules[i];
            PlatformModuleEntry copy = new PlatformModuleEntry(source.Kind);

            IModuleAdapterConfig sourceConfig = source.AdapterConfig;
            if (sourceConfig != null)
            {
                string adapterJson = JsonUtility.ToJson(sourceConfig);
                IModuleAdapterConfig clonedConfig = (IModuleAdapterConfig)JsonUtility.FromJson(adapterJson, sourceConfig.GetType());
                copy.SetAdapter(clonedConfig);
            }

            cloned.Add(copy);
        }

        return cloned;
    }

    public void Rename(string name)
    {
        _name = name;
    }
}
