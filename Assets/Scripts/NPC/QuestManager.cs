using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;
    public bool hasQuest=false;
    public bool questCompleted=false;
    void Awake()
    {
        instance = this;
    }

    public void GiveQuest()
    {
        hasQuest = true;
        Debug.Log("Neue Quest: Finde dasa Artefakt im Tempel");
    }

    public void CompleteQuest()
    {
        questCompleted = true;
        hasQuest = false;
        GameManager.instance.reputation += 20;
        Debug.Log("Quest abgeschlossen! Reputationerhöht.");
    }
}
