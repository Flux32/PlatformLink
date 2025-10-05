using System;
using UnityEngine;

[Serializable]
public class AdvertisementSettings
{
    [SerializeField] private AdmobSettings _admobSettings;

    public AdmobSettings Admob => _admobSettings;
}
