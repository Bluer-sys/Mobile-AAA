using System;
using CodeBase.Data;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(HeroAnimator))]
    public class HeroHealth : MonoBehaviour, IHero, ISavedProgress, IHealth
    {
        private HeroAnimator _animator;
        private State _state;

        public event Action HealthChanged;

        private void Awake()
        {
            _animator = GetComponent<HeroAnimator>();
        }

        public int Current
        {
            get => _state.CurrentHP;
            set
            {
                if (_state.CurrentHP != value)
                {
                    _state.CurrentHP = value;
                    HealthChanged?.Invoke();
                }
            }
        }

        public int Max
        {
            get => _state.MaxHP;
            set => _state.MaxHP = value;
        }

        public void LoadProgress(PlayerProgress progress)
        {
            _state = progress.HeroState;
            HealthChanged?.Invoke();
        }

        public void SaveProgress(PlayerProgress progress)
        {
            progress.HeroState.CurrentHP = Current;
            progress.HeroState.MaxHP = Max;
        }

        public void TakeDamage(int damage)
        {
            if (Current <= 0)
            {
                return;
            }

            Current -= damage;
            _animator.PlayHit();
        }

        public void AddHP(int hp)
        {
            Current += hp;

            if (Current > Max)
                Current = Max;
        }
    }
}