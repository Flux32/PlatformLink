using System;
using UnityEngine;
using UnityEngine.Networking;

namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules.Purchases
{
    [Serializable]
    public class RemoteIcon
    {
        public string Url { get; }

        public RemoteIcon(string url)
        {
            Url = url;
        }

        public void LoadTexture(Action<bool, Texture2D> onCompleted)
        {
            if (string.IsNullOrEmpty(Url))
            {
                onCompleted?.Invoke(false, null);
                return;
            }

            UnityWebRequest request = UnityWebRequestTexture.GetTexture(Url);
            var operation = request.SendWebRequest();
            operation.completed += _ =>
            {
#if UNITY_2020_1_OR_NEWER
                bool success = request.result == UnityWebRequest.Result.Success;
#else
                bool success = !request.isHttpError && !request.isNetworkError;
#endif
                Texture2D texture = null;
                if (success)
                {
                    texture = DownloadHandlerTexture.GetContent(request);
                }

                onCompleted?.Invoke(success, texture);
                request.Dispose();
            };
        }
    }
}

