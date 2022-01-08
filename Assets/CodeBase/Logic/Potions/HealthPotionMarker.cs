using System;
using UnityEngine;

namespace CodeBase.Logic.Potions
{
    public class HealthPotionMarker : MonoBehaviour
    {
        public SphereCollider Collider;
        
        public int Healing;

        private void Awake()
        {
            Destroy(gameObject);
        }
        
        private void OnDrawGizmos()
        {
            if(Collider == null)
                return;
            
            Gizmos.color = Color.magenta;
            Gizmos.DrawSphere(transform.position + Collider.center, Collider.radius);
        }
    }
}