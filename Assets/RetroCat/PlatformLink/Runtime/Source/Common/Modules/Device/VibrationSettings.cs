using System;

namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules.Device
{
    public struct VibrationSettings
    {
        public int DurationMs { get; }
        public int[] PatternMs { get; }
        public bool HasPattern => PatternMs != null && PatternMs.Length > 0;

        public VibrationSettings(int durationMs)
        {
            DurationMs = Math.Max(0, durationMs);
            PatternMs = null;
        }

        public VibrationSettings(int[] patternMs)
        {
            DurationMs = 0;
            PatternMs = patternMs;
        }
    }
}
