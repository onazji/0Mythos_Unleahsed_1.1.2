using UnityEngine;

/// <summary>
/// Simulates drifting sand or dust particles for ambient world motion.
/// Works well in menus, desert biomes, or calm atmospheres.
/// Attach to an empty GameObject with a ParticleSystem.
/// </summary>
[RequireComponent(typeof(ParticleSystem))]
[AddComponentMenu("Mythos/FX/Dust Drift FX")]
public class DustDriftFX : MonoBehaviour
{
    [Header("Wind Behavior")]
    [SerializeField, Tooltip("Average horizontal wind direction.")]
    private Vector3 _windDirection = new Vector3(1f, 0.15f, 0f);

    [SerializeField, Tooltip("Overall wind strength multiplier.")]
    private float _windStrength = 0.4f;

    [SerializeField, Tooltip("Randomness in drift movement.")]
    private float _turbulence = 0.2f;

    [Header("Particle Control")]
    [SerializeField, Tooltip("Scale of speed fluctuation over time.")]
    private float _gustFrequency = 0.25f;

    private ParticleSystem _ps;
    private ParticleSystem.VelocityOverLifetimeModule _vel;
    private Vector3 _baseVelocity;

    private void Awake()
    {
        _ps = GetComponent<ParticleSystem>();
        _vel = _ps.velocityOverLifetime;
        _baseVelocity = _windDirection.normalized * _windStrength;
    }

    private void Update()
    {
        float gust = 1f + Mathf.Sin(Time.time * _gustFrequency) * _turbulence;
        Vector3 liveVelocity = _baseVelocity * gust;

        _vel.x = new ParticleSystem.MinMaxCurve(liveVelocity.x);
        _vel.y = new ParticleSystem.MinMaxCurve(liveVelocity.y);
        _vel.z = new ParticleSystem.MinMaxCurve(liveVelocity.z);
    }
}
