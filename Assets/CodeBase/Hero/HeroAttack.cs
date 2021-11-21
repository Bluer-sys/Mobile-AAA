using System;
using CodeBase.Data;
using CodeBase.Enemy;
using CodeBase.Infrastructure.Services;
using CodeBase.Infrastructure.Services.Input;
using CodeBase.Infrastructure.Services.PersistentProgress;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Hero
{
    [RequireComponent(typeof(HeroAnimator), typeof(CharacterController))]
    public class HeroAttack : MonoBehaviour, IHero, ISavedProgressReader
    {
        private const string HittableLayer = "Hittable";
        
        private CharacterController _heroController;
        private HeroAnimator _heroAnimator;
        private IInputService _input;
        private int _layerMask;
        private Collider[] _hits = new Collider[3];
        private Stats _stats;

        private void Awake()
        {
            _heroAnimator = GetComponent<HeroAnimator>();
            _heroController = GetComponent<CharacterController>();

            _input = AllServices.Container.Single<IInputService>();

            _layerMask = 1 << LayerMask.NameToLayer(HittableLayer);
        }

        private void Update()
        {
            if (_input.IsAttackButtonUp() && !_heroAnimator.IsAttacking)
                _heroAnimator.PlayAttack();
        }

        public void OnAttack()
        {
            for (int i = 0; i < Hit(); i++)
            {
                PhysicsDebug.DrawDebug(StartPoint(), _stats.DamageRadius, 1, Color.blue);
                _hits[i].transform.parent.GetComponent<IHealth>().TakeDamage(_stats.Damage);
            }
        }

        public void LoadProgress(PlayerProgress progress) => 
            _stats = progress.HeroStats;

        private int Hit() =>
            Physics.OverlapSphereNonAlloc(StartPoint(), _stats.DamageRadius, _hits, _layerMask);

        private Vector3 StartPoint() => 
            new Vector3(transform.position.x, _heroController.center.y / 2, transform.position.z) + transform.forward;
    }
}