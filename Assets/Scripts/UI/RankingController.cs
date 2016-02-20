using System;
using System.Linq;
using Assets.Scripts.Events.UI;
using Assets.Scripts.Services;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class RankingController : MonoBehaviour
    {
        public RankingDisplay RankingDisplayPrefab;
        public Transform TargetTransform;
        private IEventAggregator _eventAggregator;
        private Action<PlayerScoreUpdatedEvent> _playerScoreUpdatedEventAction;
        private RankingDisplay _rankingDisplayObject;


        public void Awake()
        {
            _eventAggregator = DependencyResolver.Current.GetService<IEventAggregator>();
            _playerScoreUpdatedEventAction = OnPlayerScoreUpdated;
            _eventAggregator.Subscribe(_playerScoreUpdatedEventAction);
        }

        // Use this for initialization
        public void Start ()
        {
            _rankingDisplayObject = Instantiate(RankingDisplayPrefab);
            _rankingDisplayObject.transform.SetParent(TargetTransform, false);
        }
	
        // Update is called once per frame
        public void Update () {
	
        }

        private void OnPlayerScoreUpdated(PlayerScoreUpdatedEvent playerScoreUpdatedEvent)
        {
            var sortedPlayerScores = playerScoreUpdatedEvent.PlayerScores.OrderByDescending(i => i.Score);
            _rankingDisplayObject.RequestUpdateOfRanking(sortedPlayerScores);
        }
    }
}
