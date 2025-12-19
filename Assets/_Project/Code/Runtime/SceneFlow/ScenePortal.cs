using UnityEngine;
using UnityEngine.SceneManagement;
using Mythos.Unleashed.Runtime.UI; // <-- Required for SceneFadeController
using Mythos.Unleashed.Runtime.SceneFlow;


/// <summary>
/// ScenePortal: triggers a scene transition with fade support.
/// </summary>
[DisallowMultipleComponent]
public class ScenePortal : MonoBehaviour
{
    [Header("Destination")]
    [SerializeField] private string targetScene = "";
    [SerializeField] private string targetSpawnPointName = "Default";

    [Header("Loading")]
    [SerializeField] private bool useAsyncLoader = true;
    [SerializeField] private float fadeDuration = 0.8f; // matches SceneFadeController

    [Header("Filtering")]
    [SerializeField] private string requiredTag = "Player";

    private bool _engaged;

    private void Reset()
    {
        var col = GetComponent<Collider>();
        if (col == null) col = gameObject.AddComponent<BoxCollider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag(requiredTag)) return;
        if (_engaged) return;
        _engaged = true;

        SceneTransit.SetNextSpawn(targetSpawnPointName);
        Debug.Log($"[ScenePortal] Sending to {targetScene} via spawn '{targetSpawnPointName}'");

        Time.timeScale = 1f;

        // Start the fade + transition coroutine
        StartCoroutine(TransitionWithFade());
    }

    private System.Collections.IEnumerator TransitionWithFade()
    {
        // 1️⃣ Try to locate global fade controller (persistent from Bootstrap)
        var fade = FindObjectOfType<SceneFadeController>();
        if (fade != null)
        {
            fade.BeginFadeOut();
            yield return new WaitForSecondsRealtime(fadeDuration);
        }
        else
        {
            Debug.LogWarning("[ScenePortal] No SceneFadeController found! Instant load.");
        }

        // 2️⃣ Load the scene (same logic as before)
        if (useAsyncLoader && SceneLoader.Instance != null)
        {
            SceneLoader.Instance.Load(targetScene);
        }
        else
        {
            SceneManager.LoadScene(targetScene, LoadSceneMode.Single);
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.1f, 0.8f, 1f, 0.35f);
        var c = GetComponent<Collider>();
        if (c != null)
        {
            Gizmos.matrix = transform.localToWorldMatrix;
            if (c is BoxCollider bc)
                Gizmos.DrawCube(bc.center, bc.size);
            else
                Gizmos.DrawWireSphere(Vector3.zero, 0.5f);
        }

        Gizmos.color = new Color(0.1f, 0.8f, 1f, 0.9f);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 1.2f);
    }
#endif
}
