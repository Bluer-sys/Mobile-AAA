using System;
using System.Collections;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyHealth),typeof(EnemyAnimator))]
    [RequireComponent(typeof(Follow))]
    public class EnemyDeath: MonoBehaviour
    {
        private const float WaitBeforeDestroy = 5.0f;
        
        public ParticleSystem DeathFX;
        
        private EnemyHealth _enemyHealth;
        private EnemyAnimator _animator;
        private Follow _follow;
        private TriggerObserver[] _triggers;

        public event Action Happened;
        
        private void Awake()
        {
            _enemyHealth = GetComponent<EnemyHealth>();
            _animator = GetComponent<EnemyAnimator>();
            _follow = GetComponent<Follow>();
            _triggers = GetComponentsInChildren<TriggerObserver>();
        }

        private void Start()
        {
            _enemyHealth.HealthChanged += HealthChanged;
        }

        private void OnDestroy()
        {
            _enemyHealth.HealthChanged -= HealthChanged;
        }

        private void HealthChanged()
        {
            if (_enemyHealth.Current <= 0)
                Die();
        }

        private void Die()
        {
            _enemyHealth.HealthChanged -= HealthChanged;
            
            _animator.PlayDeath();
            DeactivateComponents();
            SpawnDeathFX();
            Happened?.Invoke();
            
            StartCoroutine(DestroyTimer());
        }

        private void DeactivateComponents()
        {
            _follow.enabled = false;
            foreach (var trigger in _triggers)
            {
                trigger.gameObject.SetActive(false);
            }
        }

        private IEnumerator DestroyTimer()
        {
            yield return new WaitForSeconds(WaitBeforeDestroy);
            Destroy(gameObject);
        }

        private void SpawnDeathFX() => 
            Instantiate(DeathFX, transform.position, Quaternion.identity);
    }
}