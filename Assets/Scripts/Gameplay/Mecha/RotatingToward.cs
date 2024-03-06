using System;
using System.Collections;
using Gameplay.Units;
using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay.Mecha
{
    public class RotatingToward : MonoBehaviour
    {
        [Header("Rotating Weapon Settings")]
        [SerializeField] protected Transform target;
        [SerializeField] public Transform turret;
        [SerializeField] private Transform turretBase;
        [SerializeField] private float rotationSpeed = 10;

        [Header("Rotation Limits Y")] 
        // relative to the initial rotation
        [SerializeField] public Vector2 rotationLimitsY = new Vector2(-90, 90);
        [SerializeField] public bool reverseRotationY;
        
        [Header("Rotation Limits X")]
        [SerializeField] public Vector2 rotationLimitsX = new Vector2(-45, 45);
        [SerializeField] public bool reverseRotationX;
        
        [Header("Debug")] public bool showAngles;
        public Color colorY = Color.green;
        public Color colorX = Color.red;
        public float radius = 5f;
        
        private Vector3 _initialRotation;

        private Vector3 _initialDirection;
        private Vector3 _initialUp;

        protected void Awake()
        {
            //base.Awake();
            _initialRotation = turret.localEulerAngles;
            _initialDirection = turret.forward;
            _initialUp = turret.up;
            
            StartCoroutine(RotateToTarget());
        }

        private static float ReverseClamp(float val, float low, float high)
        {
            if (val > low && val < high)
            {
                float mid = (high - low) / 2 + low;
                return val < mid ? low : high;
            }
            return val;
        }
        
        private delegate float ClampFunction(float val, float low, float high);
        
        private static (float lower, float upper) GetBoundsAngle(Vector2 limits, float initialRotationAxis)
        {
            float lower = (initialRotationAxis + limits.x + 360) % 360;
            float upper = (initialRotationAxis + limits.y + 360) % 360;
            if (lower > upper)
                (lower, upper) = (upper, lower);
            return (lower, upper);
        }
        
        public void SetTarget(Transform newTarget)
        {
            target = newTarget;
        }
        
        public void SetTarget(TargetInfo newTarget)
        {
            if (newTarget != null && newTarget.Unit != null)
                target = newTarget.Unit.transform;
            else
                target = null;
        }
        
        public virtual void SetTarget(Unit newTarget)
        {
            if (newTarget != null)
                target = newTarget.transform;
            else
                target = null;
        }
        /// <summary>
        /// Constantly rotates the turret to the target
        /// Go back to initial position if there is no target
        /// Takes into account the rotation limits
        /// </summary>
        /// <returns></returns>
        private IEnumerator RotateToTarget()
        {
            var initialRotation = Quaternion.Euler(_initialRotation);
            // we need to work with angle between 0 and 360
            (float lowerEulerY, float upperEulerY) = GetBoundsAngle(rotationLimitsY, _initialRotation.y);
            (float lowerEulerX, float upperEulerX) = GetBoundsAngle(rotationLimitsX, _initialRotation.x);
            
            // Perhaps we want the outer angle and not the inner
            ClampFunction clampFunctionY = reverseRotationY ? ReverseClamp : Mathf.Clamp;
            ClampFunction clampFunctionX = reverseRotationX ? ReverseClamp : Mathf.Clamp;
            
            while (true)
            {
                if (target)
                {
                    var targetDirection = target.position - turret.position;

                    var yAngle = Vector3.SignedAngle(turret.forward, targetDirection, turret.up);
                    Debug.Log(name + " yAngle: " + yAngle);
                    var rotation = Quaternion.LookRotation(targetDirection);
                    
                    Debug.DrawRay(turret.position, rotation * Vector3.forward * 5, Color.green);
                    Debug.DrawRay(turret.position, turret.rotation * Vector3.forward * 10, Color.yellow);

                    var stepRotation = rotation;//Quaternion.RotateTowards(turret.rotation, rotation, Time.fixedDeltaTime * rotationSpeed);
                    Debug.DrawRay(turret.position, stepRotation * Vector3.forward * 5, Color.blue);
                    stepRotation = stepRotation * Quaternion.Inverse(turretBase.rotation);
                    
                    var localRotation = (Quaternion.Inverse(turret.parent.rotation) * rotation).normalized;

                    localRotation = Quaternion.RotateTowards(turret.localRotation, localRotation,
                        Time.fixedDeltaTime * rotationSpeed);
                    var euler = localRotation.eulerAngles; 
                    euler.y = clampFunctionY(euler.y % 360, lowerEulerY, upperEulerY);
                    euler.x = clampFunctionX(euler.x, lowerEulerX, upperEulerX);
                    //euler.z = 0;
                    turret.localRotation = Quaternion.Euler(euler);
                    Debug.DrawRay(turret.position, turret.parent.rotation * localRotation * Vector3.forward * 5 , Color.red);
                }
                else
                {
                    turret.localRotation = Quaternion.Slerp(turret.localRotation, initialRotation, Time.fixedDeltaTime * rotationSpeed);
                }
                yield return new WaitForFixedUpdate();
            }
        }

    }
}