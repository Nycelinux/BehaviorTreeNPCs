using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public float attackRange = 2f;
    public float attackCooldown = 1f;
    public int damage = 2;
    private float lastAttackTime;

    public Transform attackPoint;
    public LayerMask hitMask;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
            Attack();
    }

    void Attack()
    {
        if (Time.time < lastAttackTime + attackCooldown) return;
        Collider[] hits = Physics.OverlapSphere(attackPoint.position, attackRange, hitMask);
        foreach(var hit in hits)
        {
            Health hp = hit.GetComponentInParent<Health>();
            if (hp != null && !hp.isDead)
            {
                hp.TakeDamage(damage);
                ThreatSystem.AddThreat(hit.transform, transform, damage);
            }
        }
        lastAttackTime = Time.time;
    }
}
