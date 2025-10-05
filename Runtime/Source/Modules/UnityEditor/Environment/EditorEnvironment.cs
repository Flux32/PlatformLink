#if UNITY_EDITOR
using PlatformLink.Common;

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