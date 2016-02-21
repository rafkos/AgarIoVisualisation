using System;
using AgarIo.Contract;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class Player : MonoBehaviour, IMonoBehaviour
    {
        public Renderer BorderRenderer;
        public Renderer InnerCircleRenderer;
        public TextMesh TextMesh;
        public BlobPositioning BlobPositioning;
        public float TextLengthCharacterSizeRatio = -36.0f;
        public float TextLengthCharacterSizeOffset = 17.5f;
        public float ScaleChangeSpeed = 2.0f;
        public float PositionChangeSpeed = 2.0f;

        public BlobDto PlayerBlobDto { get; set; }
        private int _worldSize;

        public void Awake()
        {
            if (BorderRenderer == null)
            {
                throw new ArgumentException("BorderRenderer");
            }

            if (InnerCircleRenderer == null)
            {
                throw new ArgumentException("InnerCircleRenderer");
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

        public void UpdatePlayer(BlobDto playerBlobDto, int worldSize)
        {
            PlayerBlobDto = playerBlobDto;
            _worldSize = worldSize;

            var playerName = PlayerBlobDto.Name;
            if (TextMesh.text == playerName)
            {
                return;
            }

            TextMesh.characterSize = playerName.ToCharArray().Length * TextLengthCharacterSizeRatio + TextLengthCharacterSizeOffset;
            TextMesh.text = playerName;
        }

        public void UpdatePlayer(BlobDto playerBlobDto, int worldSize, Color mainColor, Color borderColor)
        {
            UpdatePlayer(playerBlobDto, worldSize);

            InnerCircleRenderer.material.color = mainColor;
            BorderRenderer.material.color = borderColor;
        }
    }
}