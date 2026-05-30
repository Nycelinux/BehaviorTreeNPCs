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
        Transform bestTarget = null;
        foreach (var hit in hits)
        {
            if (!hit.CompareTag("Enemy"))
                continue;
            Transform root = hit.transform.root;
            if (root == agent.transform.root)
                continue;

            Health hp = DamageUtil.GetHealth(root.gameObject);
            if (hp == null || hp.isDead)
                continue;
            if (!TargetRules.CanBeTargeted(agent.transform, root))
                continue;
            bestTarget = root;
            break;
        }
        if(bestTarget != null)
        {
            blackboard.currentTarget = bestTarget;
            SquadManager.instance?.SetCombat(bestTarget);
            GuardAlertSystem.instance?.AlertAll(bestTarget);
            return NodeState.SUCCESS;
        }
       if(!SquadManager.instance || !SquadManager.instance.IsInCombat)
        {
            blackboard.currentTarget = null;
            Debug.Log(agent.name + " NO TARGET");
        }
        return NodeState.FAILURE;

    }
}
