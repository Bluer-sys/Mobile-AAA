using CodeBase.Data;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class LootSpawner : MonoBehaviour
    {
        public EnemyDeath EnemyDeath;
        
        private IGameFactory _factory;
        private IRandomService _random;
        private int _minLoot;
        private int _maxLoot;

        public void Construct(IGameFactory factory, IRandomService random)
        {
            _factory = factory;
            _random = random;
        }
        
        private void Awake()
        {
            EnemyDeath.Happened += SpawnLoot;
        }

        private void OnDisable()
        {
            EnemyDeath.Happened -= SpawnLoot;
        }

        public void SetLoot(int min, int max)
        {
            _minLoot = min;
            _maxLoot = max;
        }

        private async void SpawnLoot()
        {
            LootPiece loot = await _factory.CreateLoot();
            loot.transform.position = transform.position;

            Loot lootItem = new Loot(_random.Next(_minLoot, _maxLoot));
            
            loot.Initialize(lootItem);
        }
    }
}