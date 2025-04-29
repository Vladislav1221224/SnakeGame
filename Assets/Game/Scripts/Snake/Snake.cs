using Game.Core;
using Game.Food;
using Game.Gameplay;

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

namespace Game.Player
{
    public class Snake : MonoBehaviour
    {
        #region Proporties

        #region Static

        #endregion

        #region Bindings

        [Header("Bindings")]
        [SerializeField] Transform _tailPrefab;

        [SerializeField] Transform _head;

        [SerializeField] Transform _tailParent;

        [SerializeField] ColliderTrigger _triggerHead;

        [SerializeField] AudioSource _boostAudio;
        [SerializeField] AudioSource _unboostAudio;
        #endregion

        #region Settings

        [Header("Proporties")]

        [SerializeField] float _speed = 6f;
        [SerializeField] float _boostSpeed = 10f;
        [SerializeField] float _angularRotationSpeed = 10f;
        [SerializeField] float _angularRotationSpeedBoost = 5f;
        [SerializeField] float _tailStrength = 2f;
        [SerializeField] int _startTailLength = 5;

        #endregion

        #region Events

        public UnityAction<int> OnChangeLengthAction;

        #endregion

        #region Cache

        float _currentSpeed;
        Vector2 _currentDirection;

        [SerializeField] List<Transform> _tail = new();

        [SerializeField] bool _isBoost = false;

        #endregion

        #endregion

        #region Methods

        #region Unity methods
        private void Awake()
        {
            Bootstrap.OnInitAction += Init;
            _triggerHead.OnTriggerEnter += OnTriggerEnterHandler;
        }

        private void Update()
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            MoveHeadToDestination(mousePosition);
            UpdateTail();
        }

        private void OnDestroy()
        {
            Bootstrap.OnInitAction -= Init;
            _triggerHead.OnTriggerEnter -= OnTriggerEnterHandler;
        }

        #endregion

        #region Initialization

        void Init()
        {
            Bootstrap.OnInitAction -= Init;
            _tailParent.SetParent(transform.root);
            for (int i = 0; i < _startTailLength; i++)
                Add();
        }
        #endregion

        #region Food management

        public void OnTriggerEnterHandler(Collider2D collider)
        {
            if (collider == null) return;

            FoodController food = collider.gameObject.GetComponent<FoodController>();

            if (food == null) return;
            int points = food.Points;

            GameManager.Instance.FoodManager.Release(food);

            for (int i = 0; i < points; i++)
            {
                Add();
            }
        }

        #endregion

        #region Tail management
        public void Add()
        {
            Transform tailSection = Instantiate(_tailPrefab, _tailParent);
            tailSection.SetAsFirstSibling();

            tailSection.position = _tail.Count > 0 ? _tail[_tail.Count - 1].position : _tailParent.position;

            SpriteRenderer renderer = tailSection.GetComponent<SpriteRenderer>();
            if (renderer)
                renderer.sortingOrder = -_tail.Count;
            _tail.Add(tailSection);

            OnChangeLengthAction?.Invoke(_tail.Count);
        }
        public void Remove()
        {
            Transform tailSection = _tail[_tail.Count - 1];

            _tail.RemoveAt(_tail.Count - 1);

            Destroy(tailSection.gameObject);

            OnChangeLengthAction?.Invoke(_tail.Count);
        }

        /// <summary>
        /// Removes range of tail sections
        /// </summary>
        /// <param name="count">How many tail sections remove</param>
        public void Remove(int count)
        {
            if (count < 1 || count > _tail.Count)
                return;

            int startIndex = _tail.Count - count;

            for (int i = startIndex; i < _tail.Count; i++)
            {
                if (_tail[i] != null)
                    Destroy(_tail[i].gameObject);
            }

            _tail.RemoveRange(startIndex, count);

            OnChangeLengthAction?.Invoke(_tail.Count);
        }
        #endregion

        #region Movement

        #region Head

        /// <summary>
        /// Rotates head to destination. Works by fixedDeltaTime
        /// </summary>
        public void MoveHeadToDestination(Vector2 destination)
        {
            //Head rotation
            Vector2 direction = destination - (Vector2)_head.position;

            float angleToDirection = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            float currentAngle = _head.rotation.eulerAngles.z;

            float angleDiff = Mathf.DeltaAngle(currentAngle, angleToDirection);

            float maxTurn = _isBoost ? _angularRotationSpeedBoost : _angularRotationSpeed;//Checks is boost

            angleDiff = Mathf.Clamp(angleDiff, -maxTurn, maxTurn);

            float newAngle = currentAngle + angleDiff;

            _head.rotation = Quaternion.RotateTowards(_head.rotation, Quaternion.Euler(0f, 0f, newAngle), maxTurn * Time.deltaTime);

            //Boost logic
            if (_isBoost)
                _currentSpeed = Mathf.Lerp(_currentSpeed, _boostSpeed, 0.1f);
            else
                _currentSpeed = Mathf.Lerp(_currentSpeed, _speed, 0.1f);

            _currentDirection = _head.right;

            _head.position = (Vector2)transform.position + _currentDirection * _currentSpeed * Time.deltaTime;
        }

        #endregion

        #region Tail

        void UpdateTail()
        {
            if (_tail.Count == 0)
                return;

            Vector3 previousPosition = _head.position;
            float angle = 0;
            for (int i = 0; i < _tail.Count; i++)
            {
                Transform segment = _tail[i];
                float distance = Vector3.Distance(segment.position, previousPosition);

                if (distance > 0.1f)
                {
                    Vector3 direction = (previousPosition - segment.position).normalized;

                    segment.position = Vector3.Lerp(segment.position, previousPosition, Time.deltaTime * _currentSpeed * _tailStrength);

                    angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    segment.rotation = Quaternion.Euler(0, 0, angle);
                }

                previousPosition = segment.position;
            }
        }

        #endregion

        #endregion

        #region Setters

        public void SetBoost(bool isBoost)
        {
            if (isBoost == _isBoost) return;

            _isBoost = isBoost;

            if (_isBoost)
                _boostAudio.Play();
            else
                _unboostAudio.Play();
        }
        #endregion

        #endregion
    }
}