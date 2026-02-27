#if UNITY_EDITOR
using System;
using System.IO;
using System.Globalization;
using UnityEngine;
using UnityEditor;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Storage;
using ILogger = PlatformLink.PluginDebug.ILogger;

namespace PlatformLink.Platform.UnityEditor
{
    public class EditorStorage : IStorage
    {
        private const string DirectoryErrorMessage = "unable to save file. Directory {0} not found.";
        private const string FileExtension = ".txt";
        private const string SavedMessage = "data saved in editor mode";
        private const string LoadedMessage = "data loaded in editor mode";

        private readonly string _saveDirectory;
        
        private readonly ILogger _logger;

        public EditorStorage(ILogger logger, string saveDirectory)
        {
            _logger = logger;
            _saveDirectory = saveDirectory;
        }

        public void SaveString(string key, string data, Action<bool> onCompleted = null)
        {
            SaveInternalAsync(key, data, onCompleted);
        }

        public void SaveInt(string key, int data, Action<bool> onCompleted = null)
        {
            SaveInternalAsync(key, data.ToString(CultureInfo.InvariantCulture), onCompleted);
        }

        public void SaveBool(string key, bool data, Action<bool> onCompleted = null)
        {
            SaveInternalAsync(key, data.ToString(), onCompleted);
        }

        public void SaveFloat(string key, float data, Action<bool> onCompleted = null)
        {
            SaveInternalAsync(key, data.ToString(CultureInfo.InvariantCulture), onCompleted);
        }

        public void LoadString(string key, Action<bool, string> onCompleted)
        {
            LoadInternalAsync(key, onCompleted);
        }

        public void LoadInt(string key, Action<bool, int> onCompleted)
        {
            LoadInternalAsync(key, (success, rawData) =>
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
            LoadInternalAsync(key, (success, rawData) =>
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
            LoadInternalAsync(key, (success, rawData) =>
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

        private async void SaveInternalAsync(string key, string data, Action<bool> onCompleted = null)
        {
            using (StreamWriter writer = new StreamWriter(CreateFullPath(key)))
                await writer.WriteAsync(data);
            
            _logger.Log(SavedMessage);
            onCompleted?.Invoke(true);
        }

        private async void LoadInternalAsync(string key, Action<bool, string> onCompleted)
        {
            string data;

            if (Directory.Exists(_saveDirectory) == false)
            {
                _logger.LogError(DirectoryErrorMessage);
                onCompleted?.Invoke(false, null);
                return;
            }

            try
            {
                using StreamReader reader = new StreamReader(CreateFullPath(key));
                data = await reader.ReadToEndAsync();
            }
            catch
            {
                data = null;
            }

            _logger.Log(LoadedMessage);
            onCompleted?.Invoke(true, data);
        }

        private string CreateFullPath(string key)
        {
            return Path.Combine(_saveDirectory, key + FileExtension);
        }
    }
}
#endif
