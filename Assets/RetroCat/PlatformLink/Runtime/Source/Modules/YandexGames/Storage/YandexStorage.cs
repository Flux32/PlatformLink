using System;
using System.Runtime.InteropServices;
using System.Globalization;
using UnityEngine;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Storage;

namespace PlatformLink.Platform.YandexGames
{
    public class YandexStorage : MonoBehaviour, IStorage
    {
        private Action<bool> _saveCompleted;
        private Action<bool, string> _loadCompleted;

        [DllImport("__Internal")]
        private static extern void jslib_loadFromPlatform(string key);

        [DllImport("__Internal")]
        private static extern void jslib_saveToPlatform(string key, string data);

        public void SaveString(string key, string data, Action<bool> onCompleted = null)
        {
            SaveInternal(key, data, onCompleted);
        }

        public void SaveInt(string key, int data, Action<bool> onCompleted = null)
        {
            SaveInternal(key, data.ToString(CultureInfo.InvariantCulture), onCompleted);
        }

        public void SaveBool(string key, bool data, Action<bool> onCompleted = null)
        {
            SaveInternal(key, data.ToString(), onCompleted);
        }

        public void SaveFloat(string key, float data, Action<bool> onCompleted = null)
        {
            SaveInternal(key, data.ToString(CultureInfo.InvariantCulture), onCompleted);
        }

        public void LoadString(string key, Action<bool, string> onCompleted)
        {
            LoadInternal(key, onCompleted);
        }

        public void LoadInt(string key, Action<bool, int> onCompleted)
        {
            LoadInternal(key, (success, rawData) =>
            {
                if (success == false)
                {
                    onCompleted?.Invoke(false, default);
                    return;
                }

                if (int.TryParse(rawData, NumberStyles.Integer, CultureInfo.InvariantCulture, out int result))
                    onCompleted?.Invoke(true, result);
                else
                    onCompleted?.Invoke(false, default);
            });
        }

        public void LoadBool(string key, Action<bool, bool> onCompleted)
        {
            LoadInternal(key, (success, rawData) =>
            {
                if (success == false)
                {
                    onCompleted?.Invoke(false, default);
                    return;
                }

                if (bool.TryParse(rawData, out bool result))
                    onCompleted?.Invoke(true, result);
                else
                    onCompleted?.Invoke(false, default);
            });
        }

        public void LoadFloat(string key, Action<bool, float> onCompleted)
        {
            LoadInternal(key, (success, rawData) =>
            {
                if (success == false)
                {
                    onCompleted?.Invoke(false, default);
                    return;
                }

                if (float.TryParse(rawData, NumberStyles.Float, CultureInfo.InvariantCulture, out float result))
                    onCompleted?.Invoke(true, result);
                else
                    onCompleted?.Invoke(false, default);
            });
        }

        private void SaveInternal(string key, string data, Action<bool> onCompleted)
        {
            _saveCompleted = onCompleted;
            jslib_saveToPlatform(key, data);
        }

        private void LoadInternal(string key, Action<bool, string> onCompleted)
        {
            _loadCompleted = onCompleted;
            jslib_loadFromPlatform(key);
        }

        #region Called from PlatformLink.js
        private void fjs_onSaveDataSuccess()
        {
            _saveCompleted?.Invoke(true);
        }

        private void fjs_onLoadDataSuccess(string data)
        {
            _loadCompleted?.Invoke(true, data);
        }
        #endregion
    }
}
