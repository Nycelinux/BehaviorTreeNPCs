using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PriestRoamNode : Node
{
    private Transform center;
    private NavMeshAgent navAgent;
    private float timer = 0f;
    public PriestRoamNode(Blackboard blackboard,NavMeshAgent navAgent, Transform center):base(blackboard)
    {
        this.navAgent = navAgent;
        this.center = center;
    }
    
    
    public override NodeState Evaluate()
    {
        if(center == null|| navAgent == null)
        {
            Debug.LogError("PriestRoamNode: center is null or NavmeshAgent is null");
            return NodeState.FAILURE;
        }
      
        timer += Time.deltaTime;
        if (timer > 5f)
        {
            Vector3 randomPos = center.position + Random.insideUnitSphere * 3f;
            NavMeshHit hit;
            if (NavMesh.SamplePosition(randomPos, out hit, 3f, NavMesh.AllAreas))
                navAgent.SetDestination(hit.position);
            timer = 0f;
        }
        return NodeState.RUNNING;
    }
}
