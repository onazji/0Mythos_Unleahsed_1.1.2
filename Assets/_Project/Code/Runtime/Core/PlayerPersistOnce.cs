using UnityEngine;

[DisallowMultipleComponent]
public class PlayerPersistOnce : MonoBehaviour
{
    private static PlayerPersistOnce _instance;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }
}