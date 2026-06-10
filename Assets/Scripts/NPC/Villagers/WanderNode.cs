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
    private float waitTime;
    private Transform player;
    public bool isPaused;
    public WanderNode(Blackboard blackboard, NavMeshAgent navAgent, Vector3 center, float radius, Transform player) : base(blackboard)
    {
        this.navAgent = navAgent;
        this.center = center;
        this.radius = radius;
        this.player = player;
        waitTime = Random.Range(3f, 8f);
    }

    public override NodeState Evaluate()
    {
        // Wenn pausiert -> nur zum Spieler drehen
        if (isPaused && player != null)
        {
            Vector3 dir = (player.position - navAgent.transform.position);
            dir.y = 0f;

            if (dir.sqrMagnitude > 0.01f)
            {
                Quaternion rot = Quaternion.LookRotation(dir);
                navAgent.transform.rotation = Quaternion.Slerp(
                    navAgent.transform.rotation,
                    rot,
                    Time.deltaTime * 5f
                );
            }

            navAgent.isStopped = true;
            return NodeState.RUNNING;
        }

        timer += Time.deltaTime;

        if (timer < waitTime)
            return NodeState.RUNNING;

        timer = 0f;
        waitTime = Random.Range(3f, 8f);

        Vector3 randomPosition = center + Random.insideUnitSphere * radius;

        if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, radius, NavMesh.AllAreas))
        {
            navAgent.isStopped = false;
            navAgent.SetDestination(hit.position);
        }

        return NodeState.RUNNING;
    }

    // Wird von außen aufgerufen
    public void SetPaused(bool value)
    {
        isPaused = value;

        if (value)
        {
            navAgent.isStopped = true;
        }
    }
}
