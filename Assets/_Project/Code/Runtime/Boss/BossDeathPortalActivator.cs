using MoreMountains.TopDownEngine;
using UnityEngine;

namespace Mythos.Unleashed.Runtime
{
    /// <summary>
    /// Watches a boss Health component and activates the return portal
    /// and relic reward once the boss reaches zero health.
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

        [Header("Relic Reward")]
        [SerializeField]
        private GameObject relicReward;

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
            }
            else
            {
                returnPortal.SetActive(false);
            }

            if (relicReward == null)
            {
                Debug.LogError(
                    "[BossDeathPortalActivator] Relic Reward reference is missing.",
                    this
                );
            }
            else
            {
                relicReward.SetActive(false);
            }

            if (
                logStateChanges &&
                returnPortal != null &&
                relicReward != null
            )
            {
                Debug.Log(
                    "[BossDeathPortalActivator] Return portal and relic reward initialized as inactive.",
                    this
                );
            }
        }

        private void Update()
        {
            if (
                _portalActivated ||
                bossHealth == null ||
                returnPortal == null ||
                relicReward == null
            )
            {
                return;
            }

            if (bossHealth.CurrentHealth > 0f)
            {
                return;
            }

            ActivatePortalAndReward();
        }

        private void ActivatePortalAndReward()
        {
            _portalActivated = true;

            returnPortal.SetActive(true);
            relicReward.SetActive(true);

            if (logStateChanges)
            {
                Debug.Log(
                    "[BossDeathPortalActivator] Boss defeated. Return portal and relic reward activated.",
                    this
                );
            }
        }
    }
}