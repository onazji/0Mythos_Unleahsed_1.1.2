using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper : MonoBehaviour
{
    [SerializeField] private string firstSceneName = "MainMenu";

    private void Awake()
    {
        // Jump straight to the first playable scene.
        // Make sure "MainMenu" is in Build Settings and spelled exactly the same.
        SceneManager.LoadScene(firstSceneName, LoadSceneMode.Single);
    }
}
