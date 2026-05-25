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
        if (GameManager.instance == null)
            return NodeState.FAILURE;
        if (!blackboard.isNight)
            return NodeState.FAILURE;
        if (home == null)
            return NodeState.FAILURE;

        if (!GameManager.instance.isNight)
            return NodeState.FAILURE;

        navAgent.SetDestination(home.position);
        if (Vector3.Distance(navAgent.transform.position, home.position) < 1f)
        {
            Debug.Log("Dorfbewohner schläft");
            return NodeState.SUCCESS;
        }

        return NodeState.RUNNING;
        
    }
}
