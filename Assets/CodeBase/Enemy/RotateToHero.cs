using System;
using CodeBase.Infrastructure.Factory;
using CodeBase.Infrastructure.Services;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class RotateToHero : Follow
    {
        public float AngularSpeed;

        private IGameFactory _gameFactory;
        private Transform _heroTransform;

        private void Start()
        {
            _gameFactory = AllServices.Container.Single<IGameFactory>();

            if (_gameFactory.HeroGameObject != null)
            {
                InitializeHeroTransform();
            }
            else
            {
                _gameFactory.HeroCreated += OnHeroCreated;
            }
        }

        private void OnDestroy()
        {
            if (_gameFactory != null)
                _gameFactory.HeroCreated -= OnHeroCreated;
        }

        private void Update()
        {
            if (Initialized())
                RotateToHeroTransform();
        }

        private void RotateToHeroTransform()
        {
            Vector3 heroDirection = HeroDirection();
            // transform.rotation = Quaternion.LookRotation(heroDirection);

            if (RotationDifferenceY(heroDirection) > Constants.Epsilon)
            {
                transform.rotation = TargetRotationLerp(heroDirection);
            }
        }

        private Quaternion TargetRotationLerp(Vector3 heroDirection)
        {
            return Quaternion.Lerp(transform.rotation,
                Quaternion.LookRotation(heroDirection),
                AngularSpeed * Time.deltaTime);
        }

        private float RotationDifferenceY(Vector3 heroDirection) => 
            Math.Abs(transform.rotation.y - Quaternion.LookRotation(heroDirection).y);

        private Vector3 HeroDirection() =>
            _heroTransform.position - transform.position;

        private bool Initialized() =>
            _heroTransform != null;

        private void InitializeHeroTransform() =>
            _heroTransform = _gameFactory.HeroGameObject.transform;

        private void OnHeroCreated() =>
            InitializeHeroTransform();
    }
}
