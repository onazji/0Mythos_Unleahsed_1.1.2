using UnityEngine;
using Mythos.Unleashed.Runtime.UI;
using Mythos.Unleashed.Runtime.SceneFlow; // ✅ Access SceneLoader

public class MainMenuUI : MonoBehaviour
{
    [Header("Scene Settings")]
    [SerializeField] private string nextSceneName = "MuseumHub_WindWard";

    [Header("Fade Reference (Optional)")]
    [SerializeField] private SceneFadeController fadeController;

    private bool _isLoading;

    private void Awake()
    {
        // Automatically find the FadePanel in the persistent Bootstrap
        if (fadeController == null)
            fadeController = FindObjectOfType<SceneFadeController>(true);
    }

    public void StartGame()
    {
        if (_isLoading) return;
        _isLoading = true;

        if (fadeController != null)
        {
            fadeController.BeginFadeOut();
            Invoke(nameof(LoadNextScene), 1.0f); // Wait for fade to complete
        }
        else
        {
            Debug.LogWarning("[MainMenuUI] No FadeController found — loading immediately.");
            LoadNextScene();
        }
    }

    private void LoadNextScene()
    {
        if (SceneLoader.Instance != null)
        {
            SceneLoader.Instance.Load(nextSceneName);
            Debug.Log($"[MainMenuUI] Loading '{nextSceneName}' via SceneLoader.");
        }
        else
        {
            Debug.LogWarning("[MainMenuUI] SceneLoader instance not found — using direct load fallback.");
            UnityEngine.SceneManagement.SceneManager.LoadScene(nextSceneName);
        }
    }
}
