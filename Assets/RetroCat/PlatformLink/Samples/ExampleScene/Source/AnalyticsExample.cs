using UnityEngine;
using UnityEngine.UI;

namespace PlatformLink.Examples
{
    public class AnalyticsExample : MonoBehaviour
    {
        [SerializeField] private Button _gameReadyButton;
        
        private void OnEnable()
        {
            _gameReadyButton.onClick.AddListener(OnGameReadyButtonClicked);
        }

        private void OnDisable()
        {
            _gameReadyButton.onClick.RemoveListener(OnGameReadyButtonClicked);
        }

        private void OnGameReadyButtonClicked()
        {
            PLink.Analytics.SendGameReady();
        }
    }
}