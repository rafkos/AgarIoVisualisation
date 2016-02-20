using System;
using System.Collections.Generic;
using System.Linq;
using AgarIo.Contract;
using AgarIo.Contract.AdminCommands;
using AgarIo.Contract.GameModes.Classic;
using Assets.Scripts.Events.Game;
using Assets.Scripts.Events.UI;
using Assets.Scripts.Extensions;
using Assets.Scripts.Models;
using Assets.Scripts.Services;
using UnityEngine;

namespace Assets.Scripts.ServerConnection
{
    public class GameEventPublisher
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly Queue<Action> _executeOnMainThread;
        
        public GameEventPublisher(IEventAggregator eventAggregator, Queue<Action> executeOnMainThread)
        {
            _eventAggregator = eventAggregator;
            _executeOnMainThread = executeOnMainThread;
        }

        public void Handle(StatePushDto pushData)
        {
            var customDataText = pushData.CustomGameModeData;
            var customData = customDataText.FromJson<ClassicGameModeDataDto>();

            var playerStats = customData.PlayerStats;

            PublishScoreUpdatedEvent(playerStats);

            PublishFoodEvents(pushData);
            PublishVirusEvents(pushData);
            PublishPlayerEvents(pushData);
        }

        private void PublishScoreUpdatedEvent(IEnumerable<PlayerStatDto> playerStats)
        {
            var playerScores = playerStats.Select(i => new PlayerScore { PlayerName = i.Name, PlayerColor = Color.green, Score = i.Score });

            _executeOnMainThread.Enqueue(() => _eventAggregator.Publish(new PlayerScoreUpdatedEvent(playerScores)));
        }

        private void PublishFoodEvents(StatePushDto pushData)
        {
            var worldSize = pushData.WorldSize;
            var addedFood = pushData.AddedBlobs.Where(b => b.Type == BlobType.Food);
            var updatedFood = pushData.UpdatedBlobs.Where(b => b.Type == BlobType.Food);
            var removedFood = pushData.RemovedBlobs.Where(b => b.Type == BlobType.Food);

            _executeOnMainThread.Enqueue(() => _eventAggregator.Publish(new FoodEvent(addedFood, BlobEventType.BlobAdded,  worldSize)));
            _executeOnMainThread.Enqueue(() => _eventAggregator.Publish(new FoodEvent(updatedFood, BlobEventType.BlobUpdated, worldSize)));
            _executeOnMainThread.Enqueue(() => _eventAggregator.Publish(new FoodEvent(removedFood, BlobEventType.BlobRemoved,  worldSize)));
        }

        private void PublishVirusEvents(StatePushDto pushData)
        {
            var worldSize = pushData.WorldSize;
            var addedVirus = pushData.AddedBlobs.Where(b => b.Type == BlobType.Virus);
            var updatedVirus = pushData.UpdatedBlobs.Where(b => b.Type == BlobType.Virus);
            var removedVirus = pushData.RemovedBlobs.Where(b => b.Type == BlobType.Virus);

            _executeOnMainThread.Enqueue(() => _eventAggregator.Publish(new VirusEvent(addedVirus, BlobEventType.BlobAdded, worldSize)));
            _executeOnMainThread.Enqueue(() => _eventAggregator.Publish(new VirusEvent(updatedVirus, BlobEventType.BlobUpdated, worldSize)));
            _executeOnMainThread.Enqueue(() => _eventAggregator.Publish(new VirusEvent(removedVirus, BlobEventType.BlobRemoved, worldSize)));
        }

        private void PublishPlayerEvents(StatePushDto pushData)
        {
            var worldSize = pushData.WorldSize;
            var addedPlayers = pushData.AddedBlobs.Where(b => b.Type == BlobType.Player);
            var updatedPlayers = pushData.UpdatedBlobs.Where(b => b.Type == BlobType.Player);
            var removedPlayers = pushData.RemovedBlobs.Where(b => b.Type == BlobType.Player);

            _executeOnMainThread.Enqueue(() => _eventAggregator.Publish(new PlayersEvent(addedPlayers, BlobEventType.BlobAdded, worldSize)));
            _executeOnMainThread.Enqueue(() => _eventAggregator.Publish(new PlayersEvent(updatedPlayers, BlobEventType.BlobUpdated,  worldSize)));
            _executeOnMainThread.Enqueue(() => _eventAggregator.Publish(new PlayersEvent(removedPlayers, BlobEventType.BlobRemoved, worldSize)));
        }
    }
}