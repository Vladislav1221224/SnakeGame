using Game.Core;
using Game.Food;
using Game.Player;

using UnityEngine;
using UnityEngine.Events;

namespace Game.Gameplay
{
    public class GameManager : MonoBehaviour
    {

        #region Properties

        #region Static 

        //Singleton
        static GameManager _instance;
        public static GameManager Instance => _instance;

        #endregion

        #region Bindings

        [Header("Bindings")]

        [SerializeField] Snake _currentSnake;
        public Snake CurrentSnake => _currentSnake;
        [SerializeField] FoodManager _foodManager;
        public FoodManager FoodManager => _foodManager;


        #endregion

        #region Values

        [SerializeField] int _snakeLength = 0;

        #endregion

        #region Events

        [SerializeField] public UnityEvent<int> OnChangeSnakeLengthEvent;

        #endregion

        #endregion

        #region Methods

        #region Unity methods

        private void Awake()
        {
            if (_instance)
            {
                Destroy(gameObject);
                return;
            }
            _instance = this;
            Bootstrap.OnAwakeAction += Init;
        }
        private void OnDestroy()
        {
            _currentSnake.OnChangeLengthAction -= SetPoints;
            Bootstrap.OnAwakeAction -= Init;
        }

        #endregion

        #region Initialization
        public void Init()
        {
            Bootstrap.OnAwakeAction -= Init;
            if (_currentSnake)
                _currentSnake.OnChangeLengthAction += SetPoints;
        }
        #endregion

        #region Setters

        void SetPoints(int points)
        {
            _snakeLength = points;

            OnChangeSnakeLengthEvent?.Invoke(_snakeLength);
        }

        public void SetSnake(Snake snake)
        {
            if (snake == null) return;

            //Don't forget to unsubscribe from the event)))
            if (_currentSnake)
                _currentSnake.OnChangeLengthAction -= SetPoints;

            _currentSnake = snake;
            _currentSnake.OnChangeLengthAction += SetPoints;
        }

        #endregion

        #endregion
    }
}