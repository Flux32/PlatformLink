namespace PlatformLink.PluginDebug
{
    public interface ILogger
    {
        public void Log(object message);
        public void LogError(object message);
        public void LogWarning(object message);
    }
}