using System;
using AgarIo.Contract;
using Assets.Scripts.ServerConnection;
using Assets.Scripts.Services;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class Player : MonoBehaviour, IMonoBehaviour
    {
        public Renderer Renderer;
        public TextMesh TextMesh;
        public BlobPositioning BlobPositioning;
        public float ScaleChangeSpeed = 2.0f;
        public float PositionChangeSpeed = 2.0f;

        public BlobDto PlayerBlobDto { get; set; }
        private int _worldSize;

        public void Awake()
        {
            if (Renderer == null)
            {
                throw new ArgumentException("Renderer");
            }

            if (TextMesh == null)
            {
                throw new ArgumentException("TextMesh");
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
            if (PlayerBlobDto == null)
            {
                return;
            }

            var positionAndScale = BlobPositioning.GetBlobPositionAndScale(PlayerBlobDto, _worldSize);

            transform.localScale = Vector3.Lerp(transform.localScale, positionAndScale.Scale, ScaleChangeSpeed * Time.deltaTime);
            transform.position = Vector2.Lerp(transform.position, positionAndScale.Position, PositionChangeSpeed * Time.deltaTime);
        }

        public void OnDestroy()
        {
        }

        public void UpdatePlayer(BlobDto playerBlobDto, int worldSize, Color? mainColor = null)
        {
            PlayerBlobDto = playerBlobDto;
            _worldSize = worldSize;

            if (mainColor != null && Renderer.material.color != mainColor)
            {
                Renderer.material.color = mainColor.Value;
            }

            if (TextMesh.text != PlayerBlobDto.Name)
            {
                TextMesh.text = PlayerBlobDto.Name;
            }
        }
    }
}