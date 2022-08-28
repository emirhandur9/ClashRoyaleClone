using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeeleSoldier : SoldierBase
{
    private MeeleWeapon weapon;

    private void Awake()
    {
        if (!IsPlayer)
            Initialize();
    }
    public override void Initialize()
    {
        base.Initialize();
        weapon = GetComponentInChildren<MeeleWeapon>();
        weapon.Initialize(Power);
    }
    public override void Attack(IDamageable enemy)
    {
        UpdateTarget(enemy.Transform.position);
        weapon.AttackBehaviour(enemy);
    }

    public override void NoEnemyFounded()
    {
        base.NoEnemyFounded();
        weapon.AttackStopped();
    }

}
