#if UNITY_EDITOR
using System;
using UnityEngine;
using PlatformLink.Common;

public class EditorLeaderboard : ILeaderboard
{
    public void GetScore(string leaderBoardName, Action<bool, int> onCompleted)
    {
        int score = PlayerPrefs.GetInt(leaderBoardName, 0);
        onCompleted.Invoke(true, score);
    }

    public void SetScore(string leaderBoardName, int score, Action<bool> onCompleted = null)
    {
        PlayerPrefs.SetInt(leaderBoardName, score);
        onCompleted?.Invoke(true);
    }
}
#endif