using UnityEngine;
// using Cinemachine;
using Unity.Cinemachine;
using MoreMountains.TopDownEngine;

namespace Mythos.Unleashed.Runtime.Camera
{
    [RequireComponent(typeof(CinemachineVirtualCamera))]
    public class MythosARPGCamera : MonoBehaviour
    {
        [Header("Zoom")]
        [SerializeField] private float zoomSpeed = 2f;
        [SerializeField] private Vector2 zoomRange = new Vector2(6f, 15f);

        [Header("Orbit")]
        [SerializeField] private float orbitSpeed = 100f;
        [SerializeField] private bool allowOrbit = true;

        private CinemachineVirtualCamera _vcam;
        private CinemachineTransposer _transposer;
        private Vector3 _followOffset;

        private void Awake()
        {
            _vcam = GetComponent<CinemachineVirtualCamera>();
            _transposer = _vcam.GetCinemachineComponent<CinemachineTransposer>();
            _followOffset = _transposer.m_FollowOffset;
        }

        private void Update()
        {
            HandleZoom();
            HandleOrbit();
        }

        private void HandleZoom()
        {
            float scroll = Input.mouseScrollDelta.y;
            if (Mathf.Abs(scroll) > 0.01f)
            {
                float newDistance = Mathf.Clamp(_followOffset.magnitude - scroll * zoomSpeed, zoomRange.x, zoomRange.y);
                _followOffset = _followOffset.normalized * newDistance;
                _transposer.m_FollowOffset = Vector3.Lerp(_transposer.m_FollowOffset, _followOffset, Time.deltaTime * 10f);
            }
        }

        private void HandleOrbit()
        {
            if (!allowOrbit || !Input.GetMouseButton(1)) return; // RMB hold
            float rotation = Input.GetAxis("Mouse X") * orbitSpeed * Time.deltaTime;
            Quaternion rot = Quaternion.AngleAxis(rotation, Vector3.up);
            _followOffset = rot * _followOffset;
            _transposer.m_FollowOffset = Vector3.Lerp(_transposer.m_FollowOffset, _followOffset, Time.deltaTime * 10f);
        }
    }
}
