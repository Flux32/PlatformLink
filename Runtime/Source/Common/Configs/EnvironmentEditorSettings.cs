using System;
using UnityEngine;
using DeviceType = PlatformLink.DeviceType;

[Serializable]
public class EnvironmentEditorSettings
{
    public string Language => _language;
    public DeviceType DeviceType => _deviceType;

    [SerializeField] private string _language;
    [SerializeField] private DeviceType _deviceType;
}
