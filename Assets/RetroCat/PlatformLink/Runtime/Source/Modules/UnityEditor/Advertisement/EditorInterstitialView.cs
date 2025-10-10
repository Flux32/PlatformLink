#if UNITY_EDITOR
using System;
using UnityEngine;
using UnityEngine.UI;

public class EditorInterstitialView : MonoBehaviour
{ 
    public event Action Closed;
    public event Action Opened;

    [SerializeField] private Button _closeButton;

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
        _closeButton.onClick.AddListener(OnButtonCloseInterstetialClicked);
    }

    private void OnDisable()
    {
        _closeButton.onClick.RemoveListener(OnButtonCloseInterstetialClicked);
    }

    private void OnButtonCloseInterstetialClicked()
    {
        Close();
    }
}
#endif