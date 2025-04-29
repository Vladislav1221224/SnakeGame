using Game.Core;
using Game.Gameplay;

using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UIElements;

namespace Game.Food
{
    public class FoodManager : MonoBehaviour, IObjectPool<FoodController>
    {





        #region Properties

        #region Bindings

        [Header("Bindings")]
        [SerializeField] private FoodController foodPrefab;
        [SerializeField] private Transform foodParent;
        [SerializeField] private BoxCollider2D _foodSpawnArea;
        [SerializeField] ParticleSystemController _foodParticles;
        [SerializeField] AudioSource _eatFoodAudio;

        #endregion

        #region Options

        [SerializeField] int _startFoodCount = 15;
        [SerializeField] float _minDistance = 10f;

        #endregion

        #region Cache

        private ObjectPool<FoodController> _foodPool;

        Vector2 center;
        Vector2 size;

        float randomX;
        float randomY;
        Vector2 randomDot;

        #endregion

        #endregion

        #region Methods

        #region Unity methods
        private void Awake()
        {
            _foodPool = new ObjectPool<FoodController>(
                CreateFood,
                OnTakeFoodFromPool,
                OnReturnFoodToPool,
                OnDestroyFood,
                true, // CollectionCheck
                35, // DefaultCapacity
                100 // MaxSize
            );

            Bootstrap.OnInitAction += Init;
        }
        private void OnDestroy()
        {
            Bootstrap.OnInitAction -= Init;
        }
        private void Update()
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                _eatFoodAudio.PlayOneShot(_eatFoodAudio.clip);
            }
        }

        #endregion
        #region Initialization
        void Init()
        {
            for (int i = 0; i < _startFoodCount; i++)
            {
                Get();
            }
        }

        #endregion

        #region Object pool

        private FoodController CreateFood()
        {
            var food = Instantiate(foodPrefab, foodParent);
            food.gameObject.SetActive(false);
            return food;
        }

        private void OnTakeFoodFromPool(FoodController food)
        {
            Vector2 snakePosition = GameManager.Instance.CurrentSnake.transform.position;

            Vector2 randomPosition = GetRandomPointInBox(_foodSpawnArea, snakePosition, _minDistance);

            food.transform.position = randomPosition;

            food.gameObject.SetActive(true);
        }

        private void OnReturnFoodToPool(FoodController food)
        {
            food.gameObject.SetActive(false);
            _foodParticles.Play(food.transform.position);
            _eatFoodAudio.PlayOneShot(_eatFoodAudio.clip);
        }

        private void OnDestroyFood(FoodController food)
        {
            Destroy(food.gameObject);
        }

        // Реалiзацiя IObjectPool<FoodController>

        public int CountInactive => _foodPool.CountInactive;

        public void Clear()
        {
            _foodPool.Clear();
        }

        public FoodController Get()
        {
            return _foodPool.Get();
        }

        public PooledObject<FoodController> Get(out FoodController v)
        {
            return _foodPool.Get(out v);
        }

        public void Release(FoodController element)
        {
            _foodPool.Release(element);

            Get();
        }

        #endregion

        #endregion
        #region Other
        private Vector2 GetRandomPointInBox(BoxCollider2D box, Vector2 position, float distance)
        {
            center = box.bounds.center;
            size = box.bounds.size;

            do
            {
                randomX = Random.Range(center.x - size.x / 2f, center.x + size.x / 2f);
                randomY = Random.Range(center.y - size.y / 2f, center.y + size.y / 2f);

                randomDot = new Vector2(randomX, randomY);
            }
            while (Vector2.Distance(randomDot, position) < distance);

            return randomDot;
        }

        #endregion
    }
}