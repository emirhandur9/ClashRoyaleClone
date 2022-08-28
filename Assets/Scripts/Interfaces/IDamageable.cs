using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageable
{
    public bool IsPlayer { get; set; }
    public Transform Transform { get; }
    public void GetDamage(int damageAmount);
}
