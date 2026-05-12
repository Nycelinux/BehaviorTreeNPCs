using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class DialogueChoice
{
    public string choiceText;
    public bool startQuest;
    public QuestID questID;
    public StoryStage nextStage;
    public int reputationChange;
}
