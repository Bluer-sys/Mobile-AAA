using System;
using CodeBase.Infrastructure.Factory;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class RotateToHero : Follow
    {
        public float AngularSpeed;

        private IGameFactory _gameFactory;

        private void Update()
        {
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
            HeroTransform.position - transform.position;
    }
}
