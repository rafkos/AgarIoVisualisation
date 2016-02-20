using System.Collections.Generic;
using AgarIo.Contract;

namespace Assets.Scripts.Events.Game
{
    public class FoodEvent : BlobEvent
    {
        public FoodEvent(IEnumerable<BlobDto> blobs, BlobEventType eventType, int worldSize) : base(blobs, eventType, worldSize)
        {
        }
    }
}