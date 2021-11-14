using System;

namespace CodeBase.Data
{
    [Serializable]
    public class State
    {
        public int CurrentHP;
        public int MaxHP;

        public void ResetHP() => CurrentHP = MaxHP;

    }
}