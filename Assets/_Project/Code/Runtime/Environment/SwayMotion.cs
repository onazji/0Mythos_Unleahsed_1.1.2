using UnityEngine;

namespace Mythos.Unleashed.Runtime.Environment
{
    /// <summary>
    /// Simple sine-based sway motion to give life to foliage, banners, etc.
    /// </summary>
    public class SwayMotion : MonoBehaviour
    {
        [SerializeField] private Vector3 swayAxis = new Vector3(0f, 1f, 0f);
        [SerializeField] private float swayAmplitude = 3f;
        [SerializeField] private float swaySpeed = 1f;
        [SerializeField] private float offset = 0f;

        private Vector3 _startRot;

        private void Start()
        {
            _startRot = transform.localEulerAngles;
            offset = Random.Range(0f, Mathf.PI * 2f); // desync multiple sways
        }

        private void Update()
        {
            float sway = Mathf.Sin(Time.time * swaySpeed + offset) * swayAmplitude;
            transform.localRotation = Quaternion.Euler(_startRot + swayAxis * sway);
        }
    }
}
