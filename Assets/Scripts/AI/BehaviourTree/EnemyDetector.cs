using System;
using System.Collections;
using System.Collections.Generic;
using Gameplay;
using Gameplay.Units;
using UnityEngine;
using UnityEngine.Events;

namespace AI.BehaviourTree
{
    public class EnemyDetector : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private Faction enemyFaction;
        [SerializeField] private float idealDistance = 10f;
        [SerializeField] private float fieldOfView = 90f;
        [SerializeField] private float detectionRadius = 20f;
        [SerializeField] private LayerMask layerMask;
        [SerializeField] private float detectionRate = 0.5f;

        [SerializeField] private Transform detectionPoint;
        
        [Header("Events")]
        public UnityEvent<Unit> onTargetChanged;

        private List<Unit> _units;
        
        private Unit _target;
        
        private Unit Target
        {
            get => _target;
            set
            {
                if (value == _target) return;
                _target = value;
                onTargetChanged?.Invoke(_target);
            }
        }

        private void Awake()
        {
            _units = Factions.GetMembers(enemyFaction);
        }
        
        private void Start()
        {
            StartCoroutine(DetectionCoroutine());
        }

        private IEnumerator DetectionCoroutine()
        {
            while (true)
            {
                Unit idealTarget = null;
                float idealTargetDistance = float.MaxValue;
                foreach (var unit in _units)
                {
                    var distance = CanSeeTarget(unit);
                    if (distance > 0)
                    {
                        if (Mathf.Abs(distance - idealDistance) < idealTargetDistance)
                        {
                            idealTarget = unit;
                            idealTargetDistance = Mathf.Abs(distance - idealDistance);
                        }
                    }

                    yield return new WaitForSeconds(0.1f);
                }
                Target = idealTarget;
                yield return new WaitForSeconds(detectionRate);
            }
        }

        private float CanSeeTarget(Unit unit)
        {
            if (!unit)
                return -1;
            var direction = unit.transform.position - detectionPoint.position;
            
            float distance = direction.magnitude;

            if (distance > detectionRadius)
                return -1;

            direction.Normalize();
            var angle = Vector3.Angle(direction, detectionPoint.forward);
            if (angle > fieldOfView / 2)
                return -1;

            if (Physics.Raycast(detectionPoint.position, direction, out var hit, detectionRadius, layerMask))
            {
                if (hit.transform == unit.transform)
                {
                    return distance;
                }
            }

            return -1;
        }
    }
}