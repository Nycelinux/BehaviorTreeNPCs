using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DetectEnemyNode : Node
{
    private NavMeshAgent agent;
    public DetectEnemyNode(Blackboard blackboard, NPC_Guard guard, NavMeshAgent agent) : base(blackboard)
    {
        this.agent = agent;
    }

    public override NodeState Evaluate()
    {
        Collider[] hits = Physics.OverlapSphere(agent.transform.position, 10f);
        foreach (var hit in hits)
        {
            if (!hit.CompareTag("Enemy"))
                continue;
            Transform root = hit.transform.root;
            if (root == agent.transform.root)
                continue;

            Health hp = DamageUtil.GetHealth(root.gameObject);
            if (hp == null && hp.isDead)
                continue;
            if (!TargetRules.CanBeTargeted(agent.transform, root))
                continue;
            blackboard.currentTarget = root;
            return NodeState.SUCCESS;
        }

        blackboard.currentTarget = null; 
        return NodeState.FAILURE;
    }
}
