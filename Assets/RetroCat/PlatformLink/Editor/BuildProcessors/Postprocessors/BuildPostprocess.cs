using UnityEditor;
using UnityEditor.Callbacks;
using PlatformLink.PluginDebug;

public class BuildPostprocess
{
    [PostProcessBuild]
    public static void OnPostprocessBuild(BuildTarget target, string pathToBuiltProject)
    {
        if (target == BuildTarget.WebGL)
        {
            BuildZipArchiving zipPacker = new BuildZipArchiving(new PLinkLogger());
            zipPacker.Archive(pathToBuiltProject);
        }
    }
}