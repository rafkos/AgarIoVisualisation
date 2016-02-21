using AgarIo.Contract;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class BlobPositioning : MonoBehaviour, IMonoBehaviour
    {
        public float BlobScaleFactor = 0.5f;

        public PositionScaleTuple GetBlobPositionAndScale(BlobDto blob, int worldSize)
        {
            var radius = (float)(blob.Radius / worldSize) * BlobScaleFactor;
            var xPosition = (float)blob.Position.X / worldSize;
            var yPosition = (float)blob.Position.Y / worldSize;
            var newScale = new Vector3(radius, radius, 1.0f);
            var newPosition = new Vector2(xPosition, yPosition);

            return new PositionScaleTuple { Position = newPosition, Scale = newScale };
        }

        public void Awake()
        {
        }

        public void Start()
        {
        }

        public void Update()
        {
        }

        public void OnDestroy()
        {
        }
    }
}