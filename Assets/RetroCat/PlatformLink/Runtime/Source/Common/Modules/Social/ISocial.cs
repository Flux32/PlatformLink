namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules.Social
{
    public interface ISocial
    {
        string CreateShareLink(SocialPlatform platform, ShareRequest request);
        void ShowShareDialog(ShareRequest request);
        bool IsShareDialogAvailable();
    }
}
