using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FollowNode : Node
{
    private NavMeshAgent navAgent;
    private Transform player;
    private float followDistance = 1.5f;

    public FollowNode(Blackboard blackboard, NavMeshAgent navAgent, Transform player):base(blackboard)
    {
        this.navAgent = navAgent;
        this.player = player;
    }

    public override NodeState Evaluate()
    {
        if (GameManager.instance == null)
            return NodeState.FAILURE;

        if (blackboard.player == null)
            return NodeState.FAILURE;

        if (blackboard.reputation < 60)
            return NodeState.FAILURE;
        float distance = Vector3.Distance(navAgent.transform.position, blackboard.player.position);
        
        if(distance> followDistance)
        {
            Vector3 direction = (navAgent.transform.position - blackboard.player.position).normalized;
            Vector3 sideOffset = navAgent.transform.right * Random.Range(-2f, 2f);
            Vector3 targetPosition = blackboard.player.position + direction * followDistance + sideOffset;
           
            navAgent.SetDestination(targetPosition);
            Debug.Log("NPC folgt Spieler");
        }
        return NodeState.RUNNING;
    }
}
