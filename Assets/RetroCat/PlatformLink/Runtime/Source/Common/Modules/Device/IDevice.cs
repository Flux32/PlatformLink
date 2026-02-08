namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules.Device
{
    public interface IDevice
    {
        bool IsVibrationSupported();
        void Vibrate(VibrationPreset preset);
        void Vibrate(VibrationSettings settings);
    }
}
