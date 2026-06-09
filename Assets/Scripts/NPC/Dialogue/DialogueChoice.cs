using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class DialogueChoice
{
    public DialogueRelationShipEffect relationShipEffect = new DialogueRelationShipEffect();
    public string choiceText;
    //public bool startQuest;
    public QuestID questID;
    //public bool changeStoryChange;
    public StoryStage nextStage;
    public int reputationChange;
}
