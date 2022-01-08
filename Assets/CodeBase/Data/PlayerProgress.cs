using System;

namespace CodeBase.Data
{
    [Serializable]
    public class PlayerProgress
    {
        public State HeroState;
        public WorldData WorldData;
        public Stats HeroStats;
        public KillData KillData;
        public PurchaseData PurchaseData;
        public ActivatedSaveTriggersData ActivatedSaveTriggersData;
        public ActiveLevelTransfersData ActiveLevelTransfersData;
        public PickedPotionsData PickedPotionsData;

        public PlayerProgress(string initialLevel)
        {
            WorldData = new WorldData(initialLevel);
            HeroState = new State();
            HeroStats = new Stats();
            KillData = new KillData();
            PurchaseData = new PurchaseData();
            ActivatedSaveTriggersData = new ActivatedSaveTriggersData();
            ActiveLevelTransfersData = new ActiveLevelTransfersData();
            PickedPotionsData = new PickedPotionsData();
        }
    }
}