using System;

namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules.RemoteConfig
{
    public interface IRemoteConfigLoader
    {
        void Load(Action<bool> onCompleted);
    }
}
