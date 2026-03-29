namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules.Environment
{
    public interface IEnvironment
    {
        DeviceType DeviceType { get; }
        string Language { get; }
        string AppId { get; }
        string AppUrl { get; }
        void CloseLoadingScreen();
    }
}
