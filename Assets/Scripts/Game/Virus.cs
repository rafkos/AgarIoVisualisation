using System;
using AgarIo.Contract;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class Virus : MonoBehaviour, IMonoBehaviour
    {
        public Renderer Renderer;
        public BlobPositioning BlobPositioning;
        public float ScaleChangeSpeed = 2.0f;
        public float PositionChangeSpeed = 2.0f;
        public float RotationZ = 1.0f;

        private BlobDto _virusBlobDto;
        private int _worldSize;

        public void Awake()
        {
            if (Renderer == null)
            {
                throw new ArgumentException("Renderer");
            }

            if (BlobPositioning == null)
            {
                throw new ArgumentException("BlobPositioning");
            }
        }


        public void Start()
        {
        }

        public void Update()
        {
            if (_virusBlobDto == null)
            {
                return;
            }

            var positionAndScale = BlobPositioning.GetBlobPositionAndScale(_virusBlobDto, _worldSize);

            if (positionAndScale.Scale != transform.localScale)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, positionAndScale.Scale, ScaleChangeSpeed * Time.deltaTime);
            }

            if (positionAndScale.Position != transform.position)
            {
                transform.position = Vector2.Lerp(transform.position, positionAndScale.Position, PositionChangeSpeed * Time.deltaTime);
            }

            transform.Rotate(0, 0, RotationZ * Time.deltaTime);
        }

        public void OnDestroy()
        {
        }

        public void UpdateVirus(BlobDto virusBlobDto, int worldSize)
        {
            _virusBlobDto = virusBlobDto;
            _worldSize = worldSize;
        }
    }
}
