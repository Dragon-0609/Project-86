using System;
using AI.BehaviourTree;
using Gameplay.Mecha;
using UnityEditor;
using UnityEngine;

namespace Editor.GameplayEditor
{
    [CustomEditor(typeof(RotatingToward))]
    public class RotatingTowardEditor : UnityEditor.Editor
    {
        private void OnSceneGUI()
        {

            var rotatingToward = (RotatingToward)target;
            if (!rotatingToward)
                return;
            var detectionPoint = rotatingToward.turret;
            if (!detectionPoint)
                return;
            var angleY = Quaternion.Euler(0, rotatingToward.rotationLimitsY.x, 0) * detectionPoint.forward;
            Handles.color = rotatingToward.colorY;
            Handles.DrawSolidArc(detectionPoint.position, detectionPoint.up, angleY,
                rotatingToward.rotationLimitsY.y - rotatingToward.rotationLimitsY.x,
                rotatingToward.radius);

            var angleX = Quaternion.Euler(rotatingToward.rotationLimitsX.x,0, 0) * detectionPoint.forward;
            Handles.color = rotatingToward.colorX;
            Handles.DrawSolidArc(detectionPoint.position, detectionPoint.right, angleX,
                rotatingToward.rotationLimitsX.y - rotatingToward.rotationLimitsX.x,
                rotatingToward.radius);
        }

    }
}