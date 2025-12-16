using UnityEngine;
using UnityEngine.SceneManagement;
using Mythos.Unleashed.Runtime.UI;

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
        SceneManager.LoadScene(nextSceneName);
    }
}
