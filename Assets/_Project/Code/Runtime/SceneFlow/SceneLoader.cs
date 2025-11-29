using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    [SerializeField] private string loadingSceneName = "Loading";

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Loads a scene by name with a loading screen in between.
    /// </summary>
    public void Load(string targetScene)
    {
        StartCoroutine(LoadRoutine(targetScene));
    }

    private IEnumerator LoadRoutine(string targetScene)
    {
        // 1) Load the Loading scene
        yield return SceneManager.LoadSceneAsync(loadingSceneName, LoadSceneMode.Single);

        // Optional: small delay so the loading scene actually displays
        yield return null;

        // 2) Begin async load of target
        AsyncOperation op = SceneManager.LoadSceneAsync(targetScene, LoadSceneMode.Single);
        op.allowSceneActivation = false;

        // 3) Wait until ready (fake a tiny minimum time so "Loading…" shows)
        float minShowTime = 0.25f;
        float t = 0f;

        while (!op.isDone)
        {
            t += Time.unscaledDeltaTime;

            // Unity uses 0..0.9 for progress until it’s ready
            bool ready = op.progress >= 0.9f && t >= minShowTime;
            if (ready)
            {
                op.allowSceneActivation = true;
            }
            yield return null;
        }
    }
}
