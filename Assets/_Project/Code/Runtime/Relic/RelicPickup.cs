using UnityEngine;

/// <summary>
/// World pickup for a relic:
/// - Grants a temporary speed boost
/// - Logs the relic in RelicRegistry (persistence)
/// - Shows a popup with name/lore/icon
///
/// Attach to a small 3D object with a Trigger collider.
/// </summary>
[DisallowMultipleComponent]
public class RelicPickup : MonoBehaviour
{
    [Header("Relic Data")]
    [SerializeField, Tooltip("Relic definition ScriptableObject (required).")]
    private RelicDef relicDef;

    [Header("Speed Boost")]
    [SerializeField, Tooltip("How much faster the player moves while boosted.")]
    private float speedMultiplier = 1.5f;

    [SerializeField, Tooltip("Seconds the speed boost lasts.")]
    private float boostDuration = 5f;

    [Header("Popup UI")]
    [SerializeField, Tooltip("Drag the RelicPopup prefab here (from Assets/_Project/Prefabs/UI).")]
    private RelicPopup popupPrefab;

    [Header("Visual FX")]
    [SerializeField] private bool spin = true;
    [SerializeField] private float spinSpeed = 60f;
    [SerializeField] private bool bob = true;
    [SerializeField] private float bobHeight = 0.15f;
    [SerializeField] private float bobSpeed = 2f;

    [Header("Audio")]
    [SerializeField] private AudioClip pickupSfx;
    private AudioSource _audio;

    private Vector3 _startPos;

    private void Reset()
    {
        // Ensure a trigger exists for easy setup
        var col = GetComponent<Collider>();
        if (col == null) col = gameObject.AddComponent<SphereCollider>();
        col.isTrigger = true;
    }

    private void Awake()
    {
        _startPos = transform.position;

        _audio = gameObject.AddComponent<AudioSource>();
        _audio.playOnAwake = false;
        _audio.spatialBlend = 1f;
        _audio.rolloffMode = AudioRolloffMode.Linear;

        // Helpful warning if RelicDef is not assigned
        if (relicDef == null)
        {
            Debug.LogWarning($"[RelicPickup] No RelicDef assigned on '{name}'. " +
                             "RelicRegistry logging and popup text will be generic.");
        }
    }

    private void Update()
    {
        if (spin) transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.World);

        if (bob)
        {
            float y = Mathf.Sin(Time.time * bobSpeed) * bobHeight;
            transform.position = _startPos + new Vector3(0, y, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // 1) Speed boost
        var mover = other.GetComponent<SimpleTopDownMover>();
        if (mover != null) StartCoroutine(BoostRoutine(mover));

        // The call from pickups 10/12/25
        ActiveEffectHUD.Instance?.AddEffect(
        relicDef != null ? relicDef.displayName : "Relic Buff",
        boostDuration,
        relicDef != null ? relicDef.displayIcon : null
        );


        // 2) Registry log (persistence)
        if (RelicRegistry.Instance != null && relicDef != null)
        {
            RelicRegistry.Instance.Add(relicDef); // <-- this prints the "Collected relic: ..." debug line
        }
        else
        {
            if (RelicRegistry.Instance == null)
                Debug.LogWarning("[RelicPickup] RelicRegistry not present in scene (Bootstrap should have _RelicRegistry).");
            if (relicDef == null)
                Debug.LogWarning("[RelicPickup] RelicDef is null; nothing to add to registry.");
        }

        // 3) Popup UI
        TryShowPopup();

        // 4) SFX
        if (pickupSfx) _audio.PlayOneShot(pickupSfx);

        // 5) Remove the pickup
        gameObject.SetActive(false);
        Destroy(gameObject, 0.5f);
    }

    private System.Collections.IEnumerator BoostRoutine(SimpleTopDownMover mover)
    {
        float original = mover.SpeedMultiplier;
        mover.SpeedMultiplier = speedMultiplier;

        float t = 0f;
        while (t < boostDuration) { t += Time.deltaTime; yield return null; }

        mover.SpeedMultiplier = 1f;
    }

    private void TryShowPopup()
    {
        if (popupPrefab == null)
        {
            Debug.LogWarning("[RelicPickup] PopupPrefab not assigned; pickup will not show popup UI.");
            return;
        }

        // Find or create a Canvas
        var canvas = FindObjectOfType<Canvas>(true);
        if (canvas == null)
        {
            var go = new GameObject("HUDCanvas_Auto");
            canvas = go.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            var scaler = go.AddComponent<UnityEngine.UI.CanvasScaler>();
            scaler.uiScaleMode = UnityEngine.UI.CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);

            go.AddComponent<UnityEngine.EventSystems.EventSystem>();
        }

        var popup = Instantiate(popupPrefab, canvas.transform);
        popup.gameObject.SetActive(true);

        // Use RelicDef data if present; else generic strings
        string nameText = relicDef != null ? relicDef.displayName : "Relic Discovered";
        string loreText = relicDef != null ? relicDef.displayLore : "Mysteries whisper through old metal.";
        Sprite icon = relicDef != null ? relicDef.displayIcon : null;

        popup.Show(nameText, loreText, icon);
        Destroy(popup.gameObject, 5f);
    }
}
