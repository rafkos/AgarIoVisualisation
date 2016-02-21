using System;
using System.Collections.Generic;
using AgarIo.Contract;
using Assets.Scripts.Events.Game;
using Assets.Scripts.Services;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class VirusController : MonoBehaviour, IMonoBehaviour
    {
        private readonly Dictionary<int, Virus> _virusObjects;
        private readonly Queue<BlobToUpdate> _virusUpdateQueue;
        private IEventAggregator _eventAggregator;
        private SubscriptionToken _virusEventSubscriptionToken;

        public Virus VirusPrefab;

        public VirusController()
        {
            _virusObjects = new Dictionary<int, Virus>();
            _virusUpdateQueue = new Queue<BlobToUpdate>();
        }

        public void Awake()
        {
            if (VirusPrefab == null)
            {
                throw new ArgumentException("VirusPrefab");
            }

            _eventAggregator = DependencyResolver.Current.GetService<IEventAggregator>();
            _virusEventSubscriptionToken = _eventAggregator.Subscribe(new Action<VirusEvent>(OnVirusEvent));
        }

        public void Start()
        {
        }

        public void Update()
        {
            lock (_virusUpdateQueue)
            {
                while (_virusUpdateQueue.Count > 0)
                {
                    var blobToUpdate = _virusUpdateQueue.Dequeue();
                    var blob = blobToUpdate.Blob;
                    var worldSize = blobToUpdate.WorldSize;

                    switch (blobToUpdate.EventType)
                    {
                        case BlobEventType.BlobAdded:
                            AddNewVirus(blob, worldSize);
                            break;
                        case BlobEventType.BlobUpdated:
                            break;
                        case BlobEventType.BlobRemoved:
                            RemoveVirus(blob);
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }

        public void OnDestroy()
        {
            if (_eventAggregator != null && _virusEventSubscriptionToken != null)
            {
                _eventAggregator.Unsubscribe<VirusEvent>(_virusEventSubscriptionToken);
            }
        }

        private void OnVirusEvent(VirusEvent virusEvent)
        {
            var worldSize = virusEvent.WorldSize;
            var eventType = virusEvent.EventType;

            lock (_virusUpdateQueue)
            {
                foreach (var blob in virusEvent.Blobs)
                {
                    _virusUpdateQueue.Enqueue(new BlobToUpdate(blob, eventType, worldSize));
                }
            }
        }

        private void AddNewVirus(BlobDto virus, int worldSize)
        {
            var positionAndScale = VirusPrefab.BlobPositioning.GetBlobPositionAndScale(virus, worldSize);
            var virusObject = (Virus)Instantiate(VirusPrefab, positionAndScale.Position, Quaternion.identity);
            virusObject.transform.localScale = positionAndScale.Scale;
            SaveVirus(virusObject, virus);
        }

        private void SaveVirus(Virus virusObject, BlobDto virus)
        {
            var id = virus.Id;

            if (_virusObjects.ContainsKey(id))
            {
                RemoveVirus(virus);
            }

            _virusObjects.Add(id, virusObject);
        }

        private void RemoveVirus(BlobDto virus)
        {
            var id = virus.Id;

            if (_virusObjects.ContainsKey(id))
            {
                var virusObject = _virusObjects[id];
                Destroy(virusObject.gameObject);
            }

            _virusObjects.Remove(id);
        }
    }
}