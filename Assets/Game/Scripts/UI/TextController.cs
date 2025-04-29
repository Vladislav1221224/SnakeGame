using DG.Tweening;

using TMPro;

using UnityEngine;

namespace Game.UI.Other
{
    [RequireComponent(typeof(TextMeshProUGUI))]
    public class TextController : MonoBehaviour
    {
        #region Properties

        #region Settings

        [Header("SETTINGS")]
        [SerializeField] string _format = "Text: {0}";
        [SerializeField] float _duration = 1f;

        #endregion

        #region Animations

        [Space(10)]
        [Header("ANIMATIONS")]

        [Header("---Scale---")]
        [SerializeField] AnimationCurve _scaleAnimation;
        [SerializeField] float _strength = 1f;
        [SerializeField] int _vibrato = 2;

        [Space(5)]
        [SerializeField] Vector3 _startScale;
        [SerializeField] Vector3 _endScale;
        [Header("---Alpha---")]
        [SerializeField] AnimationCurve _alphaAnimation;

        [Space(5)]
        [SerializeField] float _startAlpha = 1f;
        [SerializeField] float _endAlpha = 1f;
        #endregion

        #region Cache

        TextMeshProUGUI _text;

        #endregion

        #endregion

        #region Methods

        #region Unity methods

        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }

        private void Start()
        {

        }

        private void Update()
        {

        }

        #endregion
        #region Setters
        public void SetText(string text)
        {
            _text.text = string.Format(_format, text);

            //Animations
            ScaleAnimation();
        }
        public void SetTextWithOutTweens(string text)
        {
            _text.text = string.Format(_format, text);
        }
        #endregion

        #region Animations

        void ScaleAnimation()
        {
            Vector3 scale = _text.transform.localScale;

            _text.transform.DOShakeScale(_duration, _strength, _vibrato, 0)
                .SetEase(_scaleAnimation)
                .OnComplete(() => _text.transform.localScale = Vector3.one);
        }

        #endregion

        #endregion
    }
}