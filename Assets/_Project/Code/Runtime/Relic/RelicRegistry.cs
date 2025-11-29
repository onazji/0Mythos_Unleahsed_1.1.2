using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Singleton that remembers which relics the player has collected.
/// Stores unlocked relic IDs in PlayerPrefs (simple prototype persistence).
/// In a future save system, swap this out for EasySave or JSON.
/// </summary>
[DisallowMultipleComponent]
[AddComponentMenu("Mythos/Relics/Relic Registry")]
public class RelicRegistry : MonoBehaviour
{


    [ContextMenu("Debug/Clear All Relics")]
private void DebugClearAllMenu()
{
    ClearAll();
}

[ContextMenu("Debug/Print Collected IDs")]
private void DebugPrint()
{
    Debug.Log("[RelicRegistry] " + string.Join(", ", _collected));
}

    // new code added above allows clearing of relics from the player prefs. for testing 10/10/25
    public static RelicRegistry Instance;

    // Internal set of collected relic IDs
    private readonly HashSet<string> _collected = new HashSet<string>();

    private const string PREF_KEY = "MYTHOS_RELICS_COLLECTED";

    private void Awake()
    {
        // Singleton setup
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        Load();
    }

    /// <summary>Mark a relic as collected and save immediately.</summary>
    public void Add(RelicDef def)
    {
        if (def == null || string.IsNullOrEmpty(def.relicId)) return;

        if (_collected.Add(def.relicId))
        {
            Save();
            Debug.Log($"[RelicRegistry] Collected relic: {def.relicId}");
        }
    }

    /// <summary>Check whether the player has already collected this relic.</summary>
    public bool Has(RelicDef def)
    {
        return def != null && _collected.Contains(def.relicId);
    }

    /// <summary>Return all collected IDs (copy).</summary>
    public List<string> GetAll() => new List<string>(_collected);

    private void Save()
    {
        string data = string.Join(",", _collected);
        PlayerPrefs.SetString(PREF_KEY, data);
        PlayerPrefs.Save();
    }

    private void Load()
    {
        _collected.Clear();
        if (!PlayerPrefs.HasKey(PREF_KEY)) return;

        string data = PlayerPrefs.GetString(PREF_KEY, "");
        if (string.IsNullOrEmpty(data)) return;

        foreach (var id in data.Split(','))
        {
            if (!string.IsNullOrWhiteSpace(id))
                _collected.Add(id);
        }
    }

    /// <summary>Erase all stored relic progress (debug only).</summary>
    public void ClearAll()
    {
        _collected.Clear();
        PlayerPrefs.DeleteKey(PREF_KEY);
        PlayerPrefs.Save();
        Debug.Log("[RelicRegistry] Cleared all relic progress.");
    }
}
