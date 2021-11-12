using System.Collections;
using UnityEngine;

namespace CodeBase.Enemy
{
    public class Aggro : MonoBehaviour
    {
        public TriggerObserver TriggerObserver;
        public Follow Follow;

        public float Cooldown;

        private WaitForSeconds _switchFollowOffAfterCooldown;
        private Coroutine _aggroCoroutine;
        private bool _hasAggroTarget;

        private void Start()
        {
            _switchFollowOffAfterCooldown = new WaitForSeconds(Cooldown);

            TriggerObserver.TriggerEnter += TriggerEnter;
            TriggerObserver.TriggerExit += TriggerExit;

            Follow.enabled = false;
        }

        private void OnDestroy()
        {
            TriggerObserver.TriggerEnter -= TriggerEnter;
            TriggerObserver.TriggerExit -= TriggerExit;
        }

        private void TriggerEnter(Collider obj)
        {
            if (_hasAggroTarget) return;

            _hasAggroTarget = true;

            TryStopAggroCoroutine();

            SwitchFollowOn();
        }

        private void TriggerExit(Collider obj)
        {
            if (!_hasAggroTarget) return;

            _hasAggroTarget = false;

            _aggroCoroutine = StartCoroutine(SwitchFollowOffAfterCooldown());
        }

        private void TryStopAggroCoroutine()
        {
            if (_aggroCoroutine == null) return;

            StopCoroutine(_aggroCoroutine);
            _aggroCoroutine = null;
        }

        private IEnumerator SwitchFollowOffAfterCooldown()
        {
            yield return _switchFollowOffAfterCooldown;

            SwitchFollowOff();
        }

        private void SwitchFollowOn() => 
            Follow.enabled = true;

        private void SwitchFollowOff() => 
            Follow.enabled = false;
    }
}
