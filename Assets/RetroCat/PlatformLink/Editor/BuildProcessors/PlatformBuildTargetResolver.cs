using UnityEditor;

public static class PlatformBuildTargetResolver
{
    public static bool TryGetBuildTarget(PlatformSettingsType type, out BuildTarget target)
    {
        switch (type)
        {
            case PlatformSettingsType.Android:
                target = BuildTarget.Android;
                return true;
            case PlatformSettingsType.Ios:
                target = BuildTarget.iOS;
                return true;
            case PlatformSettingsType.YandexGames:
                target = BuildTarget.WebGL;
                return true;
            default:
                target = BuildTarget.NoTarget;
                return false;
        }
    }

    public static bool IsActiveBuildTargetMatching(PlatformSettingsType type)
    {
        return TryGetBuildTarget(type, out BuildTarget target)
            && EditorUserBuildSettings.activeBuildTarget == target;
    }
}
