using System.Linq;
using CodeBase.Data;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawners;
using CodeBase.Logic.LevelTransfer;
using CodeBase.Logic.Potions;
using CodeBase.Logic.SaveTriggers;
using CodeBase.StaticData;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Editor
{
    [CustomEditor(typeof(LevelStaticData))]
    public class LevelStaticDataEditor : UnityEditor.Editor
    {
        private const string HeroInitialPointTag = "PlayerInitialPoint";
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            LevelStaticData levelData = (LevelStaticData)target;

            if (GUILayout.Button("Collect all"))
            {
                CollectEnemySpawners(levelData);
                CollectSaveTriggers(levelData);
                CollectLevelTransfers(levelData);
                CollectHealthPotions(levelData);

                UpdateSceneKey(levelData);
                UpdateHeroInitialPosition(levelData);
                
                EditorUtility.SetDirty(target);
            }
        }

        private static void CollectLevelTransfers(LevelStaticData levelData)
        {
            levelData.LevelTransfers = FindObjectsOfType<LevelTransferMarker>()
                .Select(x => new LevelTransferData(
                    x.GetComponent<UniqueId>().Id,
                    x.TransferTo,
                    x.IsActive,
                    x.transform.position.AsVectorData(),
                    x.GetComponent<BoxCollider>().size.AsVectorData(),
                    x.GetComponent<BoxCollider>().center.AsVectorData(),
                    new Vector3(x.transform.eulerAngles.x, x.transform.eulerAngles.y, x.transform.eulerAngles.z).AsVectorData(),
                    x.PayloadSpawnMarkers.Select(y => new PayloadSpawnMarkerData(y.GetComponent<UniqueId>().Id, y.transform.position.AsVectorData(), y.MonsterTypeId))
                        .ToList()))
                .ToList();
        }

        private static void CollectSaveTriggers(LevelStaticData levelData)
        {
            levelData.SaveTriggers = FindObjectsOfType<SaveMarker>()
                .Select(x => new SaveTriggerData(
                    x.GetComponent<UniqueId>().Id,
                    x.transform.position.AsVectorData(),
                    x.GetComponent<BoxCollider>().size.AsVectorData(),
                    x.GetComponent<BoxCollider>().center.AsVectorData()))
                .ToList();
        }

        private static void CollectEnemySpawners(LevelStaticData levelData)
        {
            levelData.EnemySpawners = FindObjectsOfType<SpawnMarker>()
                .Select(x =>
                    new EnemySpawnerData(x.GetComponent<UniqueId>().Id, x.MonsterTypeId, x.transform.position.AsVectorData()))
                .ToList();
        }

        private void CollectHealthPotions(LevelStaticData levelData)
        {
            levelData.HealthPotions = FindObjectsOfType<HealthPotionMarker>()
                .Select(x => new HealthPotionData(
                    x.GetComponent<UniqueId>().Id,
                    x.Healing,
                    x.transform.position.AsVectorData()))
                .ToList();
        }

        private static void UpdateHeroInitialPosition(LevelStaticData levelData) =>
            levelData.HeroInitialPosition = GameObject.FindWithTag(HeroInitialPointTag).transform.position;

        private static void UpdateSceneKey(LevelStaticData levelData) => 
            levelData.LevelKey = SceneManager.GetActiveScene().name;
    }
}