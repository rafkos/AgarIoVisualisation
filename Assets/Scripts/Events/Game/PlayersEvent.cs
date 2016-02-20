using System.Collections.Generic;
using AgarIo.Contract;

namespace Assets.Scripts.Events.Game
{
    public class PlayersEvent : BlobEvent
    {
        public PlayersEvent(IEnumerable<BlobDto> blobs, BlobEventType eventType, int worldSize) : base(blobs, eventType, worldSize)
        {
        }
    }
}