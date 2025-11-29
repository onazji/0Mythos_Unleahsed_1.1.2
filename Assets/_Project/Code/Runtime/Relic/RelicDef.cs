using UnityEngine;

/// <summary>
/// ScriptableObject describing a relic: a stable unique ID plus display data.
/// Create instances via Assets → Create → Mythos → Relic Definition.
/// </summary>
[CreateAssetMenu(menuName = "Mythos/Relic Definition", fileName = "RelicDef_")]
public class RelicDef : ScriptableObject
{
    [Tooltip("Unique, stable ID (e.g., RELIC_SWIFT_STEP). Case-sensitive. Do not change after shipping!")]
    public string relicId = "RELIC_ID";

    [Header("Display")]
    public string displayName = "Relic Name";
    [TextArea] public string displayLore = "A single-line legend or flavor text.";
    public Sprite displayIcon;
}
