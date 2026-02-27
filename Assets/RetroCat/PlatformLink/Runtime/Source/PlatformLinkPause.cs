using PlatformLink.PluginDebug;
using RetroCat.PlatformLink.Runtime.Source.Common.Debug;
using UnityEngine;

using ILogger = PlatformLink.PluginDebug.ILogger;

namespace PlatformLink
{
    public class PlatformLinkPause : MonoBehaviour
    {
        [SerializeField] private bool _timeScalePause = true;
        [SerializeField] private bool _audioPause = true;

        private readonly ILogger _logger = new PLinkLogger(); //TODO: Inject;

        private const string GamePausedMessage = "game paused";
        private const string GameResumedMessage = "game resumed";

        private float _pausedTimeScaleValue;
        private bool _pausedAudioListenerValue;

        private void OnEnable()
        {
            PLink.Advertisement.AdOpened += OnAdOpened; //TODO: Execution order
            PLink.Advertisement.AdClosed += OnAdClosed;
        }

        private void OnDisable()
        {
            PLink.Advertisement.AdOpened -= OnAdOpened; //TODO: Execution order
            PLink.Advertisement.AdClosed -= OnAdClosed;
        }

        private void OnAdOpened()
        {
            if (_timeScalePause == true)
                PauseTimeScale();

            if (_audioPause == true)
                PauseVolume();

            _logger.Log(GamePausedMessage);
        }

        private void OnAdClosed()
        {
            if (_timeScalePause == true)
                ResumeTimeScale();

            if (_audioPause == true)
                AudioListener.pause = _pausedAudioListenerValue;

            _logger.Log(GameResumedMessage);
        }

        private void ResumeTimeScale()
        {
            Time.timeScale = _pausedTimeScaleValue;
        }

        private void PauseTimeScale()
        {
            _pausedTimeScaleValue = Time.timeScale;
            Time.timeScale = 0;
        }

        private void PauseVolume()
        {
            _pausedAudioListenerValue = AudioListener.pause;
            AudioListener.pause = true;
        }
    }
}