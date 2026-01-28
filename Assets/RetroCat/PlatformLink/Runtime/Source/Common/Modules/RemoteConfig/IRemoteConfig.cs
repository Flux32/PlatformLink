namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules.RemoteConfig
{
    public interface IRemoteConfig
    {
        T GetRemoteConfig<T>(string key, T fallbackValue);
    }
}
