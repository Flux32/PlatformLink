using System;
using System.Runtime.InteropServices;
using UnityEngine;
using PlatformLink.Common;

namespace PlatformLink.Platform.YandexGames
{
    public class YandexStorage : MonoBehaviour, IStorage
    {
        private Action<bool> DataSaveCompleted;
        private Action<bool, string> DataLoadCompleted;

        [DllImport("__Internal")]
        private static extern void jslib_loadFromPlatform(string key);

        public void Load(string key, Action<bool, string> onCompleted)
        {
            DataLoadCompleted = onCompleted;
            jslib_loadFromPlatform(key);
        }

        [DllImport("__Internal")]
        private static extern void jslib_saveToPlatform(string key, string data);

        public void Save(string key, string data, Action<bool> onCompleted = null)
        {
            DataSaveCompleted = onCompleted;
            jslib_saveToPlatform(key, data);
        }

        #region Called from PlatformLink.js
        private void fjs_onSaveDataSuccess()
        {
            DataSaveCompleted?.Invoke(true);
        }

        private void fjs_onLoadDataSuccess(string data)
        {
            DataLoadCompleted?.Invoke(true, data);
        }
        #endregion
    }
}