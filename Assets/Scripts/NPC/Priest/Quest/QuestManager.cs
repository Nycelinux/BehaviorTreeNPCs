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
        if (HasQuest(quest.questID))
        {
            Debug.LogWarning("Quest existiert bereits: " + quest.questName);
            return;
        }
        activeQuests.Add(quest);
        quest.isActive = true;
        Debug.Log("Neue Quest: " + quest.questName);
        if(QuestMenuUI.instance != null)
            QuestMenuUI.instance.RefreshQuestList();
    }

    public void ProgressQuest(QuestID id, int amount)
    {
        Debug.Log("Progressquest aufgerufen: " + id);
        Quest quest = activeQuests.Find(q => q.questID == id && q.isActive && !q.isCompleted);
        if (quest == null)
        {
            Debug.LogWarning("Quest nicht gefunden: " + id);
            return;
        }

        quest.currentAmount += amount;
        Debug.Log(quest.questName + ": " + quest.currentAmount + "/" + quest.requiredAmount);
        if(quest.currentAmount >= quest.requiredAmount)
        {
            quest.currentAmount = quest.requiredAmount;
            quest.readyToTurnIn = true;
            Debug.Log(quest.questName + " kann bei Priesterin abgeschlossen werden!");
        }
        if (QuestMenuUI.instance != null)
            QuestMenuUI.instance.RefreshQuestList();
    }

    public void CompleteQuest(Quest quest)
    {
        if (quest == null) return;
        if(quest.currentAmount < quest.requiredAmount)
        {
            Debug.LogWarning("Quest ist noch nicht abgeschlossen: "+ quest.questName);
            return;
        }
        quest.isCompleted = true;
        quest.isActive = false;
        GameManager.instance.reputation += 20;
        Debug.Log("Quest abgeschlossen! Reputationerhöht." + quest.questName);
       
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

    public bool IsReadyToTurnIn(QuestID id)
    {
        Quest quest= activeQuests.Find(q => q.questID == id);
        if (quest == null)
            return false;
        return quest.currentAmount >= quest.requiredAmount && !quest.isCompleted;
    }

    public bool IsCompleted(QuestID id)
    {
        Quest quest = activeQuests.Find(q => q.questID == id);
        if (quest == null)
            return false;
        return quest.isCompleted;
    }

    public Quest GetQuest(QuestID id)
    {
        return activeQuests.Find(q => q.questID == id);
    }

}
