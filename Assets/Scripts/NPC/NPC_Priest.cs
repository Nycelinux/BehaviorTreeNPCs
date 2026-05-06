using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Priest : MonoBehaviour
{
    public Transform templeCenter;

    private NavMeshAgent navAgent;
    private Animator animator;
    private Node root;

    void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        root = new Selector(new List<Node>
        {
            new PriestRitualNode(),
            new PriestDialogueNode(),
            new PriestRoamNode(navAgent, templeCenter),
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
