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
    public void Interact(Vector3 hitPoint)
    {

        Debug.Log("Mit Priester interagiert");
        if (navAgent != null)
        {
            navAgent.isStopped = true;
            navAgent.velocity = Vector3.zero;
        }

        //storyAdvanceRequested = false;
        DialogueData currentDialogue = null;
        switch (StoryManager.instance.currentStage)
        {
            case StoryStage.TempleVisits:
                var quest = QuestManager.instance.GetQuest(QuestID.VisitTemple);

                if (quest == null)
                {
                    Debug.Log("Quest fehlt -> wird erstellt");
                    QuestManager.instance.AddQuest(new Quest
                    {
                        questID = QuestID.VisitTemple,
                        questName = "Visit Temple",
                        requiredAmount = 1,
                        questType = Quest.QuestType.Investigate
                    });
                }

                    if (!QuestManager.instance.IsCompleted(QuestID.VisitTemple))
                {
                    Debug.Log("Intro Dialogue");
                    Debug.Log("Quest not commpleted ... ");

                    QuestManager.instance.ProgressQuest(QuestID.VisitTemple, 1);
                    if(quest != null && quest.readyToTurnIn)
                    {
                        Debug.Log("Quest completed...");

                        QuestManager.instance.CompleteQuest(quest);
                        StoryManager.instance.StartStage(StoryStage.FindArtifact);
                        currentDialogue = introDialogue;
                        ;
                    }
                    else
                    {
                        Debug.Log("quest noch nicht abgeschlossen");

                    }
                }
                break;
            case StoryStage.FindArtifact:
                if (!QuestManager.instance.IsReadyToTurnIn(QuestID.FindArtifact))
                {
                    DialogueUI.instance.ShowHint("Das Artifakt wartet im Tempel... es kommt nicht von allein zu euch");
                    ResumeMovement();
                    return;
                }
                    QuestManager.instance.CompleteQuest(QuestManager.instance.GetQuest(QuestID.FindArtifact));
                    StoryManager.instance.StartStage(StoryStage.GatherWood);
                    currentDialogue = artifactDialogue;

                //storyAdvanceRequested = true;
                break;
            case StoryStage.GatherWood:
                if (QuestManager.instance.IsReadyToTurnIn(QuestID.GatherWood))
                {
                    QuestManager.instance.CompleteQuest(QuestManager.instance.GetQuest(QuestID.GatherWood));
                    StoryManager.instance.StartStage(StoryStage.FirstAttack);
                    currentDialogue = woodDialogue;
                }
                else
                {
                    DialogueUI.instance.ShowHint("Sammle mehr Holz");
                    ResumeMovement();
                    return;
                }                                    
                //storyAdvanceRequested = true;
                break;
            case StoryStage.FirstAttack:
                if (QuestManager.instance.IsReadyToTurnIn(QuestID.FirstAttack))
                {
                    QuestManager.instance.CompleteQuest(QuestManager.instance.GetQuest(QuestID.FirstAttack));
                    StoryManager.instance.StartStage(StoryStage.FirstAttack);
                    currentDialogue = firstAttackDialogue;
                }
                else
                {
                    DialogueUI.instance.ShowHint("Die Gegner sind noch nicht besiegt");
                    ResumeMovement();
                    return;
                }   
                //storyAdvanceRequested = true;
                break;
            case StoryStage.GainPriestTrust:
                if (QuestManager.instance.IsReadyToTurnIn(QuestID.GainPriestTrust))
                {
                    QuestManager.instance.CompleteQuest(QuestManager.instance.GetQuest(QuestID.GainPriestTrust));
                    StoryManager.instance.StartStage(StoryStage.Diplomacy);
                    currentDialogue = trustDialogue;

                }
                else
                {
                    DialogueUI.instance.ShowHint("Das Vertrauen ist noch nicht gewonnen");
                    ResumeMovement();
                    return;
                }                   
                //storyAdvanceRequested = true;
                break;
            case StoryStage.Diplomacy:
                if (QuestManager.instance.IsReadyToTurnIn(QuestID.Diplomacy))
                {
                    QuestManager.instance.CompleteQuest(QuestManager.instance.GetQuest(QuestID.Diplomacy));
                    StoryManager.instance.StartStage(StoryStage.FinalAttack);
                    currentDialogue = diplomacyDialogue;
                }
                else
                {
                    DialogueUI.instance.ShowHint("Diplomatie nicht beendet");
                    ResumeMovement(); 
                    return;
                }
                //storyAdvanceRequested = true;
                break;
            case StoryStage.FinalAttack:
                if (QuestManager.instance.IsReadyToTurnIn(QuestID.LastAttack))
                {
                    QuestManager.instance.CompleteQuest(QuestManager.instance.GetQuest(QuestID.LastAttack));
                    StoryManager.instance.StartStage(StoryStage.PriestDecision);
                    currentDialogue = lastAttackDialogue;

                }
                else
                {
                    DialogueUI.instance.ShowHint("Die Gegner sind noch nicht besiegt");
                    ResumeMovement();
                    return;
                }                    
                //storyAdvanceRequested = true;
                break;
           
            case StoryStage.PriestDecision:
                if (QuestManager.instance.IsReadyToTurnIn(QuestID.PriestDecision))
                {
                    QuestManager.instance.CompleteQuest(QuestManager.instance.GetQuest(QuestID.PriestDecision));
                    StoryManager.instance.StartStage(StoryStage.FinalDecision);
                    currentDialogue = priestDecisionDialogue;
                }
                else
                {
                    DialogueUI.instance.ShowHint("Die Priesterin hat ihre finale Entscheidung noch nicht getroffen");
                    ResumeMovement();
                    return;
                }
                //storyAdvanceRequested = true;
                break;
            case StoryStage.FinalDecision:
                if (QuestManager.instance.IsReadyToTurnIn(QuestID.FinalDecision))
                {
                    QuestManager.instance.CompleteQuest(QuestManager.instance.GetQuest(QuestID.FinalDecision));
                    currentDialogue = éndingDialogue;

                }
                else
                {
                    DialogueUI.instance.ShowHint("ende noch nicht abgeschlossen.");
                    ResumeMovement();
                    return;
                }                   
                break;
            default:
                Debug.Log("Kein Dialog für stage: " + StoryManager.instance.currentStage);
                break;

        }
        if (currentDialogue == null)
        {
            Debug.LogError("Keine Dialogdata für stage: " + StoryManager.instance.currentStage);
            Debug.LogError($"NPC: {gameObject.name}");
            ResumeMovement();
            return;
        }
        DialogueManager.instance.StartDialogue(currentDialogue, gameObject);
    }

    public void ResumeMovement()
    {
        if (navAgent != null)
        {
            navAgent.isStopped = false;
        }
    }

}
