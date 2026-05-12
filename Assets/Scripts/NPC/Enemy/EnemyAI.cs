using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    public float detectionRange = 10f;
    public float attackRange = 2f;
    public float attackCooldown = 1.5f;
    public int damage;
    private float lastAttackTime;
    private Transform target;
    private NavMeshAgent navAgent;


    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
    }

   
    void Update()
    {
        Transform threatTarget = ThreatSystem.GetHighestThreat(transform);
        if(threatTarget != null)
        {
            target = threatTarget.root;
        }
        
        if (target== null)
        {
            FindTarget();
            return;
        }
        else
        {
            HandleCombat();
        }
    }

    void FindTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRange);

        foreach (var hit in hits)
        {
            Transform root = hit.transform.root;
            Health hp = root.GetComponent<Health>();

            if (hp == null || hp.isDead)
                continue;

            if (root == transform.root)
                continue;
            if (!TargetRules.CanBeTargeted(transform, root))
                continue;

            target = root;
            break;
        }
    }

    void HandleCombat()
    {
        if (!TargetRules.CanBeTargeted(transform,target))
        {
            target = null;
            navAgent.ResetPath();
            return;

        }
        navAgent.SetDestination(target.position);
        float distance = Vector3.Distance(transform.position, target.position);
        if(distance <= attackRange)
        {
            if(Time.time >= lastAttackTime + attackCooldown)
            {
                Health hp = DamageUtil.GetHealth(target.gameObject);
                if(hp != null  && !hp.isDead)
                {
                    hp.TakeDamage(damage);
                    ThreatSystem.AddThreat(target, transform, damage);
                }
                lastAttackTime = Time.time;
            }
        }
        if (distance > 15f)
            target = null; 
    }
}
