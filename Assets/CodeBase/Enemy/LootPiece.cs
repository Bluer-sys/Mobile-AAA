using System;
using System.Collections;
using CodeBase.Data;
using TMPro;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class LootPiece : MonoBehaviour
    {
        public GameObject PickupPopup;
        public GameObject Skull;
        public ParticleSystem PickupFX;
        public TextMeshPro LootText;
        
        private Loot _loot;
        private bool _picked;
        private WorldData _worldData;

        public void Construct(WorldData worldData)
        {
            _worldData = worldData;
        }

        public void Initialize(Loot  loot)
        {
            _loot = loot;
        }

        private void OnTriggerEnter(Collider other)
        {
            Pickup();
        }

        private void Pickup()
        {
            if(_picked)
                return;
            
            _picked = true;

            UpdateWorldData();
            HideSkull();
            PlayFX();
            ShowText();
            StartCoroutine(StartDestroyTimer());
        }

        private void UpdateWorldData() => 
            _worldData.LootData.Collect(_loot);

        private void HideSkull() => 
            Skull.SetActive(false);

        private void PlayFX() => 
            Instantiate(PickupFX, transform.position, Quaternion.identity);

        private void ShowText()
        {
            LootText.text = _loot.Value.ToString();
            PickupPopup.SetActive(true);
        }

        private IEnumerator StartDestroyTimer()
        {
            yield return new WaitForSeconds(2.0f);
            Destroy(gameObject);
        }
    }
}