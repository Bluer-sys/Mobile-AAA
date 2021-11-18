using System;

namespace CodeBase.Data
{
    [Serializable]
    public class Loot
    {
        public int Value;

        public Loot(int value)
        {
            Value = value;
        }
    }
}