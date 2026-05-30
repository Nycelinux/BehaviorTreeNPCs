using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class NPC_Base : MonoBehaviour
{
    protected Node root;
    protected Animator animator;
    protected NavMeshAgent navAgent;
    protected bool initialized;

    protected virtual void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    protected virtual void Start() { }

    protected virtual void Update()
    {
        if (!initialized)
            return;
        if (root == null)
        {
            Debug.Log(name + " ROOT NULL");
            return;
        }

        root.Evaluate();
        UpdateAnimation();
    }
    protected void SetInitialized()
    {
        initialized = true;
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
