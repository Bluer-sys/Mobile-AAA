using System.Linq;
using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyAnimator))]
    public class EnemyAttack : MonoBehaviour
    {
        private const string PlayerLayerName = "Player";

        private readonly Collider[] _hits = new Collider[1];

        public float AttackCooldown = 3.0f;
        public float Cleavage = 0.5f;
        public float EffectiveDistance = 1.0f;
        public int Damage = 10;

        private EnemyAnimator _animator;
        private Transform _heroTransform;
        private float _currentAttackCooldown;
        private int _layerMask;
        private bool _attackIsActive;

        public void Construct(Transform heroTransform) =>
            _heroTransform = heroTransform;

        private void Awake()
        {
            _animator = GetComponent<EnemyAnimator>();

            _layerMask = 1 << LayerMask.NameToLayer(PlayerLayerName);
        }

        private void Update()
        {
            if (_currentAttackCooldown > 0)
            {
                DecreaseCurrentCooldown();
            }
            else if(!_animator.GetIsAttacking() && _attackIsActive)
            {
                StartAttack();
            }
        }

        private void OnAttack()
        {
            if (Hit(out Collider hit))
            {
                PhysicsDebug.DrawDebug(StartPoint(), Cleavage, 1, Color.red);
                hit.transform.GetComponent<IHealth>().TakeDamage(Damage);
            }
        }

        private void OnAttackEnded()
        {
            _currentAttackCooldown = AttackCooldown;

            _animator.SetIsAttacking(false);
        }

        private bool Hit(out Collider hit)
        {
            int hitsCount = Physics.OverlapSphereNonAlloc(StartPoint(), Cleavage, _hits, _layerMask);

            hit = _hits.FirstOrDefault();
            
            return hitsCount > 0;
        }

        public void EnableAttack() => 
            _attackIsActive = true;

        public void DisableAttack() => 
            _attackIsActive = false;

        private void StartAttack()
        {
            transform.LookAt(_heroTransform);
            _animator.PlayAttack();

            _animator.SetIsAttacking(true);
        }

        private Vector3 StartPoint() => 
            new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z) + transform.forward * EffectiveDistance;

        private void DecreaseCurrentCooldown() => 
            _currentAttackCooldown -= Time.deltaTime;
    }
}
