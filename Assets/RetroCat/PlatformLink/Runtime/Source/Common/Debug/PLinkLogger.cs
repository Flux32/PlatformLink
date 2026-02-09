using PlatformLink.PluginDebug;

namespace RetroCat.PlatformLink.Runtime.Source.Common.Debug
{
    using Debug = UnityEngine.Debug;

    public class PLinkLogger : ILogger
    {
        private const string LogPrefix = "<color=#C88CFF>PlatformLink:</color>";

        private static string CreateMessage(object message)
        {
            return $"{LogPrefix} {message}";
        }

        public void LogError(object message)
        {
            Debug.LogError(CreateMessage(message));
        }

        public void Log(object message)
        {
            Debug.Log(CreateMessage(message));
        }

        public void LogWarning(object message)
        {
            Debug.LogWarning(CreateMessage(message));
        }
    }
}