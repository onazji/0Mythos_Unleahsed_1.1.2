using UnityEngine;

[DisallowMultipleComponent]
public class SkyRotator : MonoBehaviour
{
    [Header("Rotation Settings")]
    [SerializeField, Tooltip("Speed in degrees per second.")]
    private float rotationSpeed = 2f;

    private void Update()
    {
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.World);
    }
}
