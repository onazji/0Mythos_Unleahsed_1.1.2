using UnityEngine;

/// <summary>
/// Simple top-down mover using CharacterController.
/// Now supports a public speed multiplier so pickups can buff movement temporarily.
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class SimpleTopDownMover : MonoBehaviour
{
    [Header("Base Movement")]
    [SerializeField] private float moveSpeed = 6f;

    [Header("Runtime (read-only)")]
    [SerializeField] private float speedMultiplier = 1f; // modified by powerups
    public float SpeedMultiplier
    {
        get => speedMultiplier;
        set => speedMultiplier = Mathf.Max(0.1f, value);
    }

    private CharacterController _cc;

    private void Awake()
    {
        _cc = GetComponent<CharacterController>();
        gameObject.tag = "Player"; // ensure tagging for triggers
    }

    private void Update()
    {
        // Old input system (project set to "Both")
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        Vector3 input = new Vector3(h, 0f, v).normalized;

        // Apply speed multiplier
        float finalSpeed = moveSpeed * speedMultiplier;

        // Gravity shim (keeps grounded)
        Vector3 velocity = input * finalSpeed;
        if (!_cc.isGrounded) velocity.y += Physics.gravity.y * Time.deltaTime;

        _cc.Move(velocity * Time.deltaTime);
    }
}
