using AI.BehaviourTree;
using UnityEditor;
using UnityEngine;

namespace Editor.GameplayEditor
{
    [CustomEditor(typeof(EnemyDetector))]
    public class EnemyDetectorEditor : UnityEditor.Editor
    {
        private void OnSceneGUI()
        {
            
            var enemyDetector = (EnemyDetector)target;
            if (!enemyDetector.showVision)
                return;
            var detectionPoint = enemyDetector.detectionPoint;
            if (!detectionPoint) 
                return; 
            var angle = Quaternion.Euler(0, -enemyDetector.fieldOfView / 2, 0) * detectionPoint.forward; 
            Handles.color = enemyDetector.color;
            Handles.DrawSolidArc(detectionPoint.position, detectionPoint.up, angle, enemyDetector.fieldOfView, enemyDetector.detectionRadius);
            
        }
    }
}