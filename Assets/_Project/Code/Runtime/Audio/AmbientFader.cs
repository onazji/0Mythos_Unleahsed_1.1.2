using UnityEngine;
using System.Collections;

namespace Mythos.Unleashed.Runtime.Audio
{
    /// <summary>
    /// Handles ambient audio volume fading during transitions.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(AudioSource))]
    public class AmbientFader : MonoBehaviour
    {
        private AudioSource _source;
        private Coroutine _fadeRoutine;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
            if (_source == null)
                Debug.LogWarning("[AmbientFader] Missing AudioSource component.");
        }

        public void FadeOut(float duration = 1f)
        {
            if (_fadeRoutine != null) StopCoroutine(_fadeRoutine);
            _fadeRoutine = StartCoroutine(FadeTo(0f, duration));
        }

        public void FadeIn(float duration = 1f)
        {
            if (_fadeRoutine != null) StopCoroutine(_fadeRoutine);
            _fadeRoutine = StartCoroutine(FadeTo(1f, duration));
        }

        private IEnumerator FadeTo(float targetVolume, float duration)
        {
            if (_source == null) yield break;

            float startVolume = _source.volume;
            float time = 0f;

            while (time < duration)
            {
                time += Time.unscaledDeltaTime;
                _source.volume = Mathf.Lerp(startVolume, targetVolume, time / duration);
                yield return null;
            }

            _source.volume = targetVolume;
        }
    }
}
