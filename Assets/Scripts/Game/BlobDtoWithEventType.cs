using AgarIo.Contract;
using Assets.Scripts.Events.Game;

namespace Assets.Scripts.Game
{
    public class BlobToUpdate
    {
        public BlobToUpdate(BlobDto blob, BlobEventType eventType, int worldSize)
        {
            Blob = blob;
            EventType = eventType;
            WorldSize = worldSize;
        }

        public BlobDto Blob { get; private set; }

        public BlobEventType EventType { get; private set; }

        public int WorldSize { get; private set; }
    }
}