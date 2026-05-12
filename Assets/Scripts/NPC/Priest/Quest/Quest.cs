using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Quest 
{
    public QuestID questID;
    public bool isActive;
    public bool isCompleted;

    public int requiredAmount;
    public int currentAmount;

    public string questName;
    public string description;

    public QuestType questType;

    public enum QuestType
    {
        Collect,
        Talk,
        Kill,
        Escort,
        Defend,
        Build,
        Survive,
        Deliver,
        Investigate
    }

}
