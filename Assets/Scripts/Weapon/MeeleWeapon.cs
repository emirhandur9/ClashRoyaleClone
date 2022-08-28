using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MeeleWeapon : WeaponBase
{
    private IDamageable lastTarget;
    public override void AttackBehaviour(IDamageable target)
    {
        lastTarget = target;
        animator.SetBool("isAttack", true);
    }

    public override void AttackStopped()
    {
        animator.SetBool("isAttack", false);
    }

    public void WeaponHitAnimEvent()
    {
        try
        {
            lastTarget.GetDamage(DamageAmount);
        }
        catch (NullReferenceException ex)
        {
            Debug.Log("Enemy dead");
        }
    }
}
