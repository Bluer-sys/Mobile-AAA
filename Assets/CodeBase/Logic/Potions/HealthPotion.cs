using CodeBase.Data;
using CodeBase.Hero;
using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;

namespace CodeBase.Logic.Potions
{
    public class HealthPotion : MonoBehaviour, ISavedProgress
    {
        private string _uniqueId;
        private int _healing;
        
        private bool _pickedUp;

        public void Construnct(string id, int healing)
        {
            _uniqueId = id;
            _healing = healing;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out HeroHealth heroHealth) && !_pickedUp)
            {
                _pickedUp = true;
                heroHealth.AddHP(_healing);
                gameObject.SetActive(false);
            }
        }

        public void SaveProgress(PlayerProgress progress)
        {
            if (_pickedUp)
            {
                progress.PickedPotionsData.PotionsId.Add(_uniqueId);
            }
        }

        public void LoadProgress(PlayerProgress progress)
        {
        }
    }
}