namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules.Social
{
    public sealed class ShareRequest
    {
        public ShareRequest(string title = "", string text = "", string url = "")
        {
            Title = title ?? string.Empty;
            Text = text ?? string.Empty;
            Url = url ?? string.Empty;
        }

        public string Title { get; }
        public string Text { get; }
        public string Url { get; }
    }
}
