namespace PlatformLink.Common
{
    public interface IEnvironment
    {
        DeviceType DeviceType { get; }
        string Language { get; }
    }
}