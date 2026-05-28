using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NPC_Priest : NPC_Base, IInteractable
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
    private Blackboard blackboard;

    protected override void Start()
    {
        base.Start();
        if (templeCenter == null)
        {
            Debug.LogError("TempleCenter fehlt");
            enabled = false;
            return;
        }
        blackboard = new Blackboard();
        blackboard.navAgent = navAgent;
        BuildTree();
    }

    protected override void BuildTree()
    {
        root = new Selector(blackboard,new List<Node>
        {
            new PriestRitualNode(blackboard),
            new PriestRoamNode(blackboard,navAgent, templeCenter),
        });
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
                if (!QuestManager.instance.HasQuest(QuestID.GatherWood))
                {
                    StoryManager.instance.StartStage(StoryStage.GatherWood);
                }

                break;
            case StoryStage.GatherWood:
                DialogueManager.instance.StartDialogue(woodDialogue); if (!QuestManager.instance.HasQuest(QuestID.FirstAttack))
                {
                    StoryManager.instance.StartStage(StoryStage.FirstAttack);
                }
                break;
            case StoryStage.FirstAttack:
                DialogueManager.instance.StartDialogue(firstAttackDialogue);
                if (!QuestManager.instance.HasQuest(QuestID.GainPriestTrust))
                {
                    StoryManager.instance.StartStage(StoryStage.GainPriestTrust);
                }
                break;
            case StoryStage.GainPriestTrust:
                DialogueManager.instance.StartDialogue(trustDialogue);
                if (!QuestManager.instance.HasQuest(QuestID.Diplomacy))
                {
                    StoryManager.instance.StartStage(StoryStage.Diplomacy);
                }
                break;
            case StoryStage.Diplomacy:
                DialogueManager.instance.StartDialogue(diplomacyDialogue);
                if (!QuestManager.instance.HasQuest(QuestID.LastAttack))
                {
                    StoryManager.instance.StartStage(StoryStage.FinalAttack);
                }
                break;
            case StoryStage.FinalAttack:
                DialogueManager.instance.StartDialogue(lastAttackDialogue);
                if (!QuestManager.instance.HasQuest(QuestID.PriestDecision))
                {
                    StoryManager.instance.StartStage(StoryStage.PriestDecision);
                }
                break;
           
            case StoryStage.PriestDecision:
                DialogueManager.instance.StartDialogue(priestDecisionDialogue);
                if (!QuestManager.instance.HasQuest(QuestID.FinalDecision))
                {
                    StoryManager.instance.StartStage(StoryStage.FinalDecision);
                }
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
