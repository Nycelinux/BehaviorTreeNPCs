using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogSystem : MonoBehaviour
{
    public static DialogSystem instance;
    private void Awake()
    {
        instance = this;
    }
    public void OpenPriestDialogue()
    {
        switch (StoryManager.instance.currentStage)
        {
            case StoryStage.TempleVisits:
                OpenTempleIntroduction();
                break;
            case StoryStage.GatherWood:
                OpenWoodDialogue();
                break;
            case StoryStage.GainPriestTrust:
                OpenPriestTrustDialogue();
                break;
            default:
                    Debug.Log("Die Priesterin schweigt");
                break;
        }
    }
    void OpenTempleIntroduction()
    {
        Debug.Log("Priesterin: Willkommen Bürgermeister");
        Quest searchQuest = new Quest
        {
            questID = QuestID.FindArtifact,
            questName = "Artefakt finden",
            description = "Finde das Artefakt unter dem  Tempel.",
            requiredAmount = 1,
            currentAmount = 0,
            questType = Quest.QuestType.Talk
        };
        if (!QuestManager.instance.HasQuest(QuestID.FindArtifact))
        {
            QuestManager.instance.AddQuest(searchQuest);
        }
        StoryManager.instance.StartStage(StoryStage.TempleVisits);
    }
    void OpenWoodDialogue()
    {
        GameManager.instance.reputation += 5;
        UIManager.instance.resources -= 5;
        Debug.Log("Zolle der Priesterin Respekt");
        Quest woodQuest = new Quest
        {
            questID = QuestID.GatherWood,
            questName = "Opfer bringen",
            description = "Sammle Ressourcen und bringe es zum Tempel.",
            requiredAmount = 10,
            currentAmount = 0,
            questType = Quest.QuestType.Collect
        };
        if (!QuestManager.instance.HasQuest(QuestID.GatherWood))
        {
            QuestManager.instance.AddQuest(woodQuest);
        }
        StoryManager.instance.StartStage(StoryStage.GatherWood);
    }
   
    void OpenPriestTrustDialogue()
    {
        Debug.Log("Vielleicht kann ich euch vertrauen");
    }
}
