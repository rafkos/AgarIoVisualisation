using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.Game
{
    public class CameraFollowCurrentPlayer : MonoBehaviour, IMonoBehaviour
    {
        private Vector3 _velocity = Vector3.zero;
        public float ZoomSpeed = 2.0f;
        public float ZoomFactor = 0.25f;
        public float InitialFieldOfView = 5.0f;
        public float DampTime = 0.15f;

        public IEnumerable<Player> PlayerBlobs { get; set; }

        public void Awake()
        {
        }

        public void Start()
        {
        }

        public void OnDestroy()
        {
        }

        public void Update()
        {
            if (PlayerBlobs == null || !PlayerBlobs.Any())
            {
                return;
            }

            var blobsPositions = PlayerBlobs.Select(p => p.transform).ToList();
            var blobBounds = GetVectorsBounds(blobsPositions);
            
            var cameraComponent = GetComponent<Camera>();
            var point = cameraComponent.WorldToViewportPoint(blobBounds.center);
            var delta = blobBounds.center - cameraComponent.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
            var destination = transform.position + delta;
            var newFieldOfView = InitialFieldOfView * (blobBounds.size.sqrMagnitude / ZoomFactor);

            transform.position = Vector3.SmoothDamp(transform.position, destination, ref _velocity, DampTime);
            cameraComponent.fieldOfView = Mathf.Lerp(cameraComponent.fieldOfView, newFieldOfView, ZoomSpeed * Time.deltaTime);
        }

        public static Bounds GetVectorsBounds(IList<Transform> transforms)
        {
            if (!transforms.Any())
            {
                return new Bounds(Vector3.zero, Vector3.zero);
            }

            if (transforms.Count == 1)
            {
                return new Bounds(transforms.First().position, transforms.First().localScale);
            }

            var bounds = new Bounds(transforms.First().position, transforms.First().localScale);

            foreach (var transform in transforms.Skip(1))
            {
                bounds.Encapsulate(new Bounds(transform.localPosition, transform.localScale));
            }

            return bounds;
        }
    }
}