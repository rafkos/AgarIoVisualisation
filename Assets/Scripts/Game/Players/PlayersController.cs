using System;
using System.Collections.Generic;
using System.Linq;
using AgarIo.Contract;
using Assets.Scripts.Events.Game;
using Assets.Scripts.Game.Camera;
using Assets.Scripts.Game.Players;
using Assets.Scripts.ServerConnection;
using Assets.Scripts.Services;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class PlayersController : MonoBehaviour, IMonoBehaviour
    {
        private readonly Dictionary<int, Player> _playersObjects;
        private readonly Queue<BlobToUpdate> _playersUpdateQueue;
        private IEventAggregator _eventAggregator;
        private SubscriptionToken _playersEventSubscriptionToken;

        public Player PlayerPrefab;
        public List<Color> FoodColors;
        private CameraFollowCurrentPlayer _cameraFollow;
        private GameSettings _gameSettings;

        public PlayersController()
        {
            _playersObjects = new Dictionary<int, Player>();
            _playersUpdateQueue = new Queue<BlobToUpdate>();
        }

        public void Awake()
        {
            if (PlayerPrefab == null)
            {
                throw new ArgumentException("PlayerPrefab");
            }

            _eventAggregator = DependencyResolver.Current.GetService<IEventAggregator>();
            _gameSettings = DependencyResolver.Current.GetService<GameSettings>();
            _playersEventSubscriptionToken = _eventAggregator.Subscribe(new Action<PlayersEvent>(OnPlayersEvent));
            _cameraFollow = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraFollowCurrentPlayer>();
        }

        public void Start()
        {
        }

        public void Update()
        {
            lock (_playersUpdateQueue)
            {
                var updateCamera = _playersUpdateQueue.Any(b => b.Blob.Name == _gameSettings.UserName);

                while (_playersUpdateQueue.Count > 0)
                {
                    var blobToUpdate = _playersUpdateQueue.Dequeue();
                    var blob = blobToUpdate.Blob;
                    var worldSize = blobToUpdate.WorldSize;

                    switch (blobToUpdate.EventType)
                    {
                        case BlobEventType.BlobAdded:
                            AddNewPlayer(blob, worldSize);
                            break;
                        case BlobEventType.BlobUpdated:
                            UpdatePlayer(blob, worldSize);
                            break;
                        case BlobEventType.BlobRemoved:
                            RemovePlayer(blob);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }

                if (!updateCamera && _cameraFollow != null)
                {
                    return;
                }

                var playerObjects = _playersObjects.Values.Where(b => b.PlayerBlobDto.Name == _gameSettings.UserName);
                _cameraFollow.PlayerBlobs = playerObjects;
            }
        }

        public void OnDestroy()
        {
            if (_eventAggregator != null && _playersEventSubscriptionToken != null)
            {
                _eventAggregator.Unsubscribe<PlayersEvent>(_playersEventSubscriptionToken);
            }
        }

        private void OnPlayersEvent(PlayersEvent playersEvent)
        {
            var worldSize = playersEvent.WorldSize;
            var eventType = playersEvent.EventType;

            lock (_playersUpdateQueue)
            {
                foreach (var blob in playersEvent.Blobs)
                {
                    _playersUpdateQueue.Enqueue(new BlobToUpdate(blob, eventType, worldSize));
                }
            }
        }

        private void AddNewPlayer(BlobDto player, int worldSize)
        {
            var positionAndScale = PlayerPrefab.BlobPositioning.GetBlobPositionAndScale(player, worldSize);
            var playerObject = (Player)Instantiate(PlayerPrefab, positionAndScale.Position, Quaternion.identity);
            playerObject.transform.localScale = positionAndScale.Scale;

            float h, s, v;

            var playerColor = Color.green;
            Color.RGBToHSV(playerColor, out h, out s, out v);
            var innerCircleColor = Color.HSVToRGB(h * 0.9f, s, v);
            var borderColor = Color.HSVToRGB(h, s, v);

            playerObject.UpdatePlayer(player, worldSize, innerCircleColor, borderColor);
            SavePlayer(playerObject, player);
        }

        private void SavePlayer(Player playerObject, BlobDto player)
        {
            var id = player.Id;
            if (_playersObjects.ContainsKey(id))
            {
                RemovePlayer(player);
            }

            _playersObjects.Add(id, playerObject);
        }

        private void RemovePlayer(BlobDto playerDto)
        {
            var id = playerDto.Id;
            if (_playersObjects.ContainsKey(id))
            {
                var playerObject = _playersObjects[id];
                Destroy(playerObject.gameObject);
            }

            _playersObjects.Remove(id);
        }

        private void UpdatePlayer(BlobDto playerDto, int worldSize)
        {
            if (_playersObjects.ContainsKey(playerDto.Id))
            {
                _playersObjects[playerDto.Id].UpdatePlayer(playerDto, worldSize);
            }
        }
    }
}