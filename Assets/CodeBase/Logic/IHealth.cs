using System;

namespace CodeBase.Logic
{
    public interface IHealth
    {
        event Action HealthChanged;
        int Current { get; set; }
        int Max { get; set; }
        void TakeDamage(int damage);
    }
}