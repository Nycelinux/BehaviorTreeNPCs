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

    private bool storyAdvanceRequested = false;
    public void Interact()
    {

        Debug.Log("Mit Priester interagiert");
        if (navAgent != null)
        {
            navAgent.isStopped = true;
            navAgent.velocity = Vector3.zero;
        }

        storyAdvanceRequested = false;
        DialogueData currentDialogue = null;
        switch (StoryManager.instance.currentStage)
        {
            case StoryStage.TempleVisits:
                Debug.Log("Intro Dialogue");
                currentDialogue = introDialogue;
                if (QuestManager.instance.HasQuest(QuestID.VisitTemple))
                    QuestManager.instance.ProgressQuest(QuestID.VisitTemple, 1);
                break;
            case StoryStage.FindArtifact:
                currentDialogue = artifactDialogue;
                storyAdvanceRequested = true;
                break;
            case StoryStage.GatherWood:
                currentDialogue = woodDialogue;
                storyAdvanceRequested = true;

                break;
            case StoryStage.FirstAttack:
                currentDialogue = firstAttackDialogue;
                storyAdvanceRequested = true;

                break;
            case StoryStage.GainPriestTrust:
                currentDialogue = trustDialogue;
                storyAdvanceRequested = true;

                break;
            case StoryStage.Diplomacy:
                currentDialogue = diplomacyDialogue;
                storyAdvanceRequested = true;

                break;
            case StoryStage.FinalAttack:
                currentDialogue = lastAttackDialogue;
                storyAdvanceRequested = true;

                break;
           
            case StoryStage.PriestDecision:
                currentDialogue = priestDecisionDialogue;
                storyAdvanceRequested = true;

                break;
            case StoryStage.FinalDecision:
                currentDialogue = éndingDialogue;
                break;
            default:
                Debug.Log("Kein Dialog für stage: " + StoryManager.instance.currentStage);
                break;

        }
        if (currentDialogue != null)
            DialogueManager.instance.StartDialogue(currentDialogue);
    }

    public void ResumeMovement()
    {
        if (navAgent != null)
        {
            navAgent.isStopped = false;
        }
    }

}
