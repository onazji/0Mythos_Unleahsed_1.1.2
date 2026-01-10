using UnityEngine;

/// <summary>
/// Subtle, procedural camera drift for menu and cinematic scenes.
/// Adds small oscillations to rotation and position for a natural,
/// atmospheric “alive” camera feel.
/// </summary>
[DisallowMultipleComponent]
[AddComponentMenu("Mythos/Camera/Cinematic Camera Drift")]
public class CinematicCameraDrift : MonoBehaviour
{
    [Header("Drift Intensity")]
    [SerializeField, Tooltip("Maximum degrees of rotation drift.")]
    private float _rotationAmplitude = 0.35f;

    [SerializeField, Tooltip("Maximum positional drift in meters.")]
    private float _positionAmplitude = 0.075f;

    [Header("Drift Speeds")]
    [SerializeField, Tooltip("Speed of rotational oscillation.")]
    private float _rotationSpeed = 0.2f;

    [SerializeField, Tooltip("Speed of positional oscillation.")]
    private float _positionSpeed = 0.1f;

    [Header("Follow Target (optional)")]
    [SerializeField, Tooltip("Optional target to orbit subtly around.")]
    private Transform _focusTarget;

    private Vector3 _basePos;
    private Quaternion _baseRot;

    private void Awake()
    {
        _basePos = transform.localPosition;
        _baseRot = transform.localRotation;
    }

    private void LateUpdate()
    {
        float t = Time.time;

        // Smooth oscillations
        float rotX = Mathf.Sin(t * _rotationSpeed) * _rotationAmplitude;
        float rotY = Mathf.Cos(t * _rotationSpeed * 1.2f) * _rotationAmplitude * 0.8f;
        float rotZ = Mathf.Sin(t * _rotationSpeed * 0.7f) * _rotationAmplitude * 0.5f;

        float posX = Mathf.Sin(t * _positionSpeed) * _positionAmplitude;
        float posY = Mathf.Cos(t * _positionSpeed * 0.9f) * _positionAmplitude * 0.7f;
        float posZ = Mathf.Sin(t * _positionSpeed * 1.3f) * _positionAmplitude * 0.4f;

        transform.localPosition = _basePos + new Vector3(posX, posY, posZ);
        transform.localRotation = _baseRot * Quaternion.Euler(rotX, rotY, rotZ);

        if (_focusTarget != null)
        {
            transform.LookAt(_focusTarget.position);
        }
    }

    private void OnDisable()
    {
        transform.localPosition = _basePos;
        transform.localRotation = _baseRot;
    }
}
