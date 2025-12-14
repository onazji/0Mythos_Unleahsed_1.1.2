using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Mythos.Unleashed.Runtime.UI
{
    /// <summary>
    /// Handles fade in/out transitions during scene loads using the existing HUDCanvas_Persistent.
    /// The FadePanel must contain a CanvasGroup for opacity control.
    /// </summary>
    [DisallowMultipleComponent]
    public class SceneFadeController : MonoBehaviour
    {
        [Header("Fade Components")]
        [SerializeField, Tooltip("Reference to the CanvasGroup on the FadePanel.")]
        private CanvasGroup _fadeGroup;

        [Header("Fade Settings")]
        [SerializeField, Tooltip("Duration of fade in seconds.")]
        private float _fadeDuration = 0.8f;

        [SerializeField, Tooltip("Optional delay before fade out starts.")]
        private float _delayBeforeFadeOut = 0.25f;

        private Coroutine _fadeRoutine;

        private void Awake()
        {
            if (_fadeGroup == null)
                Debug.LogWarning("[SceneFade] CanvasGroup reference missing.");
        }

        //Onazji Drayden
        //11/26/25
        /// Public methods to initiate fade in/out
        public void BeginFadeOut() => StartCoroutine(Fade(1f));
        public void BeginFadeIn()  => StartCoroutine(Fade(0f));
        ///
        /// 
        /// 


        private void OnEnable()
        {
          //  SceneLoader.OnSceneLoadBegin += HandleSceneLoadBegin;
           // SceneLoader.OnSceneLoadComplete += HandleSceneLoadComplete;
        }

        private void OnDisable()
        {
            //SceneLoader.OnSceneLoadBegin -= HandleSceneLoadBegin;
            //SceneLoader.OnSceneLoadComplete -= HandleSceneLoadComplete;
        }

        private void HandleSceneLoadBegin()
        {
            if (_fadeRoutine != null) StopCoroutine(_fadeRoutine);
            _fadeRoutine = StartCoroutine(Fade(1f));
        }

        private void HandleSceneLoadComplete()
        {
            if (_fadeRoutine != null) StopCoroutine(_fadeRoutine);
            _fadeRoutine = StartCoroutine(Fade(0f, _delayBeforeFadeOut));
        }

        private IEnumerator Fade(float target, float delay = 0f)
        {
            if (_fadeGroup == null) yield break;

            yield return new WaitForSecondsRealtime(delay);

            float start = _fadeGroup.alpha;
            float elapsed = 0f;

            while (elapsed < _fadeDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                float t = Mathf.Clamp01(elapsed / _fadeDuration);
                _fadeGroup.alpha = Mathf.Lerp(start, target, t);
                yield return null;
            }

            _fadeGroup.alpha = target;
        }
    }
}
