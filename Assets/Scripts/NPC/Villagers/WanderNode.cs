using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WanderNode : Node
{
    private NavMeshAgent navAgent;
    private Vector3 center;
    private float radius;
    private float timer;
    public WanderNode(Blackboard blackboard, NavMeshAgent navAgent, Vector3 center, float radius) : base(blackboard)
    {
        this.navAgent = navAgent;
        this.center = center;
        this.radius = radius;
    }

    public override NodeState Evaluate()
    {
        timer += Time.deltaTime;
        if (timer < 4f)
            return NodeState.RUNNING;
        timer = 0f;
        Vector3 randomPosition = center + Random.insideUnitSphere * radius;

        NavMeshHit hit;
        if(NavMesh.SamplePosition(randomPosition,out hit, radius, NavMesh.AllAreas))
        {
            navAgent.SetDestination(hit.position);
        }
        return NodeState.SUCCESS;
    }
}
