using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SquadActionNode : Node
{
    private NPC_Guard guard;
    private float attackRange = 2.5f;
    private NavMeshAgent agent; 
    private float lastAttackTime;
    private float attackCooldown = 1f;
    public SquadActionNode(Blackboard blackboard, NPC_Guard guard, NavMeshAgent agent) : base(blackboard)
    {
        this.guard = guard;
        this.agent = agent;
    }

    public override NodeState Evaluate()
    {
        
        if (blackboard.currentTarget == null)
            return NodeState.FAILURE;
        Debug.Log(agent.name + "SquadActionNode");
        float distance = Vector3.Distance(agent.transform.position, blackboard.currentTarget.position);
        switch (blackboard.role)
        {
            case squadRole.DPS:
                return HandleAttack(distance);
                break;
            case squadRole.Support:
                return HandleAttack(distance);
            case squadRole.Tank:
                return NodeState.RUNNING;
            default:
                return NodeState.FAILURE;
        }

    }

    private NodeState HandleAttack(float distance)
    {
        if (distance <= attackRange)
        {
            TryAttack();
        }
           
        return NodeState.RUNNING;
    }
   
    private void TryAttack()
    {
        if (Time.time < lastAttackTime + attackCooldown)
            return;
        lastAttackTime = Time.time;
        Attack();
    }
    private void Attack()
    {
        Debug.Log(
          $"{agent.name} greift an. Damage = {guard.attackDamage}"
      );
        Animator animator = agent.GetComponent<Animator>();
        if (animator != null)
            animator.SetTrigger("Attack");
        Health hp = DamageUtil.GetHealth(blackboard.currentTarget.gameObject);
        if (hp != null && !hp.isDead)
            hp.TakeDamage(guard.attackDamage);
        else
            Debug.LogWarning("Target hat kein Healt oder ist tot");
    }
}