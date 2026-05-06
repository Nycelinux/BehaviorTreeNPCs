using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
 

public class HostileNode : Node
{
    private NavMeshAgent navAgent;
    private Transform player;

    public HostileNode(NavMeshAgent navAgent, Transform player)
    {
        this.navAgent = navAgent;
        this.player = player;
    }

    public override NodeState Evaluate()
    {
        float reputation = GameManager.instance.reputation;

        if (reputation > -10)
            return NodeState.FAILURE;

        float distance = Vector3.Distance(navAgent.transform.position, player.position);
        if (distance <10f)
        {
            navAgent.SetDestination(player.position);
            Debug.Log("NPC greift Spieler an!");
            return NodeState.RUNNING;
        }
        return NodeState.FAILURE;
    }
}
