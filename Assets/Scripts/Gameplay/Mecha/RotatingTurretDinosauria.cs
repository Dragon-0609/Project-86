using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay.Units;
using UnityEngine;

namespace Gameplay.Mecha
{
    public class RotatingTurretDinosauria : RotatingToward
    {
        [Header("Dinosauria Additional settings")]
        public float shootMinAngle = 5f;
        public WeaponModule weaponModule;

        private void Start()
        {
            StartCoroutine(weaponModule.ShootHoldUnlimited(CanShoot));
        }

        public bool CanShoot()
        {
            return target && Vector3.Angle(turret.forward, target.position - turret.position) < shootMinAngle;
        }
    }
}