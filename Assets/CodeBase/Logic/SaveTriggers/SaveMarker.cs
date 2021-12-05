using UnityEngine;

namespace CodeBase.Logic.SaveTriggers
{
    [RequireComponent(typeof(Collider))]
    public class SaveMarker : MonoBehaviour
    {
        public BoxCollider Collider;

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        private void OnDrawGizmos()
        {
            if(Collider == null)
                return;
            
            Gizmos.color = new Color32(30, 200, 30, 130);
            Gizmos.DrawCube(transform.position + Collider.center, Collider.size);
        }
    }
}