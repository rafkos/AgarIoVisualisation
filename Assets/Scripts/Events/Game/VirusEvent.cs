using System.Collections.Generic;
using AgarIo.Contract;

namespace Assets.Scripts.Events.Game
{
    public class VirusEvent : BlobEvent
    {
        public VirusEvent(IEnumerable<BlobDto> blobs, BlobEventType eventType, int worldSize) : base(blobs, eventType, worldSize)
        {
        }
    }
}