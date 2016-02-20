using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Models;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class RankingDisplay : MonoBehaviour, IMonoBehaviour
    {
        public Transform TargetTransform;
        public RankingEntryDisplay RankingEntryDisplayPrefab;
        private IList<GameObject> _displayedRankingEntries;
        private IEnumerable<PlayerScore> _currentRanking;
        private bool _updatePending;

        public RankingDisplay()
        {
            _displayedRankingEntries = new List<GameObject>();
            _currentRanking = new List<PlayerScore>();
        }

        public void Awake()
        {
        }

        public void Start()
        {
        }

        public void Update()
        {
            if (!_updatePending)
            {
                return;
            }

            ClearDisplayedList();

            UpdateRanking();

            _updatePending = false;
        }

        public void OnDestroy()
        {
        }

        public void RequestUpdateOfRanking(IEnumerable<PlayerScore> currentRanking)
        {
            _currentRanking = currentRanking.ToList();
            _updatePending = true;
        }

        private void UpdateRanking()
        {
            _displayedRankingEntries.Clear();

            foreach (var rankingEntry in _currentRanking)
            {
                var rankingEntryDisplay = Instantiate(RankingEntryDisplayPrefab);

                rankingEntryDisplay.transform.SetParent(TargetTransform, false);
                rankingEntryDisplay.Update(rankingEntry);

                _displayedRankingEntries.Add(rankingEntryDisplay.gameObject);
            }
        }

        private void ClearDisplayedList()
        {
            foreach (var rankingEntry in _displayedRankingEntries)
            {
                Destroy(rankingEntry);
            }
        }
    }
}