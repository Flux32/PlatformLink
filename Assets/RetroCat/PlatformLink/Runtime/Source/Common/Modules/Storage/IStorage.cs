using System;

namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules.Storage
{
    public interface IStorage
    {
        void SaveString(string key, string data, Action<bool> onCompleted = null);
        void SaveInt(string key, int data, Action<bool> onCompleted = null);
        void SaveBool(string key, bool data, Action<bool> onCompleted = null);
        void SaveFloat(string key, float data, Action<bool> onCompleted = null);

        void LoadString(string key, Action<bool, string> onCompleted);
        void LoadInt(string key, Action<bool, int> onCompleted);
        void LoadBool(string key, Action<bool, bool> onCompleted);
        void LoadFloat(string key, Action<bool, float> onCompleted);
    }
}
