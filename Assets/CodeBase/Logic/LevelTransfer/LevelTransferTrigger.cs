using CodeBase.Infrastructure.Services.SaveLoad;
using CodeBase.Infrastructure.States;
using CodeBase.Logic.EnemySpawners;
using UnityEngine;

namespace CodeBase.Logic.LevelTransfer
{
    public class LevelTransferTrigger : MonoBehaviour
    {
        private const string PlayerTag = "Player";

        public BoxCollider Collider;
        public string TransferTo;
        public bool IsActive;
        public int PayloadSpawnMarkersCount;

        private IGameStateMachine _stateMachine;
        private ISaveLoadService _saveLoadService;
        
        private int _slainPayloadEnemyCount;
        private bool _triggered;

        public string Id { get; set; }

        public void Construct(IGameStateMachine stateMachine, ISaveLoadService saveLoadService)
        {
            _stateMachine = stateMachine;
            _saveLoadService = saveLoadService;
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
                other.transform.position = GetComponentInChildren<LoadZone>().transform.position;
                
                _saveLoadService.SaveProgress();
                Debug.Log("Progress Saved!");
                
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

        public void TryActivate(SpawnPoint spawnPoint)
        {
            _slainPayloadEnemyCount++;
            spawnPoint.DeathHappened -= TryActivate;
            
            if (_slainPayloadEnemyCount == PayloadSpawnMarkersCount)
            {
                SetActive();
            }
        }
    }
}