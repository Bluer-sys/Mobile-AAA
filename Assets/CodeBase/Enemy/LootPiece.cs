using System;
using System.Collections;
using System.Linq;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Enemy
{
    public class LootPiece : MonoBehaviour, ISavedProgress
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

        public void LoadProgress(PlayerProgress progress)
        {
        }

        public void SaveProgress(PlayerProgress progress)
        {
            if(_picked)
                return;

            LootOnLevel currentLevelLoot = GetLootDataOnLevel(progress) 
                                           ?? CreateCurrentLevelData(progress);

            AddLootPieceToData(currentLevelLoot);
        }

        private IEnumerator StartDestroyTimer()
        {
            yield return new WaitForSeconds(2.0f);
            Destroy(gameObject);
        }

        private LootOnLevel CreateCurrentLevelData(PlayerProgress progress)
        {
            progress.WorldData.LootOnLevel.Add(new LootOnLevel(CurrentSceneName()));
            return GetLootDataOnLevel(progress);
        }

        private void AddLootPieceToData(LootOnLevel currentLevelLoot) =>
            currentLevelLoot.LootsPiecesDatas.Add(new LootPiecesData(GetComponent<UniqueId>().Id, _loot,
                transform.position.AsVectorData()));

        private LootOnLevel GetLootDataOnLevel(PlayerProgress progress) =>
            progress.WorldData.LootOnLevel.FirstOrDefault(loot => loot.Level == CurrentSceneName());

        private string CurrentSceneName() =>
            SceneManager.GetActiveScene().name;
    }
}