using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolNode : Node
{
    private int index = 0;
    private Transform[] waypoints;
    private NavMeshAgent navAgent;

    public PatrolNode(Blackboard blackboard ,NavMeshAgent navAgent, Transform[] waypoints):base(blackboard)
    {
        this.navAgent = navAgent;
        this.waypoints = waypoints;
    }

    public override NodeState Evaluate()
    {
        if (waypoints.Length == 0)
            return NodeState.FAILURE;
        navAgent.SetDestination(waypoints[index].position);
        if(Vector3.Distance(navAgent.transform.position,waypoints[index].position) < 1f)
        {
            index = (index + 1) % waypoints.Length;
        }
        return NodeState.RUNNING;
    }
}
