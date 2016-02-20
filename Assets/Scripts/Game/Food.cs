using System;
using AgarIo.Contract;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class Food : MonoBehaviour, IMonoBehaviour
    {
        public Renderer Renderer;
        public BlobPositioning BlobPositioning;
        public float ScaleChangeSpeed = 2.0f;
        public float PositionChangeSpeed = 2.0f;

        private BlobDto _foodBlobDto;
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
            if (_foodBlobDto == null)
            {
                return;
            }

            var positionAndScale = BlobPositioning.GetBlobPositionAndScale(_foodBlobDto, _worldSize);

            if (positionAndScale.Scale != transform.localScale)
            {
                transform.localScale = Vector3.Lerp(transform.localScale, positionAndScale.Scale, ScaleChangeSpeed * Time.deltaTime);
            }

            if (positionAndScale.Position != transform.localPosition)
            {
                transform.position = Vector2.Lerp(transform.position, positionAndScale.Position, PositionChangeSpeed * Time.deltaTime);
            }
        }

        public void OnDestroy()
        {
        }

        public void UpdateFood(BlobDto fooBlobDto, int worldSize, Color? color = null)
        {
            _foodBlobDto = fooBlobDto;
            _worldSize = worldSize;

            if (color != null && Renderer.material.color != color)
            {
                Renderer.material.color = color.Value;
            }
        }
    }
}