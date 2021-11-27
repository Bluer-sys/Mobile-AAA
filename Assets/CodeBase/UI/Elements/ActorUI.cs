﻿using CodeBase.Logic;
using UnityEngine;

namespace CodeBase.UI.Elements
{
    public class ActorUI : MonoBehaviour
    {
        public HpBar HpBar;

        private IHealth _health;

        private void Awake()
        {
            IHealth health = GetComponent<IHealth>();

            if (health != null)
                ConstructHealth(health);
        }

        private void OnDestroy()
        {
            //if (_health != null)
                _health.HealthChanged -= UpdateHpBar;
        }

        public void ConstructHealth(IHealth health)
        {
            _health = health;

            _health.HealthChanged += UpdateHpBar;
        }

        private void UpdateHpBar()
        {
            HpBar.SetValue(_health.Current, _health.Max);
        }
    }
}