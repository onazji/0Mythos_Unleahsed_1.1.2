using UnityEngine;

/// <summary>
/// SceneTransit is a global singleton that stores the name of the spawn point
/// the player should appear at after a scene transition. It also keeps track of
/// whether the player has already been placed in the current scene.
/// </summary>
public class SceneTransit : MonoBehaviour
{
    // Singleton instance (accessible from anywhere)
    public static SceneTransit Instance;

    // The name of the spawn point we want to use in the next scene
    public static string NextSpawn = "";

    // Has the player already been placed in the current scene?
    public static bool PlacedThisScene = false;

    private void Awake()
    {
        // Standard singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("[SceneTransit] Ready (persistent)");
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Called by portals before a scene loads. 
    /// Sets the name of the spawn point we’ll look for in the next scene.
    /// </summary>
    public static void SetNextSpawn(string spawnName)
    {
        NextSpawn = spawnName ?? "";
        PlacedThisScene = false; // The new scene will need placement
        Debug.Log($"[SceneTransit] SetNextSpawn '{NextSpawn}'");
    }

    /// <summary>
    /// Called after we have successfully placed the player.
    /// Prevents further movement during the same scene.
    /// </summary>
    public static void MarkPlaced()
    {
        PlacedThisScene = true;
        // Optional: clear NextSpawn so we fall back to Default next time.
        NextSpawn = "";
        Debug.Log("[SceneTransit] Player placed and flag cleared");
    }
}

