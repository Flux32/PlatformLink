using UnityEngine;
using UnityEngine.UI;

namespace PlatformLink.Examples
{
    public class AdvertisementExample : MonoBehaviour
    {
        [SerializeField] private Button _showInterstetialAdButton;
        [SerializeField] private Button _showRewardedAdButton;

        private void OnEnable()
        {
            _showInterstetialAdButton.onClick.AddListener(OnShowInterstetialAdButtonClicked);
            _showRewardedAdButton.onClick.AddListener(OnShowRewardedsAdButtonClicked);
        }

        private void OnDisable()
        {
            _showInterstetialAdButton.onClick.RemoveListener(OnShowInterstetialAdButtonClicked);
            _showRewardedAdButton.onClick.RemoveListener(OnShowRewardedsAdButtonClicked);
        }

        private void OnShowInterstetialAdButtonClicked()
        {
            if (PLink.Advertisement.InterstetialAd.CanShow() == true)
                PLink.Advertisement.InterstetialAd.Show();
        }

        private void OnShowRewardedsAdButtonClicked()
        {
            if (PLink.Advertisement.RewardedAd.CanShow() == true)
                PLink.Advertisement.RewardedAd.Show();
        }
    }
}