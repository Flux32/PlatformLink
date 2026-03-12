public interface IAnalytics
{
    public void SendGameReady();
    public void SendEvent(string eventName);
    public void SendEvent(string eventName, string eventDataJson);
}
