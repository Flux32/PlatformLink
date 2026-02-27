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
        public string AppId => "";
        public string AppUrl => Application.absoluteURL;

        public EditorEnvironment(EnvironmentEditorSettings settings)
        {
            DeviceType = settings.DeviceType;
            Language = settings.Language;
        }
    }
}
#endif
