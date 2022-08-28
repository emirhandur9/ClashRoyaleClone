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
        if ((lastTarget as UnityEngine.Object))
        {
            lastTarget.GetDamage(DamageAmount);
        }

    }
}
