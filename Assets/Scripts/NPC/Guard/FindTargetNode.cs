using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FindTargetNode : Node
{
    private NavMeshAgent navAgent;
    private float detectionRange;

    public FindTargetNode(Blackboard blackboard, NavMeshAgent navAgent, float detectionRange = 10f) : base(blackboard)
    {
        this.navAgent = navAgent;
        this.detectionRange = detectionRange;
    }

    public override NodeState Evaluate()
    {
        Transform threatTarget = ThreatSystem.GetHighestThreat(navAgent.transform);
        if (threatTarget != null)
        {
            blackboard.currentTarget = threatTarget.root;
            if (GuardAlertSystem.instance != null)
                GuardAlertSystem.instance.AlertAll(threatTarget.root);

            return NodeState.SUCCESS;
        }

        Collider[] hits = Physics.OverlapSphere(navAgent.transform.position, detectionRange);
        foreach (var hit in hits)
        {
            Transform root = hit.transform.root;
            if (root == navAgent.transform.root)
                continue;
            Health hp = root.GetComponent<Health>();

            if (hp == null || hp.isDead) continue;

            if (!TargetRules.CanBeTargeted(navAgent.transform, root))
                continue;
            blackboard.currentTarget = root;
            if (GuardAlertSystem.instance != null)
                GuardAlertSystem.instance.AlertAll(root);
            return NodeState.SUCCESS;

        }
        blackboard.currentTarget = null;
        return NodeState.FAILURE;
    }
}