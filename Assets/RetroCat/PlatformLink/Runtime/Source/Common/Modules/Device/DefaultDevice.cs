using System; //don't remove
using UnityEngine;

using ILogger = PlatformLink.PluginDebug.ILogger; 

namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules.Device
{
    public class DefaultDevice : IDevice
    {
        private const int LightDurationMs = 20;
        private const int MediumDurationMs = 40;
        private const int StrongDurationMs = 80;

        private readonly ILogger _logger;

        public DefaultDevice(ILogger logger)
        {
            _logger = logger;
        }
        
        public bool IsVibrationSupported()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            return jslib_isVibrationSupported();
#elif UNITY_ANDROID || UNITY_IOS
            return true;
#else
            return false;
#endif
        }

        public void CopyToClipboard(string text)
        {
            string safeText = text ?? string.Empty;

#if UNITY_WEBGL && !UNITY_EDITOR
            jslib_copyToClipboard(safeText);
#else
            GUIUtility.systemCopyBuffer = safeText;
#endif
            _logger.Log($"copy to clipboard {text}");
        }

        public void Vibrate(VibrationPreset preset)
        {
            int durationMs = preset switch
            {
                VibrationPreset.Light => LightDurationMs,
                VibrationPreset.Medium => MediumDurationMs,
                VibrationPreset.Strong => StrongDurationMs,
                _ => MediumDurationMs,
            };

            Vibrate(new VibrationSettings(durationMs));
        }

        public void Vibrate(VibrationSettings settings)
        {
            if (IsVibrationSupported() == false)
            {
                _logger.LogWarning($"can't vibrate, because vibration is not supported");
                return;
            }

#if UNITY_WEBGL && !UNITY_EDITOR
            if (settings.HasPattern)
            {
                string patternCsv = string.Join(",", settings.PatternMs);
                jslib_vibratePattern(patternCsv);
                return;
            }

            if (settings.DurationMs <= 0)
                return;

            jslib_vibrate(settings.DurationMs);
#elif UNITY_ANDROID || UNITY_IOS
            Handheld.Vibrate();
#endif
            _logger.Log($"vibrate");
        }

#if UNITY_WEBGL && !UNITY_EDITOR
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern bool jslib_isVibrationSupported();

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void jslib_vibrate(int durationMs);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void jslib_vibratePattern(string patternCsv);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void jslib_copyToClipboard(string text);
#endif
    }
}
