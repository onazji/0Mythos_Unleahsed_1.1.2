using MoreMountains.TopDownEngine;
using UnityEngine;

namespace Mythos.Unleashed.Runtime
{
    /// <summary>
    /// Watches a boss Health component and activates the return portal
    /// once the boss reaches zero health.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class BossDeathPortalActivator : MonoBehaviour
    {
        [Header("Boss")]
        [SerializeField]
        private Health bossHealth;

        [Header("Return Portal")]
        [SerializeField]
        private GameObject returnPortal;

        [Header("Debug")]
        [SerializeField]
        private bool logStateChanges = true;

        private bool _portalActivated;

        private void Awake()
        {
            if (bossHealth == null)
            {
                Debug.LogError(
                    "[BossDeathPortalActivator] Boss Health reference is missing.",
                    this
                );
            }

            if (returnPortal == null)
            {
                Debug.LogError(
                    "[BossDeathPortalActivator] Return Portal reference is missing.",
                    this
                );

                return;
            }

            // Portal must begin unavailable while the boss is alive.
            returnPortal.SetActive(false);

            if (logStateChanges)
            {
                Debug.Log(
                    "[BossDeathPortalActivator] Return portal initialized as inactive.",
                    returnPortal
                );
            }
        }

        private void Update()
        {
            if (_portalActivated || bossHealth == null || returnPortal == null)
            {
                return;
            }

            if (bossHealth.CurrentHealth > 0f)
            {
                return;
            }

            ActivatePortal();
        }

        private void ActivatePortal()
        {
            _portalActivated = true;
            returnPortal.SetActive(true);

            if (logStateChanges)
            {
                Debug.Log(
                    "[BossDeathPortalActivator] Boss defeated. Return portal activated.",
                    returnPortal
                );
            }
        }
    }
}