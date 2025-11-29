using UnityEngine;
using TMPro;
using System.Collections;

/// <summary>
/// Handles showing a relic-pickup popup with name, lore, and icon.
/// Automatically fades in, holds, and fades out.
/// </summary>
public class RelicPopup : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private TextMeshProUGUI relicName;
    [SerializeField] private TextMeshProUGUI relicLore;
    [SerializeField] private UnityEngine.UI.Image relicIcon;

    [Header("Timing")]
    [SerializeField] private float fadeIn = 0.25f;
    [SerializeField] private float hold = 2.0f;
    [SerializeField] private float fadeOut = 0.4f;

    private void Awake()
    {
        if (canvasGroup != null) canvasGroup.alpha = 0f;
    }

    public void Show(string name, string lore, Sprite icon = null)
    {
        if (relicName) relicName.text = name;
        if (relicLore) relicLore.text = lore;
        if (relicIcon)
        {
            relicIcon.enabled = (icon != null);
            relicIcon.sprite = icon;
        }

        StopAllCoroutines();
        StartCoroutine(FadeRoutine());
    }

    private IEnumerator FadeRoutine()
    {
        float t = 0f;
        while (t < fadeIn) { t += Time.unscaledDeltaTime; canvasGroup.alpha = t / fadeIn; yield return null; }
        canvasGroup.alpha = 1f;

        yield return new WaitForSecondsRealtime(hold);

        t = 0f;
        while (t < fadeOut) { t += Time.unscaledDeltaTime; canvasGroup.alpha = 1f - (t / fadeOut); yield return null; }
        canvasGroup.alpha = 0f;
    }
}
