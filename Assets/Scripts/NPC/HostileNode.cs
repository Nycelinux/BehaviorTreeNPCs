using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
 

public class HostileNode : Node
{
    private NavMeshAgent navAgent;
    private Transform player;

    public HostileNode(Blackboard blackboard ,NavMeshAgent navAgent, Transform player):base(blackboard)
    {
        this.navAgent = navAgent;
        this.player = player;
    }

    public override NodeState Evaluate()
    {
        float reputation = GameManager.instance.reputation;

        if (blackboard.reputation > 5f)
        {
            blackboard.isHostile = false;
            return NodeState.FAILURE;
        }


        blackboard.isHostile = true;
        navAgent.SetDestination(player.position);
        return NodeState.SUCCESS;
    }
}
