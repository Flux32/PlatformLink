using System;
using UnityEngine;

[Serializable]
public class YandexSettings
{
    [SerializeField] private bool _enableYandexMetrika;
    [SerializeField] private string _yandexMetrikaId;

    public bool EnableYandexMetrika => _enableYandexMetrika;
    public string YandexMetrikaId => _yandexMetrikaId;
}
