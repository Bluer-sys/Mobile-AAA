﻿using CodeBase.Infrastructure.Services.PersistentProgress;
using UnityEngine;
using UnityEngine.UI;

namespace CodeBase.UI.UIWindows
{
    public abstract class WindowBase : MonoBehaviour
    {
        public Button CloseButton;
        
        protected IPersistentProgressService ProgressService;

        public void Construct(IPersistentProgressService progressService)
        {
            ProgressService = progressService;
        }
        
        private void Awake()
        {
            OnAwake();
        }

        private void Start()
        {
            Initialize();
            SubscribeUpdates();
        }

        private void OnDestroy()
        {
            CleanUp();
        }

        protected virtual void OnAwake()
        {
            CloseButton.onClick.AddListener(() => Destroy(gameObject));
        }

        protected virtual void Initialize()
        {
        }

        protected virtual void SubscribeUpdates()
        {
        }

        protected virtual void CleanUp()
        {
        }
    }
}