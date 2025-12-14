using UnityEngine;
using UnityEngine.SceneManagement;
using Mythos.Unleashed.Runtime.UI; // For SceneFadeController

public class MainMenuUI : MonoBehaviour
{
    [Header("Scene Settings")]
    public string NextSceneName = "MuseumHub_WindWard";
    public float LoadDelay = 0.5f;

    [Header("Fade Settings")]
    private SceneFadeController _fadeController;

    private void Awake()
    {
        // Try to find fade controller in the persistent Bootstrap scene
        if (_fadeController == null)
        {
            _fadeController = FindObjectOfType<SceneFadeController>(true);
        }
    }

    public void StartGame()
    {
        if (_fadeController != null)
        {
            StartCoroutine(LoadWithFade());
        }
        else
        {
            Debug.LogWarning("[MainMenuUI] No fade controller found — loading instantly.");
            SceneManager.LoadScene(NextSceneName);
        }
    }

    private System.Collections.IEnumerator LoadWithFade()
    {
        _fadeController.BeginFadeOut();
        yield return new WaitForSecondsRealtime(LoadDelay);
        SceneManager.LoadScene(NextSceneName);
    }
}
