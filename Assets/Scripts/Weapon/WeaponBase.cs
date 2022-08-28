using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    protected int DamageAmount { get; set; }
    [field: SerializeField] protected float AttackSpeed { get; set; }
    [field: SerializeField] protected float AttackDelay { get; set; }

    protected Animator animator;

    public abstract void AttackBehaviour(IDamageable target);
    public abstract void AttackStopped();

    public virtual void Initialize(int damageAmount)
    {
        animator = GetComponent<Animator>();
        DamageAmount = damageAmount;
    }

}
