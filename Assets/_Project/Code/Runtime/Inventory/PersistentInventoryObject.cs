using UnityEngine;

[DisallowMultipleComponent]
public sealed class PersistentInventoryObject : MonoBehaviour
{
    private static readonly System.Collections.Generic.Dictionary<string, PersistentInventoryObject>
        Instances = new();

    [SerializeField]
    private string persistenceId;

    private void Awake()
    {
        if (string.IsNullOrWhiteSpace(persistenceId))
        {
            persistenceId = gameObject.name;
        }

        if (Instances.TryGetValue(persistenceId, out PersistentInventoryObject existing))
        {
            if (existing != null && existing != this)
            {
                Debug.Log(
                    $"[PersistentInventoryObject] Destroying duplicate '{persistenceId}'.",
                    gameObject
                );

                Destroy(gameObject);
                return;
            }

            Instances.Remove(persistenceId);
        }

        Instances.Add(persistenceId, this);

        DontDestroyOnLoad(gameObject);

        Debug.Log(
            $"[PersistentInventoryObject] Preserving '{persistenceId}'.",
            gameObject
        );
    }

    private void OnDestroy()
    {
        if (
            !string.IsNullOrWhiteSpace(persistenceId) &&
            Instances.TryGetValue(persistenceId, out PersistentInventoryObject existing) &&
            existing == this
        )
        {
            Instances.Remove(persistenceId);
        }
    }
}