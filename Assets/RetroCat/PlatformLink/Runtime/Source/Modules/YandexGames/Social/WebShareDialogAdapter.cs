using System;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Social;
using UnityEngine;

namespace PlatformLink.Platform.YandexGames
{
    public class WebShareDialogAdapter : MonoBehaviour, IShareDialogAdapter
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern bool jslib_isNativeShareAvailable();

        [DllImport("__Internal")]
        private static extern void jslib_showNativeShare(string payload);
#endif

        public bool IsAvailable
        {
            get
            {
#if UNITY_WEBGL && !UNITY_EDITOR
                return jslib_isNativeShareAvailable();
#else
                return false;
#endif
            }
        }

        public void Show(ShareRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

#if UNITY_WEBGL && !UNITY_EDITOR
            string payload = JsonUtility.ToJson(new NativeSharePayload(request));
            jslib_showNativeShare(payload);
#else
            Debug.LogWarning("Native share dialog is only available in WebGL builds.");
#endif
        }

        [Serializable]
        private struct NativeSharePayload
        {
            public string title;
            public string text;
            public string url;

            public NativeSharePayload(ShareRequest request)
            {
                title = request.Title;
                text = request.Text;
                url = request.Url;
            }
        }
    }
}
