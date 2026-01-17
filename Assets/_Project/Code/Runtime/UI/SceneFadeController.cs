using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mythos.Unleashed.Runtime.Audio;

namespace Mythos.Unleashed.Runtime.UI
{
    /// <summary>
    /// Handles fade in/out transitions during scene loads using the existing HUDCanvas_Persistent.
    /// The FadePanel must contain a CanvasGroup for opacity control.
    /// Also syncs ambient audio fades for smoother transitions.
    /// </summary>
    [DisallowMultipleComponent]
    public class SceneFadeController : MonoBehaviour
    {

        [SerializeField, Tooltip("Optional link to ambient fader for sound transitions.")]
        private Mythos.Unleashed.Runtime.Audio.AmbientFader ambientFader;

        [Header("Fade Components")]
        [SerializeField, Tooltip("Reference to the CanvasGroup on the FadePanel.")]
        private CanvasGroup _fadeGroup;

        [Header("Fade Settings")]
        [SerializeField, Tooltip("Duration of fade in seconds.")]
        private float _fadeDuration = 1.0f;

        [SerializeField, Tooltip("Optional delay before fade out starts.")]
        private float _delayBeforeFadeOut = 0.25f;

        private Coroutine _fadeRoutine;

        private void Awake()
        {
            if (_fadeGroup == null)
                Debug.LogWarning("[SceneFade] CanvasGroup reference missing.");
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += HandleSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= HandleSceneLoaded;
        }

        private void HandleSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Fade in after new scene loads
            if (_fadeRoutine != null)
                StopCoroutine(_fadeRoutine);
            _fadeRoutine = StartCoroutine(Fade(0f, 0.25f));

            // 🔊 Also fade in ambient sound
            var ambient = FindObjectOfType<AmbientFader>();
            if (ambient != null)
                ambient.FadeIn(_fadeDuration);
        }

        public void BeginFadeOut()
        {
            StartCoroutine(Fade(1f));

            // 🔊 Fade out ambient sound
            var ambient = FindObjectOfType<AmbientFader>();
            if (ambient != null)
                ambient.FadeOut(_fadeDuration * 0.9f);
        }

        public void BeginFadeIn()
        {
            StartCoroutine(Fade(0f));

            // 🔊 Fade in ambient sound
            var ambient = FindObjectOfType<AmbientFader>();
            if (ambient != null)
                ambient.FadeIn(_fadeDuration);
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
