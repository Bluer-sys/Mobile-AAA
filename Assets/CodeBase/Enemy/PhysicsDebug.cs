using UnityEngine;

namespace CodeBase.Enemy
{
    public static class PhysicsDebug
    {
        public static void DrawDebug(Vector3 worldPos, float radius, float seconds, Color color)
        {
            Debug.DrawRay(worldPos, radius * Vector3.up, color, seconds);
            Debug.DrawRay(worldPos, radius * Vector3.down, color, seconds);
            Debug.DrawRay(worldPos, radius * Vector3.left, color, seconds);
            Debug.DrawRay(worldPos, radius * Vector3.right, color, seconds);
            Debug.DrawRay(worldPos, radius * Vector3.forward, color, seconds);
            Debug.DrawRay(worldPos, radius * Vector3.back, color, seconds);
        }
        
    }
}