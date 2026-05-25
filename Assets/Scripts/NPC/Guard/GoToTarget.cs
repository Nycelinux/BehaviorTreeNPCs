using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GoToTarget : Node
{
    private NavMeshAgent agent;
    private float stopDistance;

    public GoToTarget(Blackboard blackboard,NavMeshAgent agent, float stopDistance = 5f): base(blackboard)
    {
        this.agent = agent;
        this.stopDistance = stopDistance;
    }

    public override NodeState Evaluate()
    {
        if (blackboard.currentTarget == null)
            return NodeState.FAILURE;
        float distance = Vector3.Distance(agent.transform.position, blackboard.currentTarget.position);


        if ( distance< stopDistance)
        {
            agent.ResetPath();
            return NodeState.SUCCESS;
        }

        agent.SetDestination(blackboard.currentTarget.position);
        return NodeState.RUNNING;

    }
}
