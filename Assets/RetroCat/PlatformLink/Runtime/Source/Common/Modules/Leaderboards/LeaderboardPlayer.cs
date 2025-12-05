using System;

namespace RetroCat.PlatformLink.Runtime.Source.Common.Modules.Leaderboards
{
    [Serializable]
    public class LeaderboardPlayer
    {
        public string Lang { get; }
        public string PublicName { get; }
        public string UniqueId { get; }
        public string AvatarUrl { get; }
        public string AvatarPermission { get; }
        public string PublicNamePermission { get; }

        public LeaderboardPlayer(
            string lang,
            string publicName,
            string uniqueId,
            string avatarUrl,
            string avatarPermission,
            string publicNamePermission)
        {
            Lang = lang;
            PublicName = publicName;
            UniqueId = uniqueId;
            AvatarUrl = avatarUrl;
            AvatarPermission = avatarPermission;
            PublicNamePermission = publicNamePermission;
        }
    }
}

