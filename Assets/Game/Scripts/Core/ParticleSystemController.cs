using System.Collections;
using System.Collections.Generic;

using UnityEngine;


namespace Game.Core
{
    [RequireComponent(typeof(ParticleSystem))]
    public class ParticleSystemController : MonoBehaviour
    {
        #region Properties

        #region Bindings

        ParticleSystem _particleSystem;

        #endregion

        #region Cache

        Queue<Vector3> _queuePosition = new();

        #endregion

        #endregion

        #region Methods

        #region Unity methods

        private void Awake()
        {
            _particleSystem = GetComponent<ParticleSystem>();
        }

        private void Start()
        {

        }

        private void Update()
        {

        }
        void OnEnable()
        {
            StartCoroutine(ParticlePlayQueue());
        }
        void OnDisable()
        {
            StopAllCoroutines();
        }

        #endregion

        #region Play queue management

        IEnumerator ParticlePlayQueue()
        {
            while (true)
            {
                yield return null;

                if (_queuePosition.Count == 0) continue;

                Vector3 position = _queuePosition.Dequeue();

                _particleSystem.transform.position = position;

                _particleSystem.Play();
            }
        }

        #region Public

        public void Play(Vector3 position)
        {
            _queuePosition.Enqueue(position);
        }

        #endregion

        #endregion



        #endregion
    }
}