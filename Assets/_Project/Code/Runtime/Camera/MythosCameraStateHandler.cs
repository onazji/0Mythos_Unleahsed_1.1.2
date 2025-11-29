using UnityEngine;
// using Cinemachine;
using Unity.Cinemachine;

namespace Mythos.Unleashed.Runtime.Camera
{
    public class MythosCameraStateHandler : MonoBehaviour
    {
        [System.Serializable]
        public struct CameraState
        {
            public string name;
            public Vector3 offset;
            public float fov;
        }

        [SerializeField] private CinemachineVirtualCamera vcam;
        [SerializeField] private CameraState topDown;
        [SerializeField] private CameraState closeFollow;
        [SerializeField] private CameraState cinematic;

        private CinemachineTransposer _transposer;

        private void Awake()
        {
            if (vcam == null) vcam = GetComponent<CinemachineVirtualCamera>();
            _transposer = vcam.GetCinemachineComponent<CinemachineTransposer>();
        }

        public void ApplyState(string state)
        {
            CameraState s = topDown;
            if (state == "Close") s = closeFollow;
            else if (state == "Cinematic") s = cinematic;

            _transposer.m_FollowOffset = s.offset;
            vcam.m_Lens.FieldOfView = s.fov;
        }
    }
}
