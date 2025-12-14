using UnityEngine;

[DisallowMultipleComponent]
public class PersistOnce : MonoBehaviour
{
    private void Awake()
    {
        // Find all objects of this type in the scene (including inactive ones)
        var existing = FindObjectsOfType<PersistOnce>(true);

        // If another object with the same name exists, destroy this duplicate
        foreach (var instance in existing)
        {
            if (instance != this && instance.name == name)
            {
                Destroy(gameObject);
                return;
            }
        }

        DontDestroyOnLoad(gameObject);
    }
}
