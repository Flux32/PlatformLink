#if UNITY_EDITOR
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Environment;
using UnityEngine;
using DeviceType = RetroCat.PlatformLink.Runtime.Source.Common.Modules.Environment.DeviceType;

namespace PlatformLink.Platform.UnityEditor
{
    public class EditorEnvironment : IEnvironment
    {
        public string Language { get; private set; }
        public DeviceType DeviceType { get; private set; }
        public string AppURL => Application.absoluteURL;

        public EditorEnvironment(string language, DeviceType deviceType)
        {
            DeviceType = deviceType;
            Language = language;
        }
    }
}
#endif
