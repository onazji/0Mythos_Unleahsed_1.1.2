using UnityEngine;

/// <summary>
/// Creates a gentle "breathing" effect on a scene’s directional light.
/// Oscillates between two intensity values to simulate atmospheric rhythm.
/// </summary>
[DisallowMultipleComponent]
[AddComponentMenu("Mythos/Lighting/Ambient Light Pulse")]
public class AmbientLightPulse : MonoBehaviour
{
    [Header("Light Settings")]
    [SerializeField, Tooltip("Reference to the directional light that will pulse.")]
    private Light _targetLight;

    [SerializeField, Tooltip("Minimum intensity value for the pulse.")]
    private float _minIntensity = 0.85f;

    [SerializeField, Tooltip("Maximum intensity value for the pulse.")]
    private float _maxIntensity = 1.15f;

    [SerializeField, Tooltip("Speed of the breathing rhythm.")]
    private float _pulseSpeed = 0.25f;

    [Header("Color Variation")]
    [SerializeField, Tooltip("Enable to slightly tint the color during pulse.")]
    private bool _tintWithPulse = true;

    [SerializeField, Tooltip("Warm color peak (optional).")]
    private Color _warmColor = new Color(1f, 0.96f, 0.85f);

    [SerializeField, Tooltip("Cool color trough (optional).")]
    private Color _coolColor = new Color(0.95f, 0.95f, 1f);

    private float _timeOffset;
    private float _baseIntensity;
    private Color _baseColor;

    private void Awake()
    {
        if (_targetLight == null)
        {
            _targetLight = GetComponent<Light>();
            if (_targetLight == null)
            {
                Debug.LogWarning("[AmbientLightPulse] No Light assigned or found.");
                enabled = false;
                return;
            }
        }

        _baseIntensity = _targetLight.intensity;
        _baseColor = _targetLight.color;
        _timeOffset = Random.Range(0f, Mathf.PI * 2f); // desync multiple lights
    }

    private void Update()
    {
        float t = (Mathf.Sin(Time.time * _pulseSpeed + _timeOffset) + 1f) / 2f; // oscillate 0–1
        float newIntensity = Mathf.Lerp(_minIntensity, _maxIntensity, t);
        _targetLight.intensity = _baseIntensity * newIntensity;

        if (_tintWithPulse)
        {
            _targetLight.color = Color.Lerp(_coolColor, _warmColor, t);
        }
    }

    private void OnDisable()
    {
        if (_targetLight != null)
        {
            _targetLight.intensity = _baseIntensity;
            _targetLight.color = _baseColor;
        }
    }
}
