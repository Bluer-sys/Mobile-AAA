﻿using CodeBase.Infrastructure.Services.Ads;
using CodeBase.Infrastructure.Services.PersistentProgress;
using TMPro;

namespace CodeBase.UI.UIWindows.Shop
{
    public class ShopWindow : WindowBase
    {
        public TextMeshProUGUI SkullText;
        public RewardedAdItem AdItem;

        public void Construct(IPersistentProgressService progressService, IAdsService adsService)
        {
            base.Construct(progressService);
            AdItem.Construct(adsService, progressService);
        }
        
        protected override void Initialize()
        {
            AdItem.Initialize();
            RefreshSkullText();
        }

        protected override void SubscribeUpdates()
        {
            AdItem.Subscribe();
            ProgressService.Progress.WorldData.LootData.Changed += RefreshSkullText;
        }

        protected override void CleanUp()
        {
            base.CleanUp();
            AdItem.CleanUp();
            ProgressService.Progress.WorldData.LootData.Changed -= RefreshSkullText;
        }

        private void RefreshSkullText() => 
            SkullText.text = ProgressService.Progress.WorldData.LootData.Collected.ToString();
    }
}