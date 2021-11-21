using CodeBase.Data;
using CodeBase.Hero;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Infrastructure.Services.SaveLoad;
using UnityEngine;

namespace CodeBase.Logic
{
    public class SaveTrigger : MonoBehaviour, ISavedProgress
    {
        public BoxCollider Collider;

        private ISaveLoadService _saveLoadService;
        private bool _checked;

        private void Awake()
        {
            _saveLoadService = AllServices.Container.Single<ISaveLoadService>();
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
            if (!Collider)
                return;

            Gizmos.color = new Color32(30, 200, 30, 130);
            Gizmos.DrawCube(transform.position + Collider.center, Collider.size);
        }

        public void LoadProgress(PlayerProgress progress)
        {
            if (progress.ActivatedSaveTriggersData.triggersId.Contains(GetComponent<UniqueId>().Id))
            {
                _checked = true;
                gameObject.SetActive(false);
            }
        }

        public void SaveProgress(PlayerProgress progress)
        {
            if (_checked)
                progress.ActivatedSaveTriggersData.triggersId.Add(GetComponent<UniqueId>().Id);
        }
    }
}