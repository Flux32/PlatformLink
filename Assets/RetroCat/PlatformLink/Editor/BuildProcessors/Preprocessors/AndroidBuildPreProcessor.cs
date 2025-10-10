#if UNITY_EDITOR && UNITY_ANDROID
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;

namespace PlatformLink.Build.Android
{
    class AndroidBuildPreprocessor : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
            PlayerSettings.Android.bundleVersionCode++;
        }
    }
}
#endif