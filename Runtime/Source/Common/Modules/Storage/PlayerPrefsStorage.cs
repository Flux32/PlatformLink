using System;
using UnityEngine;
using PlatformLink.Common;

public class PlayerPrefsStorage : IStorage
{
    public void Load(string key, Action<bool, string> onCompleted)
    {
        onCompleted?.Invoke(true, PlayerPrefs.GetString(key, null));
    }

    public void Save(string key, string data, Action<bool> onCompleted = null)
    {
        PlayerPrefs.SetString(key, data);
        onCompleted?.Invoke(true);
    }
}