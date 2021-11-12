using System;
using UnityEngine;

namespace CodeBase.Enemy
{
    [RequireComponent(typeof(Collider))]
    public class TriggerObserver : MonoBehaviour
    {
        public Action<Collider> TriggerEnter;
        public Action<Collider> TriggerExit;

        private void OnTriggerEnter(Collider other)
        {
            TriggerEnter?.Invoke(other);
        }

        private void OnTriggerExit(Collider other)
        {
            TriggerExit?.Invoke(other);
        }
    }
}
