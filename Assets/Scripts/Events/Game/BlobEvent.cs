using System.Collections.Generic;
using AgarIo.Contract;
using Assets.Scripts.Services;

namespace Assets.Scripts.Events.Game
{
    public abstract class BlobEvent : IEvent
    {
        protected BlobEvent(IEnumerable<BlobDto> blobs, BlobEventType eventType, int worldSize)
        {
            Blobs = blobs;
            EventType = eventType;
            WorldSize = worldSize;
        }

        public int WorldSize { get; private set; }

        public BlobEventType EventType { get; private set; }

        public IEnumerable<BlobDto> Blobs { get; private set; }
    }
}
