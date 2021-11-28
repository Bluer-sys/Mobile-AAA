using System.Linq;
using CodeBase.Data;
using CodeBase.Logic;
using CodeBase.Logic.EnemySpawners;
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

            if (GUILayout.Button("Collect Spawners"))
            {
                levelData.EnemySpawners = FindObjectsOfType<SpawnMarker>()
                    .Select(x => new EnemySpawnerData(x.GetComponent<UniqueId>().Id, x.MonsterTypeId, x.transform.position.AsVectorData()))
                    .ToList();

                UpdateSceneKey(levelData);
                UpdateHeroInitialPosition(levelData);
                
                EditorUtility.SetDirty(target);
            }

            if (GUILayout.Button("Collect Save Triggers"))
            {
                levelData.SaveTriggers = FindObjectsOfType<SaveMarker>()
                    .Select(x => new SaveTriggerData(
                        x.GetComponent<UniqueId>().Id,
                        x.transform.position.AsVectorData(),
                        x.GetComponent<BoxCollider>().size.AsVectorData(),
                        x.GetComponent<BoxCollider>().center.AsVectorData()))
                    .ToList();
                
                UpdateSceneKey(levelData);
                UpdateHeroInitialPosition(levelData);
                
                EditorUtility.SetDirty(target);
            }
        }

        private static void UpdateHeroInitialPosition(LevelStaticData levelData) =>
            levelData.HeroInitialPosition = GameObject.FindWithTag(HeroInitialPointTag).transform.position;

        private static void UpdateSceneKey(LevelStaticData levelData) => 
            levelData.LevelKey = SceneManager.GetActiveScene().name;
    }
}