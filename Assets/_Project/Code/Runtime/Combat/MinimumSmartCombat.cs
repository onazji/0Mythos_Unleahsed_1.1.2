using MoreMountains.TopDownEngine;
using UnityEngine;

namespace Mythos.Unleashed.Runtime
{
    /// <summary>
    /// Minimum movement-first combat prototype.
    /// Selects the nearest enemy inside an awareness radius,
    /// aims the equipped weapon, and fires automatically.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterHandleWeapon))]
    public sealed class MinimumSmartCombat : MonoBehaviour
    {
        [Header("Awareness")]
        [SerializeField]
        private float awarenessRadius = 12f;

        [SerializeField]
        private LayerMask enemyLayers;

        [Header("Aim")]
        [SerializeField]
        private float aimHeightOffset = 0.75f;

        [Header("Debug")]
        [SerializeField]
        private bool logTargetChanges = true;

        private CharacterHandleWeapon _weaponHandler;
        private Health _currentTarget;

        private void Awake()
        {
            _weaponHandler = GetComponent<CharacterHandleWeapon>();

            _weaponHandler.ForceWeaponAimControl = true;
            _weaponHandler.ForcedWeaponAimControl = WeaponAim.AimControls.Script;
            _weaponHandler.ForceAlwaysShoot = false;
        }

        private void Update()
        {
            if (!TargetIsValid(_currentTarget))
            {
                SetTarget(FindNearestTarget());
            }

            if (_currentTarget == null)
            {
                StopAutomaticFire();
                return;
            }

            WeaponAim weaponAim = _weaponHandler.WeaponAimComponent;

            if (weaponAim == null)
            {
                StopAutomaticFire();
                return;
            }

            Vector3 targetPosition =
                _currentTarget.transform.position +
                Vector3.up * aimHeightOffset;

            weaponAim.SetCurrentAim(targetPosition);
            _weaponHandler.ForceAlwaysShoot = true;
        }

        private Health FindNearestTarget()
        {
            Collider[] hits = Physics.OverlapSphere(
                transform.position,
                awarenessRadius,
                enemyLayers,
                QueryTriggerInteraction.Ignore
            );

            Health nearestTarget = null;
            float nearestDistanceSquared = float.PositiveInfinity;

            foreach (Collider hit in hits)
            {
                Health candidate = hit.GetComponentInParent<Health>();

                if (!TargetIsValid(candidate))
                {
                    continue;
                }

                float distanceSquared =
                    (candidate.transform.position - transform.position).sqrMagnitude;

                if (distanceSquared >= nearestDistanceSquared)
                {
                    continue;
                }

                nearestTarget = candidate;
                nearestDistanceSquared = distanceSquared;
            }

            return nearestTarget;
        }

        private bool TargetIsValid(Health target)
        {
            if (target == null)
            {
                return false;
            }

            if (!target.gameObject.activeInHierarchy)
            {
                return false;
            }

            if (target.CurrentHealth <= 0f)
            {
                return false;
            }

            float distanceSquared =
                (target.transform.position - transform.position).sqrMagnitude;

            return distanceSquared <= awarenessRadius * awarenessRadius;
        }

        private void SetTarget(Health newTarget)
        {
            if (_currentTarget == newTarget)
            {
                return;
            }

            _currentTarget = newTarget;

            if (logTargetChanges)
            {
                string targetName =
                    _currentTarget != null ? _currentTarget.name : "none";

                Debug.Log(
                    $"[MinimumSmartCombat] Current target: {targetName}.",
                    this
                );
            }
        }

        private void StopAutomaticFire()
        {
            if (_weaponHandler == null)
            {
                return;
            }

            _weaponHandler.ForceAlwaysShoot = false;

            if (_weaponHandler.CurrentWeapon != null)
            {
                _weaponHandler.CurrentWeapon.WeaponInputStop();
            }
        }

        private void OnDisable()
        {
            StopAutomaticFire();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, awarenessRadius);
        }
    }
}