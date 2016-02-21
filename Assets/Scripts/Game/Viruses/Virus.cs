using System;
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
            transform.Rotate(0, 0, RotationZ * Time.deltaTime);
        }

        public void OnDestroy()
        {
        }
    }
}
