using System.IO;
using UnityEditor;
using UnityEngine;

public static class RegistryBuilder
{
    [MenuItem("Mythos/Sync Prefab Registry")]
    public static void UpdateRegistry()
    {
        string path = "Assets/Data/PrefabRegistry.json";
        Directory.CreateDirectory("Assets/Data");

        var allPrefabs = Directory.GetFiles("Assets/Prefabs", "*.prefab", SearchOption.AllDirectories);
        var entries = new System.Collections.Generic.List<string>();

        foreach (var prefabPath in allPrefabs)
        {
            entries.Add(prefabPath);
        }

        File.WriteAllLines(path, entries);
        AssetDatabase.Refresh();
        Debug.Log($"✅ Synced {entries.Count} prefabs to PrefabRegistry.json");
    }
}
