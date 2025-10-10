using System;
using UnityEngine;

[Serializable]
public class AdmobSettings
{
    public string InterstitialUnitID => _interstitialUnitID;
    public string RewardedUnitID => _rewardedUnitID;

    [SerializeField] private string _appID;
    [SerializeField] private string _interstitialUnitID;
    [SerializeField] private string _rewardedUnitID;
}