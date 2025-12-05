using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Leaderboards;
using UnityEngine;
using UnityEngine.UI;

namespace PlatformLink.Examples
{
    public class PlatformLinkLeaderboardEntryExample : MonoBehaviour
    {
        [SerializeField] private Text _label;

        public void Initialize(LeaderboardEntry entry)
        {
            _label.text = $"{entry.Rank}. {entry.Player.PublicName} {entry.Score}";
        }
    }
}