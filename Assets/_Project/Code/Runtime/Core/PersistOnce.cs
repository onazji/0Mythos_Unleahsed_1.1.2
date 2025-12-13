using UnityEngine;
public class PersistOnce : MonoBehaviour
{
    private static PersistOnce _instance;
    private void Awake()
    {
        if (_instance != null && _instance != this) { Destroy(gameObject); return; }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
