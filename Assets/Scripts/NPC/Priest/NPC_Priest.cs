using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Priest : MonoBehaviour, IInteractable
{
    public Transform templeCenter;
    public DialogueData introDialogue;
    public DialogueData woodDialogue;
    public DialogueData firstAttackDialogue;
    public DialogueData lastAttackDialogue;
    public DialogueData trustDialogue;
    public DialogueData artifactDialogue;
    public DialogueData diplomacyDialogue;
    public DialogueData priestDecisionDialogue;
    public DialogueData éndingDialogue;

    private NavMeshAgent navAgent;
    private Animator animator;
    private Node root;

    void Start()
    {
        if(templeCenter == null)
        {
            Debug.LogError("TempleCenter fehlt");
            enabled = false;
            return;
        }
        navAgent = GetComponent<NavMeshAgent>();
        if(navAgent == null)
        {
            Debug.LogError(" NPC_Priest braucht NavMeshagent");
            enabled = false;
            return;
        }
        animator = GetComponent<Animator>();

        root = new Selector(new List<Node>
        {
            new PriestRitualNode(),
            new PriestRoamNode(navAgent, templeCenter),
        });
    }

    void Update()
    {
        if(root != null)
            root.Evaluate();
        if (animator != null && navAgent != null)
        {
            float speed = navAgent.velocity.magnitude;
            if (speed < 0.1f) speed = 0f;
            animator.SetFloat("Speed", speed, 0.01f, Time.deltaTime);
        }
    }

    public void Interact()
    {
        Debug.Log("Mit Priester interagiert");
        if (navAgent != null)
        {
            navAgent.isStopped = true;
            navAgent.velocity = Vector3.zero;
        }

        switch (StoryManager.instance.currentStage)
        {
            case StoryStage.TempleVisits:
                Debug.Log("Intro Dialogue");
                if (introDialogue == null)
                {
                    Debug.LogError("introDialogue fehlt");
                    return;
                }
                DialogueManager.instance.StartDialogue(introDialogue);
                break;
            case StoryStage.FindArtifact:
                DialogueManager.instance.StartDialogue(artifactDialogue);
                break;
            case StoryStage.GatherWood:
                DialogueManager.instance.StartDialogue(woodDialogue);
                break;
            case StoryStage.FirstAttack:
                DialogueManager.instance.StartDialogue(firstAttackDialogue);
                break;
            case StoryStage.GainPriestTrust:
                DialogueManager.instance.StartDialogue(trustDialogue);
                break;
            case StoryStage.Diplomacy:
                DialogueManager.instance.StartDialogue(diplomacyDialogue);
                break;
            case StoryStage.FinalAttack:
                DialogueManager.instance.StartDialogue(lastAttackDialogue);
                break;
           
            case StoryStage.PriestDecision:
                DialogueManager.instance.StartDialogue(priestDecisionDialogue);
                break;
            case StoryStage.FinalDecision:
                DialogueManager.instance.StartDialogue(éndingDialogue);
                break;
            default:
                Debug.Log("Kein Dialog für stage: " + StoryManager.instance.currentStage);
                break;

        }
    }

    public void ResumeMovement()
    {
        if (navAgent != null)
        {
            navAgent.isStopped = false;
        }
    }

}
