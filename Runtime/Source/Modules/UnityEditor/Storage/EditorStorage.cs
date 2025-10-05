#if UNITY_EDITOR
using System;
using System.IO;
using UnityEngine;
using UnityEditor;
using PlatformLink.Common;

using ILogger = PlatformLink.PluginDebug.ILogger;

namespace PlatformLink.Platform.UnityEditor
{
    public class EditorStorage : IStorage
    {
        private const string FileExtension = ".txt";
        private readonly string SaveDirectory = Application.dataPath + "/PlatformLink/Editor/Saves/";

        private const string SavedMessage = "data saved in editor mode";
        private const string LoadedMessage = "data loaded in editor mode";

        private const string DirectoryErrorMessage = "Unable to save file. Directory {0} not found.";

        private readonly ILogger _logger;

        public EditorStorage(ILogger logger)
        {
            _logger = logger;
        }

        public void Load(string key, Action<bool, string> onCompleted)
        {
            LoadAsync(key, onCompleted);
        }

        public void Save(string key, string data, Action<bool> onCompleted = null)
        {
            SaveAsync(key, data, onCompleted);
        }

        private async void SaveAsync(string key, string data, Action<bool> onCompleted = null)
        {
            using (StreamWriter writer = new StreamWriter(CreateFullPath(key)))
                await writer.WriteAsync(data);


            AssetDatabase.Refresh();

            _logger.Log(SavedMessage);
            onCompleted?.Invoke(true);
        }

        private async void LoadAsync(string key, Action<bool, string> onCompleted)
        {
            string data;

            if (Directory.Exists(SaveDirectory) == false)
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
            return Path.Combine(SaveDirectory, key + FileExtension);
        }
    }
}
#endif