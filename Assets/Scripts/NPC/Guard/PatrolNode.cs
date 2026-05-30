using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PatrolNode : Node
{
    private int index = 0;
    private Transform[] waypoints;
    private NavMeshAgent navAgent;
    private bool initialized = false;

    public PatrolNode(Blackboard blackboard, NavMeshAgent navAgent, Transform[] waypoints) : base(blackboard)
    {
        this.navAgent = navAgent;
        this.waypoints = waypoints;
    }

    public override NodeState Evaluate()
    {
        if (navAgent == null || !navAgent.isOnNavMesh)
        {
            Debug.Log("navAgent null oder Guard nicht auf NaavMesh");
            return NodeState.FAILURE;
        }
            

        if (waypoints == null || waypoints.Length < 2)
        {
            Debug.LogWarning("PatrolNode: zu wenige Waypoints!");
            return NodeState.FAILURE;
        }

        if (blackboard.currentTarget != null)
            return NodeState.FAILURE;

        if (!initialized)
        {
            
            initialized = true;
            index = 0;
            navAgent.ResetPath();
            navAgent.SetDestination(waypoints[index].position);

            Debug.Log("Patrol START -> " + waypoints[0].name);
        }

        if (!navAgent.pathPending &&
            navAgent.remainingDistance <= navAgent.stoppingDistance + 0.3f)
        {
            index = (index + 1) % waypoints.Length;
            navAgent.SetDestination(waypoints[index].position);

            Debug.Log("Patrol Next -> " + waypoints[index].name);
        }

       
        return NodeState.RUNNING;
    }
}