using UnityEngine;

public class GameState : MonoBehaviour
{
    public static GameState Instance { get; private set; }
    private const string ShardKey = "MU_TotalShards";

    public int TotalShards { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
        TotalShards = PlayerPrefs.GetInt(ShardKey, 0);
    }

    public void AddShards(int amount)
    {
        TotalShards = Mathf.Max(0, TotalShards + amount);
        PlayerPrefs.SetInt(ShardKey, TotalShards);
        PlayerPrefs.Save();
    }
}
