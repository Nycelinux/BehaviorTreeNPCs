using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LowHealthRetreatNode : Node
{

    private NavMeshAgent agent;
    public LowHealthRetreatNode(Blackboard blackboard, NavMeshAgent agent) : base(blackboard)
    {
        this.agent = agent;
    }

    public override NodeState Evaluate()
    {
        if(blackboard.health>25)
            return NodeState.FAILURE;

        Vector3 retreatDirection = -agent.transform.forward * 10f;
        Vector3 targetPosition = agent.transform.position + retreatDirection;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(targetPosition, out hit, 10f, NavMesh.AllAreas))
            agent.SetDestination(hit.position);

        return NodeState.RUNNING;
    }
}
