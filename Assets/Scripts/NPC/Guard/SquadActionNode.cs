using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SquadActionNode : Node
{
    private NPC_Guard guard;
    private float attackRange = 2.5f;
    private NavMeshAgent agent;
    public SquadActionNode(Blackboard blackboard, NPC_Guard guard, NavMeshAgent agent) : base(blackboard)
    {
        this.guard = guard;
        this.agent = agent;
    }

    public override NodeState Evaluate()
    {
        if (blackboard.currentTarget == null)
            return NodeState.FAILURE;

        float distance = Vector3.Distance(agent.transform.position, blackboard.currentTarget.position);
        switch (blackboard.role)
        {
            case squadRole.DPS:
                return HandleDPS(distance);
            case squadRole.Support:
                return HandleSupport();
            case squadRole.Tank:
                return HandleTank(distance);
            default:
                return NodeState.FAILURE;
        }

    }

    private NodeState HandleDPS(float distance)
    {
        if (distance <= attackRange)
            Attack();
        return NodeState.RUNNING;
    }
    private NodeState HandleTank(float distance)
    {
        if (distance <= attackRange)
            Attack();
        return NodeState.RUNNING;
    }

    private NodeState HandleSupport()
    {
        //Add Later Heal logic
        return NodeState.RUNNING;
    }

    private void Attack()
    {
        Animator animator = agent.GetComponent<Animator>();
        if (animator != null)
            animator.SetTrigger("Attack");
        Health hp = DamageUtil.GetHealth(blackboard.currentTarget.gameObject);
        if (hp != null && !hp.isDead)
            hp.TakeDamage(guard.attackDamage);
    }
}