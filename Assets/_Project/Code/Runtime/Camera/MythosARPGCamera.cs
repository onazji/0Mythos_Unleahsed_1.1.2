using UnityEngine;
using Unity.Cinemachine;
using MoreMountains.TopDownEngine;

namespace Mythos.Unleashed.Runtime.Camera
{
    [RequireComponent(typeof(CinemachineCamera))]
    public class MythosARPGCamera : MonoBehaviour
    {
        [Header("Zoom Settings")]
        [SerializeField] private float zoomSpeed = 2f;
        [SerializeField] private Vector2 zoomRange = new Vector2(6f, 15f);

        [Header("Orbit Settings")]
        [SerializeField] private float orbitSpeed = 100f;
        [SerializeField] private bool allowOrbit = true;

        [Header("Cinemachine References")]
        [SerializeField] private CinemachineCamera cinemachineCam;
        private CinemachinePositionComposer _composer;
        private Transform _followTarget;
        private float _currentDistance;

        private void Awake()
        {
            if (cinemachineCam == null)
            {
                cinemachineCam = GetComponent<CinemachineCamera>();
                if (cinemachineCam == null)
                {
                    Debug.LogError("[MythosARPGCamera] Missing CinemachineCamera component!");
                    enabled = false;
                    return;
                }
            }

            _composer = cinemachineCam.GetComponent<CinemachinePositionComposer>();
            if (_composer == null)
            {
                Debug.LogWarning("[MythosARPGCamera] No CinemachinePositionComposer found — zoom will be disabled.");
            }

            _followTarget = cinemachineCam.Follow;
            _currentDistance = _composer != null ? _composer.CameraDistance : zoomRange.y;
        }

        private void Update()
        {
            if (_composer == null) return;

            HandleZoom();
            if (allowOrbit)
                HandleOrbit();
        }

        private void HandleZoom()
        {
            float scroll = Input.mouseScrollDelta.y;
            if (Mathf.Abs(scroll) < 0.01f) return;

            _currentDistance -= scroll * zoomSpeed;
            _currentDistance = Mathf.Clamp(_currentDistance, zoomRange.x, zoomRange.y);
            _composer.CameraDistance = Mathf.Lerp(_composer.CameraDistance, _currentDistance, Time.deltaTime * 10f);
        }

        private void HandleOrbit()
        {
            if (!Input.GetMouseButton(1) || _followTarget == null) return;

            float rotation = Input.GetAxis("Mouse X") * orbitSpeed * Time.deltaTime;
            transform.RotateAround(_followTarget.position, Vector3.up, rotation);
        }
    }
}
