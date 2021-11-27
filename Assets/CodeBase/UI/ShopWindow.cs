using TMPro;

namespace CodeBase.UI
{
    public class ShopWindow : WindowBase
    {
        public TextMeshProUGUI SkullText;
        
        protected override void Initialize() =>
            RefreshSkullText();

        protected override void SubscribeUpdates() =>
            ProgressService.Progress.WorldData.LootData.Changed += RefreshSkullText;

        protected override void CleanUp()
        {
            base.CleanUp();
            ProgressService.Progress.WorldData.LootData.Changed -= RefreshSkullText;
        }

        private void RefreshSkullText() => 
            SkullText.text = ProgressService.Progress.WorldData.LootData.Collected.ToString();
    }
}