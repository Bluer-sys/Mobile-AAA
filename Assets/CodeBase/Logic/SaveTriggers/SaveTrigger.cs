using CodeBase.Data;
using CodeBase.Hero;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using UnityEngine;

namespace CodeBase.Logic.SaveTriggers
{
    [RequireComponent(typeof(BoxCollider))]
    public class SaveTrigger : MonoBehaviour, ISavedProgress
    {
        public BoxCollider Collider;
        public string Id { get; set; }
        
        private ISaveLoadService _saveLoadService;
        private bool _checked;

        public void Construct(ISaveLoadService saveLoadService)
        {
            _saveLoadService = saveLoadService;
        }

        private void Awake()
        {
            Collider = GetComponent<BoxCollider>();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent(out IHero hero))
                return;

            if (_checked)
                return;

            _checked = true;

            _saveLoadService.SaveProgress();
            Debug.Log("Progress Saved!");
            gameObject.SetActive(false);
        }

        private void OnDrawGizmos()
        {
            if(Collider == null)
                return;
            
            Gizmos.color = new Color32(30, 200, 30, 130);
            Gizmos.DrawCube(transform.position + Collider.center, Collider.size);
        }
        
        public void LoadProgress(PlayerProgress progress)
        {
            if (progress.ActivatedSaveTriggersData.triggersId.Contains(Id))
            {
                _checked = true;
                gameObject.SetActive(false);
            }
        }

        public void SaveProgress(PlayerProgress progress)
        {
            if (_checked)
                progress.ActivatedSaveTriggersData.triggersId.Add(Id);
        }
    }
}