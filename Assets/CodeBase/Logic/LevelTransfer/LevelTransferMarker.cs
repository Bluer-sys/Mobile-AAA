using UnityEngine;

namespace CodeBase.Logic.LevelTransfer
{
    [RequireComponent(typeof(BoxCollider))]
    public class LevelTransferMarker : MonoBehaviour
    {
        public BoxCollider Collider;
        
        public string TransferTo;
        public bool IsActive;

        private void Awake()
        {
            Destroy(gameObject);
        }

        private void OnDrawGizmos()
        {
            if(Collider == null)
                return;
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(transform.position + Collider.center, Collider.size);
        }
    }
}
