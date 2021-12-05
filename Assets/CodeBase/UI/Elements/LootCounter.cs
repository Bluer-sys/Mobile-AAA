using CodeBase.Data;
using TMPro;
using UnityEngine;

namespace CodeBase.UI.Elements
{
    public class LootCounter : MonoBehaviour
    {
        public TextMeshProUGUI Counter;
        
        private WorldData _worldData;

        public void Construct(WorldData worldData)
        {
            _worldData = worldData;
            _worldData.LootData.Changed += UpdateCounter;
        }

        private void Start()
        {
            //Возможна проблема из-за Addressables с пропуском одного кадра в InstantiateAsync (nullRef - вызывается Start до Construct)
            UpdateCounter();
        }

        private void UpdateCounter()
        {
            Counter.text = _worldData.LootData.Collected.ToString();
        }
    }
}