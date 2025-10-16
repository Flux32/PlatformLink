#if UNITY_EDITOR
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Environment;

namespace PlatformLink.Platform.UnityEditor
{
    public class EditorEnvironment : IEnvironment
    {
        public string Language { get; private set; }
        public DeviceType DeviceType { get; private set; }

        public EditorEnvironment(string language, DeviceType deviceType)
        {
            DeviceType = deviceType;
            Language = language;
        }
    }
}
#endif