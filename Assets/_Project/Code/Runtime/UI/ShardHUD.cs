using UnityEngine;
using TMPro;

public class ShardHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI shardLabel;

    private void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        if (shardLabel == null) return;
        int value = (GameState.Instance != null) ? GameState.Instance.TotalShards : 0;
        shardLabel.text = $"Shards: {value}";
    }
}


