using System.Collections.Generic;
using UnityEngine;

public class ActiveEffectHUD : MonoBehaviour
{
    public static ActiveEffectHUD Instance { get; private set; }

    [SerializeField] private RectTransform listParent;
    [SerializeField] private EffectEntry entryPrefab;

    private readonly List<EffectEntry> _entries = new();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // a duplicate in a later scene
            return;
        }

        Instance = this;

        // Ensure we mark the **root** object persistent (the Canvas)
        var root = transform.root;
        if (root != null) DontDestroyOnLoad(root.gameObject);
        else DontDestroyOnLoad(gameObject);
    }

    public void AddEffect(string effectName, float duration, Sprite icon = null)
    {
        if (listParent == null || entryPrefab == null || string.IsNullOrEmpty(effectName)) return;

        // refresh existing entry with same name
        foreach (var e in _entries)
        {
            if (e != null && e.Name == effectName)
            {
                e.Begin(duration, icon);
                return;
            }
        }

        var entry = Instantiate(entryPrefab, listParent);
        entry.name = $"Effect_{effectName}";
        entry.Begin(duration, icon);
        _entries.Add(entry);
    }

    private void LateUpdate()
    {
        for (int i = _entries.Count - 1; i >= 0; i--)
        {
            if (_entries[i] == null || _entries[i].IsDone)
            {
                if (_entries[i] != null) Destroy(_entries[i].gameObject);
                _entries.RemoveAt(i);
            }
        }
    }
}
