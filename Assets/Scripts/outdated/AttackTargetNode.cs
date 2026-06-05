using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AttackTargetNode : Node
{
    private NavMeshAgent navAgent;
    private NPC_Guard guard;
    private float attackRange =5f;
    private float attackCooldown=1.5f;
    private float lastAttackTime;

    public AttackTargetNode(Blackboard blackboard, NavMeshAgent navAgent, NPC_Guard guard) : base(blackboard)
    {
        this.navAgent = navAgent;
        this.guard = guard;
    }

    public override NodeState Evaluate()
    {
        if (blackboard.currentTarget == null)
        {
            return NodeState.FAILURE;
        }

        Health hp = DamageUtil.GetHealth(blackboard.currentTarget.gameObject);
        if (hp == null || hp.isDead)
        {
            blackboard.currentTarget = null;
            return NodeState.FAILURE;
        }

        float distance = Vector3.Distance(navAgent.transform.position, blackboard.currentTarget.position);
        if (distance > attackRange)
            return NodeState.FAILURE;
        if (Time.time >= lastAttackTime + attackCooldown)
        {
            Debug.Log("Guard attacks Enemy");
            Animator anim = navAgent.GetComponent<Animator>();
            if (anim != null)
                anim.SetTrigger("Attack");

            if (hp != null)
            {
                hp.TakeDamage(guard.attackDamage);
                ThreatSystem.AddThreat(blackboard.currentTarget, navAgent.transform, guard.attackDamage);
            }
            lastAttackTime = Time.time;
        }
        return NodeState.RUNNING;
        }

}
