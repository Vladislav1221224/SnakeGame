using Game.Core;
using Game.Gameplay;

using UnityEngine;
using UnityEngine.Events;

namespace Game.UI
{
    /// <summary>
    /// Manages text values and other hud functions
    /// </summary>
    public class HUDController : MonoBehaviour
    {
        #region Properties

        #region Events

        [SerializeField] public UnityEvent<string> OnChangeSnakeLength;

        #endregion

        #endregion

        #region Methods

        #region Unity methods
        private void Awake()
        {
            Bootstrap.OnInitAction += Init;
        }
        private void OnDestroy()
        {
            Bootstrap.OnInitAction -= Init;
        }
        private void OnEnable()
        {
            if (GameManager.Instance)
                GameManager.Instance.OnChangeSnakeLengthEvent.AddListener(SetSnakeLength);
        }
        private void OnDisable()
        {
            GameManager.Instance.OnChangeSnakeLengthEvent.RemoveListener(SetSnakeLength);
        }

        #endregion

        #region Initialization

        void Init()
        {

            if (!GameManager.Instance) return;

            Bootstrap.OnInitAction -= Init;

            GameManager.Instance.OnChangeSnakeLengthEvent.AddListener(SetSnakeLength);
        }

        #endregion

        #region Setters
        public void SetSnakeLength(int length)
        {
            OnChangeSnakeLength?.Invoke(length.ToString());
        }
        #endregion

        #endregion
    }
}