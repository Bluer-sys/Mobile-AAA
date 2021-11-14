using System;
using UnityEngine;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(HeroHealth))]
    [RequireComponent(typeof(HeroMove))]
    [RequireComponent(typeof(HeroAttack))]
    [RequireComponent(typeof(HeroAnimator))]
    public class HeroDeath : MonoBehaviour
    {
        public ParticleSystem DeathFX;

        private HeroHealth _heroHealth;
        private HeroMove _heroMove;
        private HeroAnimator _heroAnimator;
        private HeroAttack _heroAttack;
        private bool _isDead;

        private void Awake()
        {
            _heroHealth = GetComponent<HeroHealth>();
            _heroMove = GetComponent<HeroMove>();
            _heroAttack = GetComponent<HeroAttack>();
            _heroAnimator = GetComponent<HeroAnimator>();
        }

        private void Start()
        {
            _heroHealth.HealthChanged += HealthChanged;
        }

        private void OnDestroy()
        {
            _heroHealth.HealthChanged -= HealthChanged;
        }

        private void HealthChanged()
        {
            if (_heroHealth.Current <= 0)
                if (!_isDead)
                    Die();
        }

        private void Die()
        {
            _isDead = true;

            _heroMove.enabled = false;
            _heroAttack.enabled = false;
            _heroAnimator.PlayDeath();

            Instantiate(DeathFX, transform.position, Quaternion.identity);
        }
    }
}