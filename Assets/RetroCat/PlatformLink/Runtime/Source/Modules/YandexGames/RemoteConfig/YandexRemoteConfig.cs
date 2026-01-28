using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using PlatformLink.Lifecycle;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.RemoteConfig;
using UnityEngine;

namespace RetroCat.PlatformLink.Runtime.Source.Modules.YandexGames.RemoteConfig
{
    public class YandexRemoteConfig : MonoBehaviour, IRemoteConfig, IInitializable, IRemoteConfigLoader
    {
        private const string LogPrefix = "YandexRemoteConfig: ";

        private readonly Dictionary<string, string> _flags = new Dictionary<string, string>(StringComparer.Ordinal);
        private Action<bool> _loadCompleted;
        private bool _isLoaded;
        private bool _isLoading;

        [DllImport("__Internal")]
        private static extern void jslib_loadRemoteConfig();

        public void Initialize()
        {
            Load(null);
        }

        public void Load(Action<bool> onCompleted)
        {
            if (_isLoaded)
            {
                onCompleted?.Invoke(true);
                return;
            }

            if (onCompleted != null)
            {
                _loadCompleted += onCompleted;
            }

            if (_isLoading)
                return;

            _isLoading = true;
            jslib_loadRemoteConfig();
        }

        public T GetRemoteConfig<T>(string key, T fallbackValue)
        {
            if (!_isLoaded)
            {
                Debug.LogError($"{LogPrefix}Remote config is not loaded. Key: {key ?? "<null>"}");
                return fallbackValue;
            }

            if (string.IsNullOrWhiteSpace(key))
            {
                Debug.LogError($"{LogPrefix}Key is null or empty.");
                return fallbackValue;
            }

            if (!_flags.TryGetValue(key, out string rawValue) || rawValue == null)
            {
                Debug.LogError($"{LogPrefix}Key '{key}' not found or value is null. Using fallback.");
                return fallbackValue;
            }

            if (TryParse(rawValue, out T parsed))
                return parsed;

            Debug.LogError($"{LogPrefix}Failed to parse key '{key}' value '{rawValue}' as {typeof(T).Name}. Using fallback.");
            return fallbackValue;
        }

        private static bool TryParse<T>(string rawValue, out T result)
        {
            result = default;

            if (rawValue == null)
                return false;

            Type targetType = typeof(T);

            if (targetType == typeof(string))
            {
                result = (T)(object)rawValue;
                return true;
            }

            if (targetType == typeof(int))
            {
                if (int.TryParse(rawValue, NumberStyles.Integer, CultureInfo.InvariantCulture, out int intValue))
                {
                    result = (T)(object)intValue;
                    return true;
                }

                return false;
            }

            if (targetType == typeof(bool))
            {
                if (bool.TryParse(rawValue, out bool boolValue))
                {
                    result = (T)(object)boolValue;
                    return true;
                }

                string normalized = rawValue.Trim().ToLowerInvariant();
                if (normalized == "1" || normalized == "yes" || normalized == "y")
                {
                    result = (T)(object)true;
                    return true;
                }

                if (normalized == "0" || normalized == "no" || normalized == "n")
                {
                    result = (T)(object)false;
                    return true;
                }

                return false;
            }

            if (targetType == typeof(float))
            {
                if (float.TryParse(rawValue, NumberStyles.Float, CultureInfo.InvariantCulture, out float floatValue))
                {
                    result = (T)(object)floatValue;
                    return true;
                }

                return false;
            }

            if (targetType == typeof(double))
            {
                if (double.TryParse(rawValue, NumberStyles.Float, CultureInfo.InvariantCulture, out double doubleValue))
                {
                    result = (T)(object)doubleValue;
                    return true;
                }

                return false;
            }

            return false;
        }

        #region Called from PlatformLink.js
        private void fjs_onRemoteConfigLoaded(string json)
        {
            _flags.Clear();

            if (string.IsNullOrEmpty(json))
            {
                Debug.LogError($"{LogPrefix}Remote config payload is empty.");
                CompleteLoad(false);
                return;
            }

            try
            {
                FlagsWrapper wrapper = JsonUtility.FromJson<FlagsWrapper>(json);
                if (wrapper == null)
                {
                    Debug.LogError($"{LogPrefix}Remote config payload is invalid.");
                    CompleteLoad(false);
                    return;
                }

                if (wrapper.items != null)
                {
                    foreach (FlagItem item in wrapper.items)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.key))
                            continue;

                        _flags[item.key] = item.value;
                    }
                }

                CompleteLoad(true);
            }
            catch (Exception exception)
            {
                Debug.LogError($"{LogPrefix}Failed to parse remote config payload. {exception.Message}");
                CompleteLoad(false);
            }
        }

        private void fjs_onRemoteConfigFailed()
        {
            Debug.LogError($"{LogPrefix}Failed to load remote config.");
            _flags.Clear();
            CompleteLoad(false);
        }
        #endregion

        private void CompleteLoad(bool success)
        {
            _isLoading = false;
            _isLoaded = success;
            _loadCompleted?.Invoke(success);
            _loadCompleted = null;
        }

        [Serializable]
        private class FlagsWrapper
        {
            public FlagItem[] items;
        }

        [Serializable]
        private class FlagItem
        {
            public string key;
            public string value;
        }
    }
}
