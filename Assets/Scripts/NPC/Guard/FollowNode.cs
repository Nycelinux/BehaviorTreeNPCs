using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowNode : Node
{
    private NavMeshAgent navAgent;
    private Transform player;

    public FollowNode(NavMeshAgent navAgent, Transform player)
    {
        this.navAgent = navAgent;
        this.player = player;
    }

    public override NodeState Evaluate()
    {
        float reputation = GameManager.instance.reputation;

        if (reputation < 60)
            return NodeState.FAILURE;
        navAgent.SetDestination(player.position);
        Debug.Log("NPC folgt Spieler");
        return NodeState.RUNNING;
    }
}
