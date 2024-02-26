using UnityEngine;

namespace Utility
{
    public class CopyRotation : MonoBehaviour
    {
        public Transform target;
        
        private void Update()
        {
            transform.rotation = target.rotation;
        }
        
    }
}