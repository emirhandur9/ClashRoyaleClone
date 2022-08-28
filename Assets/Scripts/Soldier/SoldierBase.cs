using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public abstract class SoldierBase : MonoBehaviour, IDamageable
{

    [field: SerializeField] protected SoldierData soldierData;
    [field: SerializeField] protected LayerMask AttackableLayer { get; set; }
    [field: SerializeField] protected int Power { get; set; }
    [field: SerializeField] protected int AttackRadius { get; set; }


    [SerializeField] Image healthBarImg;
    [SerializeField] GameObject model;

    [SerializeField] float enemyCheckTime;

    [SerializeField] bool IsPlayerSoldier;
    public bool IsPlayer { get => IsPlayerSoldier; set { IsPlayerSoldier = value; } }

    public Transform Transform { get => transform; }
    public int CurrentHealth { get; set; }

    protected NavMeshAgent agentControl;
    protected Animator animator;
    public abstract void Attack(IDamageable enemy);
    

    public virtual void Initialize()
    {
        //Component
        animator = GetComponent<Animator>();
        agentControl = GetComponent<NavMeshAgent>();

        //Value
        CurrentHealth = soldierData.startHealth;

        //Func
        GoClosestTower();
        UpdateHeatlhBar();

        StartCoroutine(EnemyCheckCoroutine());
    }

    public IEnumerator EnemyCheckCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(enemyCheckTime);

            var enemy = CheckForEnemy();
            if (enemy != null)
                Attack(enemy);
            else
                NoEnemyFounded();
        }
    }

    public IDamageable CheckForEnemy()
    {
        Collider[] enemyColliders = Physics.OverlapSphere(transform.position, AttackRadius, AttackableLayer);

        foreach (var coll in enemyColliders)
        {
            var enemy = coll.GetComponent<IDamageable>();
            if (enemy.IsPlayer != IsPlayerSoldier)
            {
                var tower = coll.GetComponent<Tower>();
                if (tower != null && tower.isBaseTower && !GameManager.Instance.CheckCanAttackBaseTower(IsPlayerSoldier))
                    continue;
                
                return enemy;
            }
        }
        return null;
    }

    public virtual void NoEnemyFounded()
    {
        GoClosestTower();
    }

    public void GoClosestTower()
    {
        if (GameManager.Instance == null) return;

        Transform tower = GameManager.Instance.GetClosestTower(IsPlayerSoldier, transform.position);
        if (tower != null)
            UpdateTarget(tower.position);
    }
    public void UpdateTarget(Vector3 targetPos)
    {
        agentControl.SetDestination(targetPos);
    }
    protected void Death()
    {
        Destroy(this.gameObject);
    }

    protected void UpdateHeatlhBar()
    {
        healthBarImg.fillAmount = CurrentHealth / 5.0f;
    }

    public void GetDamage(int damageAmount)
    {
        CurrentHealth -= damageAmount;
        UpdateHeatlhBar();
        if (CurrentHealth <= 0)
            Death();
    }

    public void SetMaterial(Material material)
    {
        model.GetComponent<MeshRenderer>().material = material;
    }
}
