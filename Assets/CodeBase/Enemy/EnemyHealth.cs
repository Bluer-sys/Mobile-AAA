using System;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class EnemyHealth : MonoBehaviour, IHealth
    {
        [SerializeField] private int _current;
        [SerializeField] private int _max;
        
        private EnemyAnimator _animator;

        public event Action HealthChanged;

        private void OnHitEnded()
        {
            _animator.SetIsAttacking(false);
        }
        
        public int Current
        {
            get => _current;
            set => _current = value;
        }

        public int Max
        {
            get => _max;
            set => _max = value;
        }

        private void Awake()
        {
            _animator = GetComponent<EnemyAnimator>();
        }

        private void Start()
        {
            HealthChanged?.Invoke();
        }

        public void TakeDamage(int damage)
        {
            Current -= damage;
            _animator.PlayHit();
            
            HealthChanged?.Invoke();
        }
    }
}