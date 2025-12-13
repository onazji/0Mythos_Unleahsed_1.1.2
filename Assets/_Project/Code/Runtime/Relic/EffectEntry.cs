using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// One small row showing: icon, name, and a shrinking bar/timer.
/// </summary>
public class EffectEntry : MonoBehaviour
{
    [SerializeField] private Image iconImage;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private Image barFill;

    private float _remain;
    private float _duration;

    public string Name => nameText != null ? nameText.text : "";

    public bool IsDone => _remain <= 0f;

    public void Begin(float duration, Sprite icon)
    {
        _duration = Mathf.Max(0.01f, duration);
        _remain = _duration;

        if (iconImage) { iconImage.sprite = icon; iconImage.enabled = icon != null; }
        if (nameText && string.IsNullOrEmpty(nameText.text)) nameText.text = "Effect";
        UpdateVisuals();
    }

    private void Update()
    {
        if (_remain <= 0f) return;
        _remain -= Time.deltaTime;
        if (_remain < 0f) _remain = 0f;
        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        if (timeText) timeText.text = $"{_remain:0.0}s";
        if (barFill) barFill.fillAmount = (_duration > 0f) ? (_remain / _duration) : 0f;
    }
}
