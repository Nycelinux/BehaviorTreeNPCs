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
        SetInitialized();
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
                if (!QuestManager.instance.isQuestCompleted(QuestID.FindArtifact))
                {
                    DialogueUI.instance.ShowHint("Das Artefakt wurde noch nicht gefunden.");
                    return;
                }
                currentDialogue = artifactDialogue;
                //storyAdvanceRequested = true;
                break;
            case StoryStage.GatherWood:
                if (!QuestManager.instance.isQuestCompleted(QuestID.GatherWood))
                {
                    DialogueUI.instance.ShowHint("Wir brauchen noch mehr Holz.Komm zurück wenn die Arbeit erledigt ist.");
                    return;
                }
                currentDialogue = woodDialogue;
                //storyAdvanceRequested = true;

                break;
            case StoryStage.FirstAttack:
                if (!QuestManager.instance.isQuestCompleted(QuestID.FirstAttack))
                {
                    DialogueUI.instance.ShowHint("Die Angreifer sind noch nicht besiegt.");
                    return;
                }
                currentDialogue = firstAttackDialogue;
                //storyAdvanceRequested = true;

                break;
            case StoryStage.GainPriestTrust:
                if (!QuestManager.instance.isQuestCompleted(QuestID.GainPriestTrust))
                {
                    DialogueUI.instance.ShowHint("Das Vertrauen ist och nicht gewonnen");
                    return;
                }
                currentDialogue = trustDialogue;
                //storyAdvanceRequested = true;

                break;
            case StoryStage.Diplomacy:
                if (!QuestManager.instance.isQuestCompleted(QuestID.Diplomacy))
                {
                    DialogueUI.instance.ShowHint("Das Bündnis wurde noch nicht geschlossen.");
                    return;
                }
                currentDialogue = diplomacyDialogue;
                storyAdvanceRequested = true;

                break;
            case StoryStage.FinalAttack:
                if (!QuestManager.instance.isQuestCompleted(QuestID.LastAttack))
                {
                    DialogueUI.instance.ShowHint("Das Dorf kämpft noch ums Überleben.");
                    return;
                }
                currentDialogue = lastAttackDialogue;
                //storyAdvanceRequested = true;

                break;
           
            case StoryStage.PriestDecision:
                if (!QuestManager.instance.isQuestCompleted(QuestID.PriestDecision))
                {
                    DialogueUI.instance.ShowHint("Ich aheb meine Entscheidung noch nicht gefällt.");
                    return;
                }
                currentDialogue = priestDecisionDialogue;
                //storyAdvanceRequested = true;

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
