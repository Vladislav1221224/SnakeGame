using UnityEngine;

namespace Game.Player
{
    public class PlayerController : MonoBehaviour
    {
        #region Proporties

        #region Bindings

        [Header("Bindings")]
        [SerializeField] Camera _camera;
        [SerializeField] Transform _playerParent;
        [SerializeField] Snake _snake;

        #endregion

        #region Options

        [Header("Proporties")]
        [SerializeField] float _defaultZoom = 15f;
        [SerializeField] float _boostZoom = 25f;
        [SerializeField] float _zoomLerp = 0.05f;
        [SerializeField] float _cameraMoveLerp = 0.2f;
        #endregion

        #region Cache

        float currentZoom;

        #endregion

        #endregion

        #region Methods

        #region Unity

        void Update()
        {
            if (!_snake || !_playerParent) return;

            Boost(Input.GetKey(KeyCode.LeftShift));

            Move();
        }

        #endregion

        void Boost(bool isBoost)
        {
            if (!_camera) return;

            _snake.SetBoost(isBoost);

            currentZoom = isBoost ? _boostZoom : _defaultZoom;

            _camera.orthographicSize = Mathf.Lerp(_camera.orthographicSize, currentZoom, _zoomLerp);
        }

        /// <summary>
        /// Camera moves to snake
        /// </summary>
        void Move()
        {
            if (!_playerParent || !_snake) return;

            Vector3 newCameraPosition = Vector2.Lerp(_playerParent.position, _snake.transform.position, _cameraMoveLerp);
            newCameraPosition.z = -10;
            _playerParent.position = newCameraPosition;
        }

        #endregion
    }
}