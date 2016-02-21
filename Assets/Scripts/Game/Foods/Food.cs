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
        }

        public void OnDestroy()
        {
        }

        public void UpdateFood(BlobDto fooBlobDto, int worldSize, Color? color = null)
        {
            if (color != null && Renderer.material.color != color)
            {
                Renderer.material.color = color.Value;
            }
        }
    }
}