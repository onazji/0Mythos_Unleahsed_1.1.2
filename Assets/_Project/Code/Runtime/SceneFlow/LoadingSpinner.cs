using UnityEngine;
using TMPro;

namespace Mythos.Unleashed.Runtime.UI
{
    public class LoadingSpinner : MonoBehaviour
    {
        [Header("Rotation")]
        [SerializeField] private float rotationSpeed = 180f;
        [SerializeField] private bool clockwise = true;
        [SerializeField] private AnimationCurve speedEase = AnimationCurve.EaseInOut(0, 0, 1, 1);

        [Header("Optional Text Pulse")]
        [SerializeField] private TMP_Text loadingText;
        [SerializeField] private float pulseSpeed = 2f;
        [SerializeField, Range(0f, 1f)] private float minAlpha = 0.3f;

        private float _t = 0f;

        void Update()
        {
            // Spinner rotation with ease
            _t = Mathf.PingPong(Time.unscaledTime * 0.2f, 1f);
            float easedSpeed = rotationSpeed * speedEase.Evaluate(_t);
            transform.Rotate(Vector3.forward * (clockwise ? -easedSpeed : easedSpeed) * Time.unscaledDeltaTime);

            // Optional text alpha pulse
            if (loadingText != null)
            {
                var c = loadingText.color;
                c.a = Mathf.Lerp(minAlpha, 1f, (Mathf.Sin(Time.unscaledTime * pulseSpeed) + 1f) * 0.5f);
                loadingText.color = c;
            }
        }
    }
}
