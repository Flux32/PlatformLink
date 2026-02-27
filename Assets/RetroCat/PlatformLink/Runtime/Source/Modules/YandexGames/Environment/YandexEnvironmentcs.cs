using System.Runtime.InteropServices;
using UnityEngine;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Environment;
using DeviceType = RetroCat.PlatformLink.Runtime.Source.Common.Modules.Environment.DeviceType;

namespace PlatformLink.Platform.YandexGames
{
    public class YandexEnvironment : MonoBehaviour, IEnvironment
    {
        public DeviceType DeviceType => GetDeviceType();
        public string Language => GetLanguage();
        public string AppId => GetAppId();
        public string AppUrl => $"https://yandex.ru/games/app/{AppId}";
        
        [DllImport("__Internal")]
        private static extern string jslib_getDeviceType();

        private DeviceType GetDeviceType()
        {
            string stringDeviceType = jslib_getDeviceType();

            return stringDeviceType switch
            {
                "mobile" => DeviceType.Mobile,
                "desktop" => DeviceType.Desktop,
                "tablet" => DeviceType.Tablet,
                "tv" => DeviceType.TV,
                _ => DeviceType.Desktop,
            };
        }

        [DllImport("__Internal")]
        private static extern string jslib_getLanguage();

        private string GetLanguage() => jslib_getLanguage();

        [DllImport("__Internal")]
        private static extern string jslib_getAppId();

        private string GetAppId() => jslib_getAppId();
    }
}
