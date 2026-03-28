using System.Runtime.InteropServices;

namespace PlatformLink
{
    public static class LoadingScreen
    {
        public static void Close()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            jslib_closeLoadingScreen();
#endif
        }

#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void jslib_closeLoadingScreen();
#endif
    }
}
