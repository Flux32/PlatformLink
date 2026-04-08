using System;
using UnityEngine;

[Serializable]
public class IosSettings
{
    public AdmobSettings Admob => _admobSettings;

    [SerializeField] private AdmobSettings _admobSettings;
}
