using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace Mythos.Unleashed.Runtime.Audio
{
    /// <summary>
    /// Handles ambient audio volume fading during transitions.
    /// Automatically disables ambience when leaving its scene.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AudioSource))]
    public class AmbientFader : MonoBehaviour
    {
        private AudioSource _source;
        private Coroutine _fadeRoutine;

        [Header("Additional Ambient Sources")]
        [SerializeField] private AudioSource[] ambientSources;

        [SerializeField, Range(0.1f, 5f)] private float fadeDuration = 2.0f;

        private string _originScene;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
            if (_source == null)
                Debug.LogWarning("[AmbientFader] Missing AudioSource component.");

            // Remember which scene this ambience belongs to
            _originScene = SceneManager.GetActiveScene().name;

            // Subscribe to scene change event
            SceneManager.sceneLoaded += OnSceneChanged;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnSceneChanged;
        }

        private void OnSceneChanged(Scene newScene, LoadSceneMode mode)
        {
            // If we’ve left the scene this ambience belongs to, fade it out and destroy it
            if (newScene.name != _originScene)
            {
                StartCoroutine(FadeOutAndDestroy());
            }
        }

        private IEnumerator FadeOutAndDestroy()
        {
            if (_source != null)
                yield return StartCoroutine(FadeOut(_source));

            foreach (var src in ambientSources)
                if (src != null)
                    yield return StartCoroutine(FadeOut(src));

            Destroy(gameObject, 0.25f);
        }

        // Your existing FadeIn/FadeOut/FadeTo methods stay the same
        public void FadeOut(float duration = 1f)
        {
            if (_fadeRoutine != null) StopCoroutine(_fadeRoutine);
            _fadeRoutine = StartCoroutine(FadeTo(0f, duration));

            foreach (var src in ambientSources)
                if (src != null) StartCoroutine(FadeOut(src));
        }

        public void FadeIn(float duration = 1f)
        {
            if (_fadeRoutine != null) StopCoroutine(_fadeRoutine);
            _fadeRoutine = StartCoroutine(FadeTo(1f, duration));

            foreach (var src in ambientSources)
                if (src != null) StartCoroutine(FadeIn(src));
        }

        private IEnumerator FadeTo(float targetVolume, float duration)
        {
            if (_source == null) yield break;

            float startVolume = _source.volume;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                elapsed += Time.unscaledDeltaTime;
                _source.volume = Mathf.Lerp(startVolume, targetVolume, elapsed / duration);
                yield return null;
            }

            _source.volume = targetVolume;
        }

        public IEnumerator FadeIn(AudioSource source)
        {
            float t = 0f;
            while (t < fadeDuration)
            {
                t += Time.unscaledDeltaTime;
                source.volume = Mathf.Lerp(0f, 0.3f, t / fadeDuration);
                yield return null;
            }
            source.volume = 0.3f;
        }

        public IEnumerator FadeOut(AudioSource source)
        {
            float startVolume = source.volume;
            float elapsed = 0f;

            while (elapsed < fadeDuration)
            {
                elapsed += Time.unscaledDeltaTime;
                source.volume = Mathf.Lerp(startVolume, 0f, elapsed / fadeDuration);
                yield return null;
            }

            source.volume = 0f;
        }
    }
}
