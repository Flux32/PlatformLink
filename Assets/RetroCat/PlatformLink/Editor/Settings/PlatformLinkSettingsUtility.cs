using System.IO;
using UnityEditor;
using UnityEngine;

public static class PlatformLinkSettingsUtility
{
    private const string ProjectResourceFolder = "Assets/Resources/ProjectConfigs";
    private const string ProjectAssetPath = ProjectResourceFolder + "/PlatformLinkConfig.asset";
    private const string DefaultResourcePath = "Configs/PlatformLinkConfig";

    [MenuItem("Window/PlatformLink/Create Project Settings Asset", false, 1)]
    public static void CreateOrSelectProjectSettings()
    {
        EnsureProjectSettingsAssetExists();
        Selection.activeObject = AssetDatabase.LoadAssetAtPath<Object>(ProjectAssetPath);
        EditorGUIUtility.PingObject(Selection.activeObject);
    }

    public static void EnsureProjectSettingsAssetExists()
    {
        if (File.Exists(ProjectAssetPath))
            return;

        if (!AssetDatabase.IsValidFolder("Assets/Resources"))
        {
            AssetDatabase.CreateFolder("Assets", "Resources");
        }

        if (!AssetDatabase.IsValidFolder(ProjectResourceFolder))
        {
            AssetDatabase.CreateFolder("Assets/Resources", "ProjectConfigs");
        }

        var defaultSettings = Resources.Load<PlatformLinkSettings>(DefaultResourcePath);
        
        if (defaultSettings == null)
        {
            var empty = ScriptableObject.CreateInstance<PlatformLinkSettings>();
            AssetDatabase.CreateAsset(empty, ProjectAssetPath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            return;
        }

        var instance = Object.Instantiate(defaultSettings);
        instance.name = "PlatformLinkConfig";
        AssetDatabase.CreateAsset(instance, ProjectAssetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
}

