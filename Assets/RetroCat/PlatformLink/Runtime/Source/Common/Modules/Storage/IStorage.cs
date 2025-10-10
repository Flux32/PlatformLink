using System;

namespace PlatformLink.Common
{
    public interface IStorage
    {
        void Save(string key, string data, Action<bool> onCompleted = null);
        public void Load(string key, Action<bool, string> onCompleted);
    }
}