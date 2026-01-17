using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Mythos.Unleashed.Runtime.SceneFlow
{
    /// <summary>
    /// Handles scene loading with optional fade transitions, ambient audio fades, and a loading screen.
    /// </summary>
    [DisallowMultipleComponent]
    public class SceneLoader : MonoBehaviour
    {
        public static SceneLoader Instance { get; private set; }

        [Header("Scene Settings")]
        [SerializeField] private string loadingSceneName = "Loading";

        [Header("Fade Settings")]
        [SerializeField] private float fadeOutDuration = 0.8f;
        [SerializeField] private float fadeInDelay = 0.2f;
        [SerializeField] private float minLoadingScreenTime = 1.5f;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// Loads a scene by name with a fade-out, loading screen, and fade-in.
        /// </summary>
        public void Load(string targetScene)
        {
            StartCoroutine(LoadRoutine(targetScene));
        }

        private IEnumerator LoadRoutine(string targetScene)
        {
            // Step 1: Fade out visuals and ambience
            var fade = FindObjectOfType<Mythos.Unleashed.Runtime.UI.SceneFadeController>();
            if (fade != null)
            {
                fade.BeginFadeOut();

                // If ambient fader exists, fade it too
                var ambient = FindObjectOfType<Mythos.Unleashed.Runtime.Audio.AmbientFader>();
                if (ambient != null)
                    ambient.FadeOut(fadeOutDuration);

                yield return new WaitForSecondsRealtime(fadeOutDuration);
            }

            // Step 2: Load the loading scene
            yield return SceneManager.LoadSceneAsync(loadingSceneName, LoadSceneMode.Single);

            // Optional: Fade-in loading screen gently
            fade = FindObjectOfType<Mythos.Unleashed.Runtime.UI.SceneFadeController>();
            if (fade != null)
                fade.BeginFadeIn();

            // Step 3: Begin async load of target
            AsyncOperation op = SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Single);
            op.allowSceneActivation = false;

            float timer = 0f;
            while (!op.isDone)
            {
                timer += Time.unscaledDeltaTime;

                if (op.progress >= 0.9f && timer >= minLoadingScreenTime)
                {
                    op.allowSceneActivation = true;
                }

                yield return null;
            }

            // Step 4: Fade back in (visual + audio)
            yield return new WaitForSecondsRealtime(fadeInDelay);

            fade = FindObjectOfType<Mythos.Unleashed.Runtime.UI.SceneFadeController>();
            if (fade != null)
                fade.BeginFadeIn();

            var ambientIn = FindObjectOfType<Mythos.Unleashed.Runtime.Audio.AmbientFader>();
            if (ambientIn != null)
                ambientIn.FadeIn(fadeOutDuration);

            Debug.Log($"[SceneLoader] Scene '{targetScene}' loaded successfully with full fade and ambience transitions.");
        }
    }
}
