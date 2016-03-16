using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Events.Game;
using Assets.Scripts.Events.UI;
using Assets.Scripts.Models;
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
        private Action<PlayerColorAssignedEvent> _playerColorsAssignedEventAction;
        private RankingDisplay _rankingDisplayObject;
        private Dictionary<string, Color> _playerColors;

        public void Awake()
        {
            _eventAggregator = DependencyResolver.Current.GetService<IEventAggregator>();
            _playerColors = new Dictionary<string, Color>();
            _playerScoreUpdatedEventAction = OnPlayerScoreUpdated;
            _playerColorsAssignedEventAction = OnPlayerColorAssigned;
            _eventAggregator.Subscribe(_playerScoreUpdatedEventAction);
            _eventAggregator.Subscribe(_playerColorsAssignedEventAction);
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
            var sortedPlayerScores = playerScoreUpdatedEvent.PlayerScores.OrderByDescending(i => i.Score).Select(ps =>
            {
                Color color;
                var colorWasFound = _playerColors.TryGetValue(ps.PlayerName, out color);

                if (!colorWasFound)
                {
                    color = Color.black;
                }

                return new PlayerScore { PlayerName = ps.PlayerName, PlayerColor = new Color(color.r, color.g, color.b), Score = ps.Score };
            });

            _rankingDisplayObject.RequestUpdateOfRanking(sortedPlayerScores);
        }

        private void OnPlayerColorAssigned(PlayerColorAssignedEvent playerColorAssignedEvent)
        {
            if (!_playerColors.ContainsKey(playerColorAssignedEvent.PlayerName))
            {
                _playerColors.Add(playerColorAssignedEvent.PlayerName, playerColorAssignedEvent.Color);
            }
        }
    }
}
