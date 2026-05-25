using UnityEngine;
using UnityEngine.AI;

public class GuardCombatNode : Node
{
    private Transform curTarget;
    private NPC_Guard guard;
    private NavMeshAgent navAgent;
    private float detectionRange = 10f;
    private float attackRange = 5f;
    private float attackCooldown = 1.5f;
    private float lastAttackTime = 0f;


    public GuardCombatNode(Blackboard blackboard ,NavMeshAgent navAgent, NPC_Guard guard):base(blackboard)
    {
        this.navAgent = navAgent;
        this.guard = guard;
    }

    public override NodeState Evaluate()
    {
        Transform threatTarget = ThreatSystem.GetHighestThreat(navAgent.transform);
        if(threatTarget != null)
        {
            curTarget = threatTarget.root;
            return AttackTarget();
        }
        
        if(curTarget != null)
        {
            Health hp = DamageUtil.GetHealth(curTarget.gameObject);
            if(hp == null || hp.isDead)
            {
                curTarget = null;
                navAgent.ResetPath();
                return NodeState.FAILURE;
            }
            return AttackTarget();
        }

        Collider[] hits = Physics.OverlapSphere(navAgent.transform.position, detectionRange);
        foreach(var hit in hits)
        {
            Transform root = hit.transform.root;
            if (root == navAgent.transform.root)
                continue;
            Health hp = root.GetComponent<Health>();

            if (hp == null || hp.isDead) continue;
            
            if (hit.transform == navAgent.transform) continue;
            curTarget = root;

            if(GuardAlertSystem.instance != null)
                GuardAlertSystem.instance.AlertAll(curTarget);
            return NodeState.RUNNING;
            
        }
        navAgent.ResetPath();
        return NodeState.FAILURE;
    }
    private NodeState AttackTarget()
    {
        if (curTarget == null) {
            navAgent.ResetPath();
            return NodeState.FAILURE;
        }

        if (!TargetRules.CanBeTargeted(navAgent.transform, curTarget))
        {
            curTarget = null;
            navAgent.ResetPath();
            return NodeState.FAILURE;
        }

        Health hp = DamageUtil.GetHealth(curTarget.gameObject);
        if(hp == null|| hp.isDead)
        {
            curTarget = null;
            navAgent.ResetPath();
            return NodeState.FAILURE;
        }

        float distance = Vector3.Distance(navAgent.transform.position, curTarget.position);
        navAgent.SetDestination(curTarget.position);
        if(distance <= attackRange)
        {
            if(Time.time >= lastAttackTime +attackCooldown)
            {
                Debug.Log("Guard attacks Enemy");
                Animator anim = navAgent.GetComponent<Animator>();
                if (anim != null)
                    anim.SetTrigger("Attack");

                if (hp != null)
                {
                    hp.TakeDamage(guard.attackDamage);
                    ThreatSystem.AddThreat(curTarget, navAgent.transform, guard.attackDamage);
                }
                    

                lastAttackTime = Time.time;
            }
            
        }


        if (distance > 15f)
        {
            curTarget = null;
            navAgent.ResetPath();
            return NodeState.FAILURE;
        }
        return NodeState.RUNNING;
    }

    public void SetTarget(Transform enemy)
    {
        curTarget = enemy!= null? enemy.root : null;
    }
}