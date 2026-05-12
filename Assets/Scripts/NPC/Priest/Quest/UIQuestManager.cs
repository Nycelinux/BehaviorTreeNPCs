using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIQuestManager : MonoBehaviour
{
    public static UIQuestManager instance;
    public TMP_Text questText;
    void Awake()
    {
        instance = this;
    }

    public void UpdateQuestUI()
    {
        questText.text = "";

        foreach (Quest quest in QuestManager.instance.activeQuests)
        {
            string status = quest.isCompleted ? "[FERTIG]" : "";
            questText.text +=
                quest.questName + "\n" +
                quest.description + "\n" +
                quest.currentAmount + "/" + quest.requiredAmount + 
                " " + status + "\n\n";
        }
    }
}
