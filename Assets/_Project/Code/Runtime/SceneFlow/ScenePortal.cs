using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// ScenePortal is a simple doorway trigger:
/// - When the Player enters, it tells SceneTransit which spawn point to use in the next scene,
/// - Then it loads the target scene (via SceneLoader if available, else direct).
///
/// Drop this on any portal GameObject with a BoxCollider marked "Is Trigger".
/// </summary>
[DisallowMultipleComponent]
public class ScenePortal : MonoBehaviour
{
    [Header("Destination")]
    [SerializeField] private string targetScene = "";             // Exact scene name as it appears in Build Settings
    [SerializeField] private string targetSpawnPointName = "Default"; // The SpawnPoint.SpawnName to appear at in the destination

    [Header("Loading")]
    [SerializeField] private bool useAsyncLoader = true;          // If true and SceneLoader exists, use it

    [Header("Filtering")]
    [SerializeField] private string requiredTag = "Player";       // Only objects with this tag can activate the portal

    // Tiny guard to prevent double-triggering on the same frame or while loading
    private bool _engaged;

    private void Reset()
    {
        // Ensure a trigger collider is present when this component is first added
        var col = GetComponent<Collider>();
        if (col == null) col = gameObject.AddComponent<BoxCollider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ignore anything that isn't the Player (by tag)
        if (!other.CompareTag(requiredTag)) return;
        if (_engaged) return; // already triggered; ignore re-entry spam
        _engaged = true;

        // 1) Tell the transit singleton which spawn the next scene should use
        SceneTransit.SetNextSpawn(targetSpawnPointName);
        Debug.Log($"[ScenePortal] Sending to {targetScene} via spawn '{targetSpawnPointName}'");

        // 2) Make sure time isn't frozen (e.g., if coming from a paused menu)
        Time.timeScale = 1f;

        // 3) Load the destination scene (prefer the global async loader if present)
        if (useAsyncLoader && SceneLoader.Instance != null)
        {
            SceneLoader.Instance.Load(targetScene);
        }
        else
        {
            // Fallback: direct load (blocking)
            SceneManager.LoadScene(targetScene, LoadSceneMode.Single);
        }
    }

#if UNITY_EDITOR
    // Simple gizmo so you can see portals easily in the Scene view
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

        // Label-ish line toward "forward" to indicate direction
        Gizmos.color = new Color(0.1f, 0.8f, 1f, 0.9f);
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 1.2f);
    }
#endif
}
