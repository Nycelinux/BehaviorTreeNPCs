using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueButtonUI : MonoBehaviour
{
    private DialogueChoice currentChoice;
    public TMP_Text buttonnText;
   
    public void Setup(DialogueChoice choice)
    {
        currentChoice = choice;
        buttonnText.text = choice.choiceText;
        GetComponent<Button>()
            .onClick
            .AddListener(Choose);
    }

    void Choose()
    {
        Debug.Log("Choice gewählt: " + currentChoice.choiceText);

        GameManager.instance.reputation += currentChoice.reputationChange;
        if (StoryManager.instance.currentStage == StoryStage.TempleVisits)
            QuestManager.instance.ProgressQuest(QuestID.VisitTemple, 1);
        if (currentChoice.startQuest)
            CreateQuest(currentChoice.questID);
        
        StoryManager.instance.StartStage(currentChoice.nextStage);
        DialogueManager.instance.EndDialogue();

        
    }
    void CreateQuest(QuestID id)
    {
        switch (id)
        {
            case QuestID.FindArtifact:
                Quest artifactQuest = new Quest
                {
                    questID = QuestID.FindArtifact,
                    questName = "Finde das Artifakt",
                    description = " Finde das Artifakt unter dem Tempel",
                    requiredAmount = 1,
                    currentAmount = 0,
                    questType = Quest.QuestType.Collect
                };
                QuestManager.instance.AddQuest(artifactQuest);
                break;
            case QuestID.GatherWood:
                Quest woodQuest = new Quest
                {
                    questID = QuestID.GatherWood,
                    questName = "Sammle Holz",
                    description = " Sammle Holz und bringe es der Priesterin.",
                    requiredAmount = 10,
                    currentAmount = 0,
                    questType = Quest.QuestType.Collect
                };
                QuestManager.instance.AddQuest(woodQuest);
                break;
        }
    }
}
