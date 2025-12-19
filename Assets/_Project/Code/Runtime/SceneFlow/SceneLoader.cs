using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

namespace Mythos.Unleashed.Runtime.SceneFlow
{
    /// <summary>
    /// Handles scene loading with optional fade transitions and a loading scene.
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
            // Step 1: Fade out (if controller exists)
            var fade = FindObjectOfType<Mythos.Unleashed.Runtime.UI.SceneFadeController>();
            if (fade != null)
            {
                fade.BeginFadeOut();
                yield return new WaitForSecondsRealtime(fadeOutDuration);
            }

            // Step 2: Load the Loading scene
            yield return SceneManager.LoadSceneAsync(loadingSceneName, LoadSceneMode.Single);

            // Optional delay to ensure "Loading" shows up
            yield return new WaitForSecondsRealtime(0.2f);

            // Step 3: Begin async load of target scene
            AsyncOperation op = SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Single);
            op.allowSceneActivation = false;

            float minShowTime = 0.5f;
            float timer = 0f;

            while (!op.isDone)
            {
                timer += Time.unscaledDeltaTime;

                // Unity reports progress up to 0.9 before activation
                bool ready = op.progress >= 0.9f && timer >= minShowTime;
                if (ready)
                {
                    op.allowSceneActivation = true;
                }

                yield return null;
            }

            yield return new WaitForEndOfFrame();

            // Step 4: Fade back in once new scene is active
            fade = FindObjectOfType<Mythos.Unleashed.Runtime.UI.SceneFadeController>();
            if (fade != null)
            {
                yield return new WaitForSecondsRealtime(fadeInDelay);
                fade.BeginFadeIn();
            }

            Debug.Log($"[SceneLoader] Scene '{targetScene}' loaded successfully with fade transitions.");
        }
    }
}
