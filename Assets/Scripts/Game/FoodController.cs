using System;
using System.Collections.Generic;
using AgarIo.Contract;
using Assets.Scripts.Events.Game;
using Assets.Scripts.Services;
using Assets.Scripts.UI;
using UnityEngine;
using Random = System.Random;

namespace Assets.Scripts.Game
{
    public class FoodController : MonoBehaviour, IMonoBehaviour
    {
        private readonly Dictionary<int, Food> _foodObjects;
        private readonly Queue<BlobToUpdate> _foodUpdateQueue;
        private readonly Random _random;
        private IEventAggregator _eventAggregator;
        private SubscriptionToken _foodEventSubscriptionToken;

        public Food FoodPrefab;
        public List<Color> FoodColors;

        public FoodController()
        {
            _foodObjects = new Dictionary<int, Food>();
            _foodUpdateQueue = new Queue<BlobToUpdate>();
            _random = new Random();
        }

        public void Awake()
        {
            if (FoodPrefab == null)
            {
                throw new ArgumentException("FoodPrefab");
            }

            if (FoodColors == null)
            {
                throw new ArgumentException("FoodColors");
            }

            _eventAggregator = DependencyResolver.Current.GetService<IEventAggregator>();
            _foodEventSubscriptionToken = _eventAggregator.Subscribe(new Action<FoodEvent>(OnFoodEvent));
        }

        public void Start()
        {
        }

        public void Update()
        {
            lock (_foodUpdateQueue)
            {
                while (_foodUpdateQueue.Count > 0)
                {
                    var blobToUpdate = _foodUpdateQueue.Dequeue();
                    var blob = blobToUpdate.Blob;
                    var worldSize = blobToUpdate.WorldSize;

                    switch (blobToUpdate.EventType)
                    {
                        case BlobEventType.BlobAdded:
                            AddNewFood(blob, worldSize);
                            break;
                        case BlobEventType.BlobUpdated:
                            UpdateFood(blob, worldSize);
                            break;
                        case BlobEventType.BlobRemoved:
                            RemoveFood(blob);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }

        public void OnDestroy()
        {
            _eventAggregator.Unsubscribe<FoodEvent>(_foodEventSubscriptionToken);
        }

        private void OnFoodEvent(FoodEvent foodEvent)
        {
            var worldSize = foodEvent.WorldSize;
            var eventType = foodEvent.EventType;

            lock (_foodUpdateQueue)
            {
                foreach (var blob in foodEvent.Blobs)
                {
                    _foodUpdateQueue.Enqueue(new BlobToUpdate(blob, eventType, worldSize));
                }
            }
        }

        private void AddNewFood(BlobDto food, int worldSize)
        {
            var positionAndScale = FoodPrefab.BlobPositioning.GetBlobPositionAndScale(food, worldSize);
            var foodObject = (Food)Instantiate(FoodPrefab, positionAndScale.Position, Quaternion.identity);
            foodObject.transform.localScale = positionAndScale.Scale;

            var colorIndex = _random.Next(0, FoodColors.Count - 1);
            var foodColor = FoodColors[colorIndex];

            foodObject.UpdateFood(food, worldSize, foodColor);
            SaveFood(foodObject, food);
        }

        private void UpdateFood(BlobDto foodDto, int worldSize)
        {
            if (_foodObjects.ContainsKey(foodDto.Id))
            {
                _foodObjects[foodDto.Id].UpdateFood(foodDto, worldSize);
            }
        }

        private void SaveFood(Food foodObject, BlobDto food)
        {
            var id = food.Id;

            if (_foodObjects.ContainsKey(id))
            {
                RemoveFood(food);
            }

            _foodObjects.Add(id, foodObject);
        }

        private void RemoveFood(BlobDto food)
        {
            var id = food.Id;

            if (_foodObjects.ContainsKey(id))
            {
                var foodObject = _foodObjects[id];
                Destroy(foodObject.gameObject);
            }

            _foodObjects.Remove(id);
        }
    }
}