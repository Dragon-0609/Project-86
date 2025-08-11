using System.Linq;
using Gameplay;
using UnityEngine;
namespace BladesCombat
{
    public class BladeCollision : BladeComponent
    {
        public override bool UseTriggers => true;

        public override void OnTriggerEnter(Collider other, object additionalData = null)
        {
            if (additionalData is not TriggerData data)
            {
                Debug.Log($"Trigger not from blade. Must be for something else");
                return;
            }

            bool isPlayer = SharedData.Colliders.Contains(other);
            if (isPlayer) return;

            IHealth health = other.GetComponent<IHealth>();
            if (health == null)
            {
                return;
            }
            
            bool isFriendlyUnit = health.Faction == Faction.Republic;

            if (isFriendlyUnit)
            {
                if (data.IsLeftBlade && !Switcher.IsLeftActive)
                {
                    return;
                }
                if (!data.IsLeftBlade && !Switcher.IsRightActive)
                {
                    return;
                }
            }

            health.TakeDamage(new DamagePackage()
            {
                BulletSize = 0,
                DamageAmount = SharedData.FullDamage,
                DamageAudioClip = null,
                DamageSourcePosition = Vector3.zero,
                IsBullet = false,
                Faction = Faction.Republic
            });

        }

    }
}