using System;
using UnityEngine;

[Serializable]
public class AndroidSettings
{
    public AdmobSettings Admob => _admobSettings;

    [SerializeField] private AdmobSettings _admobSettings;
}
