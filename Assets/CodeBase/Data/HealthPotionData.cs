using System;

namespace CodeBase.Data
{
    [Serializable]
    public class HealthPotionData
    {
        public string Id;
        public int Healing;
        public Vector3Data Position;

        public HealthPotionData(string id, int healing, Vector3Data position)
        {
            Id = id;
            Healing = healing;
            Position = position;
        }
    }
}