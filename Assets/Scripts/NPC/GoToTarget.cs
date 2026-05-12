using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoToTarget : Node
{
    private Transform target;
    private NavMeshAgent agent;

    public GoToTarget(NavMeshAgent agent, Transform target)
    {
        this.agent = agent;
        this.target = target;
    }

    public override NodeState Evaluate()
    { 
        agent.SetDestination(target.position);
        if (Vector3.Distance(agent.transform.position, target.position) < 1.5f)
           return NodeState.SUCCESS;

        return NodeState.RUNNING;

    }
}
