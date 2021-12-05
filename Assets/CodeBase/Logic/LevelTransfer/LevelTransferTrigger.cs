using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.States;
using UnityEngine;

namespace CodeBase.Logic.LevelTransfer
{
    public class LevelTransferTrigger : MonoBehaviour, ISavedProgress
    {
        private const string PlayerTag = "Player";

        public BoxCollider Collider;
        public string TransferTo;
        public bool IsActive;
        
        private IGameStateMachine _stateMachine;
        private bool _triggered;
        
        public string Id { get; set; }

        public void Construct(IGameStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        private void Awake()
        {
            Collider = GetComponent<BoxCollider>();
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (_triggered)
                return;

            if (other.CompareTag(PlayerTag))
            {
                _triggered = true;
                _stateMachine.Enter<LoadLevelState, string>(TransferTo);
            }
        }
        
        private void OnDrawGizmos()
        {
            if(Collider == null)
                return;
            
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(transform.position + Collider.center, Collider.size);
        }

        public void SetActive()
        {
            IsActive = true;
            gameObject.SetActive(true);
        }
        
        public void LoadProgress(PlayerProgress progress)
        {
            if (progress.ActiveLevelTransfersData.TriggersId.Remove(Id))
            {
                IsActive = true;
                gameObject.SetActive(true);
            }
        }

        public void SaveProgress(PlayerProgress progress)
        {
            if (IsActive)
            {
                progress.ActiveLevelTransfersData.TriggersId.Add(Id);
            }
        }
    }
}