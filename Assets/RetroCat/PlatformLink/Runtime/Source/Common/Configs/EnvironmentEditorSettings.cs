using System;
using UnityEngine;
using DeviceType = RetroCat.PlatformLink.Runtime.Source.Common.Modules.Environment.DeviceType;

[Serializable]
public class EnvironmentEditorSettings
{
    [SerializeField] private string _language = "En";
    [SerializeField] private DeviceType _deviceType;
    
    public string Language => _language;
    public DeviceType DeviceType => _deviceType;
}