using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float movementSpeed;
    public int DamageAmount { get; set; }
    public SoldierBase target;

    private void Update()
    {
        if(target != null)
        {
            Vector3 dir = (target.transform.position - transform.position).normalized;
            transform.Translate(dir * Time.deltaTime * movementSpeed);

            float distance = Vector3.Distance(target.transform.position, transform.position);
            if (distance < .1f)
            {
                target.GetDamage(DamageAmount);
                Destroy(gameObject);
            }
        }

        if (target == null)
            Destroy(gameObject);
    }
}
