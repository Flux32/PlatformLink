using System;
using UnityEngine;

[Serializable]
public class YandexSettings
{
    [SerializeField] private bool _enableYandexMetrika;
    [SerializeField] private string _yandexMetrikaId;
    [SerializeField] private YandexLoadingScreenSettings _loadingScreen = new YandexLoadingScreenSettings();

    public bool EnableYandexMetrika => _enableYandexMetrika;
    public string YandexMetrikaId => _yandexMetrikaId;
    public YandexLoadingScreenSettings LoadingScreen
    {
        get
        {
            if (_loadingScreen == null)
                _loadingScreen = new YandexLoadingScreenSettings();

            return _loadingScreen;
        }
    }
}

[Serializable]
public class YandexLoadingScreenSettings
{
    [SerializeField] private bool _manualClose;

    public bool ManualClose => _manualClose;
}
