namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules.Social
{
    public interface IShareDialogAdapter
    {
        bool IsAvailable { get; }
        void Show(ShareRequest request);
    }
}
