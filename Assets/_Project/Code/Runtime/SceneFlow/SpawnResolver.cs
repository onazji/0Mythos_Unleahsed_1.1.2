using UnityEngine;
using System.Collections;

/// <summary>
/// SpawnResolver moves the Player to a named SpawnPoint after a scene loads.
/// It cooperates with SceneTransit:
///   - ScenePortal sets SceneTransit.NextSpawn (e.g., "FromMuseum") before loading.
///   - When the new scene starts, SpawnResolver looks for a SpawnPoint whose
///     SpawnName matches NextSpawn (or "Default" if none was set).
///   - Once the Player is placed, SceneTransit.MarkPlaced() prevents any other
///     late movers from yanking the Player somewhere else this scene.
///
/// Drop ONE of these in EVERY playable scene (Museum, Maze, Boss).
/// Name the GameObject something like "_SpawnResolver".
/// </summary>
[DisallowMultipleComponent]
[AddComponentMenu("Mythos/Spawning/Spawn Resolver")]
public class SpawnResolver : MonoBehaviour
{
    [Tooltip("If no explicit NextSpawn was set, we’ll use this fallback.")]
    [SerializeField] private string fallbackSpawnName = "Default";

    private void Start()
    {
        // Use a coroutine so all scene objects (including SpawnPoints and Player)
        // have a chance to Awake/Start before we try to move the Player.
        StartCoroutine(ResolveNextFrame());
    }

    private IEnumerator ResolveNextFrame()
    {
        // Wait one frame to avoid race conditions with other initializers.
        yield return null;

        // If someone already placed the Player this scene (e.g. a late hook),
        // do nothing.
        if (SceneTransit.PlacedThisScene)
            yield break;

        // Find the Player (must be tagged "Player").
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("[SpawnResolver] No Player found (tag 'Player').");
            yield break;
        }

        // Decide which spawn name we want: the requested one, or fallback.
        string desired = string.IsNullOrEmpty(SceneTransit.NextSpawn)
            ? fallbackSpawnName
            : SceneTransit.NextSpawn;

        // Look up all SpawnPoints in the scene.
        SpawnPoint[] points = FindObjectsOfType<SpawnPoint>(true);
        // (Optional) Log what we found for easy debugging.
        // Debug.Log($"[SpawnResolver] Want '{desired}'. Found {points.Length} spawn(s).");

        // Try to find an exact name match first.
        SpawnPoint chosen = null;
        foreach (var p in points)
        {
            if (p != null && p.gameObject.activeInHierarchy && p.SpawnName == desired)
            {
                chosen = p;
                break;
            }
        }

        // If we couldn’t find the desired one, try the fallback by name.
        if (chosen == null && desired != fallbackSpawnName)
        {
            foreach (var p in points)
            {
                if (p != null && p.gameObject.activeInHierarchy && p.SpawnName == fallbackSpawnName)
                {
                    chosen = p;
                    break;
                }
            }
        }

        if (chosen == null)
        {
            Debug.LogWarning($"[SpawnResolver] No matching SpawnPoint for '{desired}'. Player stays where placed.");
            yield break;
        }

        // Move the Player. If there’s a CharacterController, disable it briefly
        // so setting transform doesn’t fight the controller.
        var cc = player.GetComponent<CharacterController>();
        if (cc != null) cc.enabled = false;

        player.transform.SetPositionAndRotation(chosen.transform.position, chosen.transform.rotation);

        if (cc != null) cc.enabled = true;

        // Mark the Player as placed so nothing else re-positions them this scene.
        SceneTransit.MarkPlaced();

        // (Optional) Log placement result.
        // Debug.Log($"[SpawnResolver] Moved player to '{desired}' at {chosen.transform.position}");
    }
}
