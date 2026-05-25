using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    public List<Quest> activeQuests = new List<Quest>();

    void Awake()
    {
        instance = this;
    }

    public void AddQuest(Quest quest)
    {
        activeQuests.Add(quest);
        quest.isActive = true;
        Debug.Log("Neue Quest: Finde dasa Artefakt im Tempel" + quest.questName);
        if(QuestMenuUI.instance != null)
            QuestMenuUI.instance.RefreshQuestList();
    }

    public void ProgressQuest(QuestID id, int amount)
    {
        Quest quest = activeQuests.Find(q => q.questID == id);
        if (quest == null || quest.isCompleted)
            return;
        quest.currentAmount += amount;
        Debug.Log(quest.questName + ": " + quest.currentAmount + "/" + quest.requiredAmount);
        if(quest.currentAmount >= quest.requiredAmount)
        {
            CompleteQuest(quest);
        }
        if (QuestMenuUI.instance != null)
            QuestMenuUI.instance.RefreshQuestList();
    }

    public void CompleteQuest(Quest quest)
    {
        quest.isCompleted = true;
        GameManager.instance.reputation += 20;
        Debug.Log("Quest abgeschlossen! Reputationerhöht." + quest.questName);
        if(quest.questID== QuestID.FindArtifact)
        {
            StoryManager.instance.StartStage(StoryStage.GatherWood);
        }
        if (quest.questID == QuestID.GatherWood)
        {
            StoryManager.instance.StartStage(StoryStage.DefendVillage);
        }
        if (quest.questID == QuestID.DefendVillage)
        {
            StoryManager.instance.StartStage(StoryStage.RatAttack);
        }
        if (quest.questID == QuestID.RatAttack)
        {
            StoryManager.instance.StartStage(StoryStage.GainPriestTrust);
        }
        if (quest.questID == QuestID.Diplomacy)
        {
            StoryManager.instance.StartStage(StoryStage.FinalAttack);
        }
        if (quest.questID == QuestID.PriestDecision)
        {
            StoryManager.instance.StartStage(StoryStage.PriestDecision);
        }
        if (QuestMenuUI.instance != null)
            QuestMenuUI.instance.RefreshQuestList();

    }

    void HandleItemCollected(string itemID)
    {
        if (itemID == "Artifact")
            ProgressQuest(QuestID.FindArtifact, 1);
    }

    private void OnEnable()
    {
        EventManager.OnItemCollected += HandleItemCollected;
    }

    private void OnDisable()
    {
        EventManager.OnItemCollected -= HandleItemCollected;
    }

    public bool HasQuest(QuestID id)
    {
        //return activeQuests.Exists(q => q.questID == id); 
        return activeQuests.Exists(q => q.questID == id && !q.isCompleted); 
    }
}
