using System;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(Attack))]
    public class CheckAttackRange : MonoBehaviour
    {
        public TriggerObserver TriggerObserver;
        
        private Attack _attack;

        private void Awake()
        {
            _attack = GetComponent<Attack>();
        }

        private void Start()
        {
            TriggerObserver.TriggerEnter += TriggerEnter;
            TriggerObserver.TriggerExit += TriggerExit;

            _attack.DisableAttack();
        }

        private void TriggerEnter(Collider obj)
        {
            _attack.EnableAttack();
        }

        private void TriggerExit(Collider obj)
        {
            _attack.DisableAttack();
        }
    }
}