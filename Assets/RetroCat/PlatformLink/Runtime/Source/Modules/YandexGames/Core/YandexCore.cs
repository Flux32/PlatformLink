using System;
using UnityEngine;

public class YandexCore : MonoBehaviour
{
    private Action _onInitialized;
    private bool _isInitialized;

    public void Initialize(Action onInitialized)
    {
        if (_onInitialized != null)
            Debug.LogError("Initialize operation already called");
        
        _onInitialized = onInitialized;
    }
    
    #region Called from PlatformLink.js
    private void fjs_platformLinkInitialized()
    {
        _isInitialized = true;
        _onInitialized?.Invoke();
    }
    #endregion
}
