using UnityEngine;

[DisallowMultipleComponent]
public class LightBreather : MonoBehaviour
{
    [Header("Breathing Light Settings")]
    [SerializeField, Tooltip("Base light intensity.")] 
    private float baseIntensity = 1.1f;

    [SerializeField, Tooltip("How much intensity oscillates (+/-).")] 
    private float pulseAmplitude = 0.15f;

    [SerializeField, Tooltip("Speed of light breathing.")] 
    private float pulseSpeed = 0.6f;

    private Light _light;

    private void Awake()
    {
        _light = GetComponent<Light>();
        if (_light == null)
            Debug.LogWarning("[LightBreather] No Light component found on this object.");
    }

    private void Update()
    {
        if (_light)
        {
            float pulse = Mathf.Sin(Time.time * pulseSpeed);
            _light.intensity = baseIntensity + (pulse * pulseAmplitude);
        }
    }
}
