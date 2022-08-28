using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour, IDamageable
{
    [SerializeField] Image healthBarImg;
    public int CurrentHealth { get; set; }

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] bool isPlayerTower;
    [SerializeField] LayerMask AttackableLayer;
    [SerializeField] float AttackRadius;
    [SerializeField] float enemyCheckTime;
    [SerializeField] float attackDelay;
    [SerializeField] int damageAmount;
    public bool isBaseTower;
    public bool IsPlayer { get => isPlayerTower;  set { isPlayerTower = value; } }

    public Transform Transform { get => transform; }

    private bool isAttacking;

    private void Awake()
    {
        CurrentHealth = 25;

        StartCoroutine(EnemyCheckCoroutine());
    }
    public void GetDamage(int damageAmount)
    {
        CurrentHealth -= damageAmount;
        UpdateHeatlhBar();
        if (CurrentHealth <= 0)
            DestroyTower();
    }
    protected void UpdateHeatlhBar()
    {
        healthBarImg.fillAmount = CurrentHealth / 25.0f;
    }

    public void DestroyTower()
    {
        if (isBaseTower)
            GameManager.Instance.GameFinished(isPlayerTower);

        Destroy(this.gameObject);
    }
    public IEnumerator EnemyCheckCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(enemyCheckTime);

            var enemy = CheckForEnemy();
            if (enemy != null && !isAttacking)
            {
                Attack(enemy);
            }

        }
    }

    public void Attack(SoldierBase enemy)
    {
        Bullet bullet = Instantiate(bulletPrefab).GetComponent<Bullet>();
        bullet.target = enemy;
        bullet.transform.position = transform.position;
        bullet.DamageAmount = damageAmount;
        isAttacking = true;
        StartCoroutine(AttackDelayWaiter());
    }

    private IEnumerator AttackDelayWaiter()
    {
        yield return new WaitForSeconds(attackDelay);
        isAttacking = false;
    }
    public SoldierBase CheckForEnemy()
    {
        Collider[] enemyColliders = Physics.OverlapSphere(transform.position, AttackRadius, AttackableLayer);

        foreach (var coll in enemyColliders)
        {
            var enemy = coll.GetComponent<SoldierBase>();
            if (enemy.IsPlayer != isPlayerTower)
            {
                return enemy;
            }
        }
        return null;
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, AttackRadius);
    }
}
