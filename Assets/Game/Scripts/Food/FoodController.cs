using UnityEngine;
using UnityEngine.Pool;

namespace Game.Food
{
    public class FoodController : MonoBehaviour
    {
        #region Properties

        #region Bindings

        [SerializeField] Transform _foodTransform;

        #endregion

        #region Options
        [Header("Proporties")]
        [SerializeField] int _points = 1;
        public int Points => _points;

        [SerializeField] int _maxPoints = 5;
        [SerializeField, Min(1)] int _minPoints = 1;

        #endregion

        #endregion

        #region Methods

        #region Unity methods
        private void OnEnable()
        {
            Init();
        }
        #endregion

        #region Initialization

        void Init()
        {
            _points = Random.Range(_minPoints, _maxPoints);
            _foodTransform.localScale = Vector3.one * _points;
        }

        #endregion

        #endregion
    }
}