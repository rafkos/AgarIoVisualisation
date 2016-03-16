using Assets.Scripts.ServerConnection;
using Assets.Scripts.Services;
using Assets.Scripts.UI;
using UnityEngine;

namespace Assets.Scripts.Game.Camera
{
    public class CameraFreeFly : MonoBehaviour, IMonoBehaviour
    {
        private GameSettings _gameSettings;
        public float MoveSpeed = 10;
        public float ZoomSpeed = 50;

        public void Awake()
        {
            _gameSettings = DependencyResolver.Current.GetService<GameSettings>();
        }

        public void Start()
        {
        }

        public void Update()
        {
            if (!_gameSettings.IsFlyCameraEnabled)
            {
                return;
            }

            transform.position += transform.up * MoveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;
            transform.position += transform.right * MoveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime;
            transform.position += transform.forward * ZoomSpeed * Input.GetAxis("Mouse ScrollWheel") * Time.deltaTime;
        }

        public void OnDestroy()
        {
        }
    }
}
