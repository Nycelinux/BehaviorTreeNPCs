using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class DetectThreatNode : Node
{
    private NavMeshAgent navAgent;
    public DetectThreatNode(Blackboard blackboard, NavMeshAgent navAgent) : base(blackboard)
    {
        this.navAgent = navAgent;
    }

    public override NodeState Evaluate()
    {
        Transform player = blackboard.player;
        if (player== null)
        {
            return NodeState.FAILURE;
        }
        float detectionRange = blackboard.reputation < 20 ? 20f : 10f;
        Collider[] hits = Physics.OverlapSphere(navAgent.transform.position, detectionRange);
        foreach (var hit in hits)
        {
            Transform root = hit.transform.root;
            if (root == navAgent.transform.root)
                continue;
            Health hp = root.GetComponent<Health>();

            if (hp == null || hp.isDead) continue;

          
            blackboard.currentTarget = root;
            return NodeState.SUCCESS;

        }
        blackboard.currentTarget = null;
        return NodeState.FAILURE;
    }
}
