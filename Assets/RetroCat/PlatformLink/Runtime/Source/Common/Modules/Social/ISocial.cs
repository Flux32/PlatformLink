namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules.Social
{
    public interface ISocial
    {
        string CreateShareLink(SocialPlatform platform, string url, string comment);
    }
}
