using System;
using UnityEngine;

namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules.Device
{
    public class DefaultDevice : IDevice
    {
        private const int LightDurationMs = 20;
        private const int MediumDurationMs = 40;
        private const int StrongDurationMs = 80;

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
                return;

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
        }

#if UNITY_WEBGL && !UNITY_EDITOR
        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern bool jslib_isVibrationSupported();

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void jslib_vibrate(int durationMs);

        [System.Runtime.InteropServices.DllImport("__Internal")]
        private static extern void jslib_vibratePattern(string patternCsv);
#endif
    }
}
