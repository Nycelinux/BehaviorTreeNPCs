using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SleepNode : Node
{
    private NavMeshAgent navAgent;
    private Transform home;

    public SleepNode( Blackboard blackboard,NavMeshAgent navAgent, Transform home):base(blackboard)
    {
        this.navAgent = navAgent;
        this.home = home;
    }

    public override NodeState Evaluate()
    {
        Debug.Log(navAgent.name + " SleepNode Evaluate");

        if (GameManager.instance == null)
            return NodeState.FAILURE;
        
        if (home == null)
        {
            Debug.LogError(navAgent.name + " hat kein Home!");
            return NodeState.FAILURE;
        }

        if (!GameManager.instance.isNight)
            return NodeState.FAILURE;

        NavMeshPath path = new NavMeshPath();
        bool hasPath = navAgent.CalculatePath(home.position, path);
        if(!hasPath || path.status != NavMeshPathStatus.PathComplete)
        {
            Debug.LogError(navAgent.name + " home nicht erreichbar: " + home.name);
            return NodeState.FAILURE;
        }
        
        navAgent.SetDestination(home.position);
        Debug.Log(navAgent.name +" going to home: " +home.position);
        //if (Vector3.Distance(navAgent.transform.position, home.position) < 1f)
        if (!navAgent.pathPending && navAgent.remainingDistance <= navAgent.stoppingDistance + 0.1f)
        {
            Debug.Log("Dorfbewohner schläft");
            return NodeState.SUCCESS;
        }

        return NodeState.RUNNING;
        
    }
}
