using System.Runtime.InteropServices;
using UnityEngine;
using PlatformLink.Common;

namespace PlatformLink.Platform.YandexGames
{
    public class YandexEnvironment : MonoBehaviour, IEnvironment
    {
        public DeviceType DeviceType => GetDeviceType();
        public string Language => GetLanguage();


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

        private string GetLanguage()
        {
            return jslib_getLanguage();
        }
    }
}