using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SquadPositioningNode : Node
{
    private NavMeshAgent agent;
    public SquadPositioningNode(Blackboard blackboard, NavMeshAgent agent) : base(blackboard)
    {
        this.agent = agent;
    }

    public override NodeState Evaluate()
    {
        if (blackboard.currentTarget == null)
            return NodeState.FAILURE;

        Vector3 targetPos = blackboard.currentTarget.position;
        Vector3 movePos = targetPos;
        Vector3 forward = (agent.transform.position-targetPos).normalized;
        Vector3 right = Vector3.Cross(Vector3.up,forward);

        switch (blackboard.role)
        {
            case squadRole.Tank:
                movePos = targetPos + forward * 2f;
                break;
            case squadRole.DPS:
                float side = Random.Range(-1f, 1f);
                Vector3 flankOffset = right * side * 4f;
                movePos = targetPos + flankOffset+ forward * 1.5f;
                break;
            case squadRole.Support:
                movePos = targetPos - forward * 4f +right*Random.Range(-1f, 1f);
                break;
        }
        blackboard.movePosition = movePos;
        agent.SetDestination(movePos);
        return NodeState.FAILURE;
    }
}
