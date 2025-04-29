using UnityEngine;
using UnityEngine.Events;

namespace Game.Core
{
    public class Bootstrap : MonoBehaviour
    {
        #region Properties


        #region Actions

        public static UnityAction OnAwakeAction;

        public static UnityAction OnInitAction;

        public static UnityAction OnStartAction;

        #endregion
        #endregion

        #region Methods

        #region Unity methods

        public void Awake()
        {
            OnAwakeAction?.Invoke();

            OnInitAction?.Invoke();
        }
        public void Start()
        {
            OnStartAction?.Invoke();
        }

        private void Update()
        {

        }

        #endregion


        // TODO: Ваші власні методи

        #endregion
    }
}