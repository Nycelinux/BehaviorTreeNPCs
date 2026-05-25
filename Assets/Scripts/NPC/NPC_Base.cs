using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class NPC_Base : MonoBehaviour
{
    protected Node root;
    protected Animator animator;
    protected NavMeshAgent navAgent;


    protected virtual void Start()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    protected virtual void Update()
    {
        root?.Evaluate();
        UpdateAnimation();
    }

    protected abstract void BuildTree();
    protected virtual void UpdateAnimation()
    {
        if(animator != null && navAgent != null)
        {
            float speed = navAgent.velocity.magnitude;
            if (speed < 0.1f)
                speed = 0f;
            animator.SetFloat("Speed", speed, 0.1f, Time.deltaTime);
        }
    }
}
