using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class YandexCore : MonoBehaviour
{
    private Action _onInitialized;
    private bool _isInitialized;

    [DllImport("__Internal")]
    private static extern string jslib_initializePlugin();
    
    public void Initialize(Action onInitialized)
    {
        if (_onInitialized != null)
            Debug.LogError("Initialize operation already called");
        
        _onInitialized = onInitialized;
        jslib_initializePlugin();
    }
    
    #region Called from PlatformLink.js
    private void fjs_platformLinkInitialized()
    {
        _isInitialized = true;
        _onInitialized?.Invoke();
    }
    #endregion
}
