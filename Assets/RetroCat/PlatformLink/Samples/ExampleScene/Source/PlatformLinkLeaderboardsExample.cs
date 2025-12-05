using System.Collections.Generic;
using RetroCat.PlatformLink.Runtime.Source.Common.Modules.Leaderboards;
using UnityEngine;
using UnityEngine.UI;

namespace PlatformLink.Examples
{
    public class PlatformLinkLeaderboardsExample : MonoBehaviour
    {
        [SerializeField] private Button _loadEntryButton;
        [SerializeField] private PlatformLinkLeaderboardEntryExample _entryPrefab;
        [SerializeField] private Transform _entryContainer;
        [SerializeField] private string _leaderboardId;
        [SerializeField] private int _quantityTop = 5;
        [SerializeField] private int _quantityAround = 5;
        [SerializeField] private bool _includeUser = true;
        
        private void OnEnable()
        {
            _loadEntryButton.onClick.AddListener(OnLoadEntryClick);
        }

        private void OnDisable()
        {
            _loadEntryButton.onClick.RemoveListener(OnLoadEntryClick);
        }

        private void OnLoadEntryClick()
        {
            _loadEntryButton.interactable = false;
            List<Transform> children = new List<Transform>();
            
            for (int i = 0; i < _entryContainer.childCount; i++)
                children.Add(_entryContainer.GetChild(i));
            
            foreach (Transform child in children)
                Destroy(child.gameObject);
            
            PLink.Leaderboard.GetEntries(
                _leaderboardId,
                _quantityTop,
                _includeUser,
                _quantityAround,
                (isSuccess, entries) =>
                {
                    _loadEntryButton.interactable = true;

                    if (isSuccess)
                    {
                        foreach (LeaderboardEntry entry in entries.Entries)
                        {
                            PlatformLinkLeaderboardEntryExample entryObject = Instantiate(_entryPrefab, _entryContainer);
                            entryObject.Initialize(entry);
                        }
                    }
                });
        }
    }
}