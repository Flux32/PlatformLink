namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules.Analytics
{
    public interface IAnalyticsAdapter
    {
        public void SendGameReady();
        public void SendEvent(string eventName);
        public void SendEvent(string eventName, string eventDataJson);
    }
}
