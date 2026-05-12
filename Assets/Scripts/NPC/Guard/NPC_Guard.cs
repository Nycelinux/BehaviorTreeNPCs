using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Guard : MonoBehaviour
{
    public Transform homePoint;
    public Transform[] wayPoints;
    public Transform player;
    public int attackDamage = 2;

    private NavMeshAgent navAgent;
    private Animator animator;
    private Node root;
    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        var combatNode = new GuardCombatNode(navAgent, this);
        if(GuardAlertSystem.instance != null)
            GuardAlertSystem.instance.Register(combatNode);
        root = new Selector(new List<Node>
        {
            new SleepNode(navAgent,homePoint),
            combatNode,
            new HostileNode(navAgent,player),
            new FollowNode(navAgent, player),
            new PatrolNode(navAgent,wayPoints)
        });
    }

    void Update()
    {
        root.Evaluate();
        if (animator != null && navAgent != null)
        {
            float speed = navAgent.velocity.magnitude;
            if (speed < 0.1f) speed = 0f;
            animator.SetFloat("Speed", speed, 0.1f, Time.deltaTime);
        }
    }
}

// Reputation beeinflusst Angst, Npcs helfen sich gegenseitig (heilen, fliehen gemeinsam) als Erweiterung
