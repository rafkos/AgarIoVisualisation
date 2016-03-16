using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Game.Players;
using Assets.Scripts.ServerConnection;
using Assets.Scripts.Services;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.Game.Camera
{
    public class CameraFollowCurrentPlayer : MonoBehaviour, IMonoBehaviour
    {
        private Vector3 _velocity = Vector3.zero;
        public float ZoomSpeed = 2.0f;
        public float ZoomFactor = 20f;
        public float MultipleBlobsFactor = 0.375f;
        public float DampTime = 0.15f;
        private GameSettings _gameSettings;

        public IEnumerable<Player> PlayerBlobs { get; set; }

        public void Awake()
        {
            _gameSettings = DependencyResolver.Current.GetService<GameSettings>();
        }

        public void Start()
        {
        }

        public void OnDestroy()
        {
        }

        public void Update()
        {
            if (PlayerBlobs == null || !PlayerBlobs.Any() || _gameSettings.IsFlyCameraEnabled)
            {
                return;
            }

            var adjustedZoomFactor = PlayerBlobs.Count() == 1 ? ZoomFactor : ZoomFactor * MultipleBlobsFactor;
            var blobsPositions = PlayerBlobs.Select(p => p.transform).ToList();
            var blobSizes = PlayerBlobs.Select(p => p.BorderRenderer.bounds).ToList();
            var blobBounds = GetVectorsBounds(blobsPositions, blobSizes);

            var cameraComponent = GetComponent<UnityEngine.Camera>();
            var point = cameraComponent.WorldToViewportPoint(blobBounds.center);
            var delta = blobBounds.center - cameraComponent.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
            var destination = transform.position + delta;
            var newFieldOfView = adjustedZoomFactor * (Math.Max(blobBounds.size.x, blobBounds.size.y));

            transform.position = Vector3.SmoothDamp(transform.position, destination, ref _velocity, DampTime);
            cameraComponent.fieldOfView = Mathf.Lerp(cameraComponent.fieldOfView, newFieldOfView, ZoomSpeed * Time.deltaTime);
        }

        public static Bounds GetVectorsBounds(IList<Transform> transforms, IList<Bounds> blobBounds)
        {
            if (transforms.Count != blobBounds.Count)
            {
                throw new ArgumentException("transforms and blobBounds must be of same length.");
            }

            var positionsAndSizes = transforms.Select((t, i) => new PositionAndSizeTuple
            {
                Position = t.position,
                Size = blobBounds[i].size
            }).ToList();

            if (!positionsAndSizes.Any())
            {
                return new Bounds(Vector3.zero, Vector3.zero);
            }

            if (positionsAndSizes.Count == 1)
            {
                return new Bounds(positionsAndSizes.First().Position, positionsAndSizes.First().Size);
            }

            var bounds = new Bounds(positionsAndSizes.First().Position, positionsAndSizes.First().Size);

            foreach (var positionAndSize in positionsAndSizes.Skip(1))
            {
                bounds.Encapsulate(new Bounds(positionAndSize.Position, positionAndSize.Size));
            }

            return bounds;
        }
    }

    public class PositionAndSizeTuple
    {
        public Vector3 Position;
        public Vector3 Size;
    }
}