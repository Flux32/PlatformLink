using System.Collections.Generic;
using UnityEngine;
using SystemDeviceType = UnityEngine.DeviceType;

namespace PlatformLink.Common
{
    public class DefaultEnvironment : IEnvironment
    {
        private readonly Dictionary<SystemDeviceType, DeviceType> _deviceTypeMapping = new Dictionary<SystemDeviceType, DeviceType>()
        {
            { SystemDeviceType.Desktop, DeviceType.Desktop },
            { SystemDeviceType.Handheld, DeviceType.Mobile },
            { SystemDeviceType.Console, DeviceType.Desktop },
            { SystemDeviceType.Unknown, DeviceType.Desktop },
        };

        private readonly Dictionary<SystemLanguage, string> _languageMapping = new Dictionary<SystemLanguage, string>()
        {
            { SystemLanguage.Russian, "ru"},
            { SystemLanguage.English, "en" },
        };
        
        public DeviceType DeviceType => _deviceTypeMapping[SystemInfo.deviceType];
        public string Language => _languageMapping[Application.systemLanguage];
    }
}
