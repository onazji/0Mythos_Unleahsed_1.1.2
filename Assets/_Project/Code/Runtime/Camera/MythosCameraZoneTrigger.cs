using UnityEngine;

namespace Mythos.Unleashed.Runtime.Camera
{
    [RequireComponent(typeof(Collider))]
    public class MythosCameraZoneTrigger : MonoBehaviour
    {
        [SerializeField] private string targetState = "Cinematic";

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                var handler = FindObjectOfType<MythosCameraStateHandler>();
                if (handler != null) handler.ApplyState(targetState);
            }
        }
    }
}
