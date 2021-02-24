using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TotallyNotEvil
{
    public interface IDamagable
    {
        bool CanTakeDamage { get; set; }
        int Health { get; set; }
        void TakeDamage(int dmg);
    }
}