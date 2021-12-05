using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(EnemyAttack))]
    public class CheckAttackRange : MonoBehaviour
    {
        public TriggerObserver TriggerObserver;
        
        private EnemyAttack enemyAttack;

        private void Awake()
        {
            enemyAttack = GetComponent<EnemyAttack>();
        }

        private void Start()
        {
            TriggerObserver.TriggerEnter += TriggerEnter;
            TriggerObserver.TriggerExit += TriggerExit;

            enemyAttack.DisableAttack();
        }

        private void TriggerEnter(Collider obj)
        {
            enemyAttack.EnableAttack();
        }

        private void TriggerExit(Collider obj)
        {
            enemyAttack.DisableAttack();
        }
    }
}