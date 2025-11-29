using UnityEngine;

#if UNITY_EDITOR
using UnityEditor; // for in-scene labels (editor only)
#endif

/// <summary>
/// Drop this on an empty GameObject to mark a named spawn location
/// the player can appear at when a scene loads (e.g., "Default",
/// "FromMuseum", "FromMaze", "FromBoss").
///
/// Only the name matters to the resolver. Position/rotation of this
/// object becomes the player's arrival transform.
/// </summary>
[DisallowMultipleComponent]
[AddComponentMenu("Mythos/Spawning/Spawn Point")]
public class SpawnPoint : MonoBehaviour
{
    [Tooltip("The key the portal will request on the other side (e.g., \"Default\", \"FromMuseum\", \"FromBoss\"). Case-sensitive.")]
    [SerializeField] private string spawnName = "Default";

    /// <summary>Public read-only accessor used by resolvers.</summary>
    public string SpawnName => spawnName;

#if UNITY_EDITOR
    // Pretty gizmo so you can see spawn points clearly in the Scene view.
    private void OnDrawGizmos()
    {
        // Base marker
        Gizmos.color = new Color(0.1f, 0.9f, 0.7f, 0.85f);
        Gizmos.DrawSphere(transform.position, 0.25f);

        // Forward arrow (which way the player will face)
        Gizmos.color = new Color(0.1f, 0.9f, 0.7f, 0.9f);
        var p = transform.position;
        Gizmos.DrawLine(p, p + transform.forward * 0.8f);

        // Text label
        Handles.color = Color.cyan;
        Handles.Label(p + Vector3.up * 0.35f, $"Spawn: {spawnName}");
    }
#endif
}
