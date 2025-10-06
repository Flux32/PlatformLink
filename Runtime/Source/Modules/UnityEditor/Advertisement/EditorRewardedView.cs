using System;
using UnityEngine;
using UnityEngine.UI;

public class EditorRewardedView : MonoBehaviour
{
    public event Action Closed;
    public event Action Rewarded;
    public event Action RewardRejected;
    public event Action Opened;

    [SerializeField] private Button _closeButton;
    [SerializeField] private Button _rewardButton;
    
    public bool IsOpened { get; private set; }
    
    private void Awake()
    {
        gameObject.SetActive(false);
    }

    public void Open()
    {
        gameObject.SetActive(true);
        Opened?.Invoke();
    }

    public void Close()
    {
        gameObject.SetActive(false);
        Closed?.Invoke();
    }

    private void OnEnable()
    {
        _closeButton.onClick.AddListener(OnButtonCloseRewardedClicked);
        _rewardButton.onClick.AddListener(OnRewardButtonClicked);
    }

    private void OnDisable()
    {
        _closeButton.onClick.RemoveListener(OnButtonCloseRewardedClicked);
        _closeButton.onClick.RemoveListener(OnRewardButtonClicked);
    }

    private void OnButtonCloseRewardedClicked()
    {
        RewardRejected?.Invoke();
        Close();
    }

    private void OnRewardButtonClicked()
    {
        Rewarded?.Invoke();
        Close();
    }
}