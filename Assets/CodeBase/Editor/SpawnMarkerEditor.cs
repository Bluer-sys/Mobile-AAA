using CodeBase.Logic.EnemySpawners;
using UnityEditor;
using UnityEngine;

namespace CodeBase.Editor
{
    [CustomEditor(typeof(SpawnMarker))]
    public class SpawnMarkerEditor : UnityEditor.Editor
    {
        [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
        public static void RenderCustomGizmoForSpawnMarker(SpawnMarker spawner, GizmoType gizmo)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(spawner.transform.position, 0.5f);
        }
        
        [DrawGizmo(GizmoType.Active | GizmoType.Pickable | GizmoType.NonSelected)]
        public static void RenderCustomGizmoForPayloadSpawnMarker(PayloadSpawnMarker spawner, GizmoType gizmo)
        {
            Gizmos.color = new Color(1,0.6f,0, 1);
            Gizmos.DrawSphere(spawner.transform.position, 0.5f);
        }
    }
}