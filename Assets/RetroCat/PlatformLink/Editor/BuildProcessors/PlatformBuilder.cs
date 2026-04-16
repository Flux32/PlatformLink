using System.Linq;
using UnityEditor;
using UnityEditor.Build.Reporting;
using UnityEngine;

public static class PlatformBuilder
{
    public static void Build(PlatformSettingsEntry entry)
    {
        if (PlatformBuildTargetResolver.TryGetBuildTarget(entry.Type, out BuildTarget target) == false)
            return;

        if (EditorUserBuildSettings.activeBuildTarget != target)
            return;

        string[] scenes = EditorBuildSettings.scenes
            .Where(scene => scene.enabled)
            .Select(scene => scene.path)
            .ToArray();

        if (scenes.Length == 0)
        {
            Debug.LogError("PlatformBuilder: no enabled scenes in Build Settings.");
            return;
        }

        string defaultName = string.IsNullOrWhiteSpace(entry.Name) ? entry.Type.ToString() : entry.Name;
        string outputPath = ResolveOutputPath(target, defaultName);

        if (string.IsNullOrEmpty(outputPath))
            return;

        BuildPlayerOptions options = new BuildPlayerOptions
        {
            scenes = scenes,
            locationPathName = outputPath,
            target = target,
            targetGroup = BuildPipeline.GetBuildTargetGroup(target),
            options = BuildOptions.None,
        };

        BuildReport report = BuildPipeline.BuildPlayer(options);
        Debug.Log($"PlatformBuilder: build finished with result '{report.summary.result}' at '{outputPath}'.");
    }

    private static string ResolveOutputPath(BuildTarget target, string defaultName)
    {
        switch (target)
        {
            case BuildTarget.Android:
                return EditorUtility.SaveFilePanel("Build Android", string.Empty, defaultName, "apk");
            case BuildTarget.iOS:
                return EditorUtility.SaveFolderPanel("Build iOS", string.Empty, defaultName);
            case BuildTarget.WebGL:
                return EditorUtility.SaveFolderPanel("Build WebGL", string.Empty, defaultName);
            default:
                return string.Empty;
        }
    }
}
