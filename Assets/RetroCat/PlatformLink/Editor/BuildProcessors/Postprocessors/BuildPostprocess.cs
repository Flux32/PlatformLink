using UnityEditor;
using UnityEditor.Callbacks;
using PlatformLink.PluginDebug;
using RetroCat.PlatformLink.Runtime.Source.Common.Debug;

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