using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestDetailsUI : MonoBehaviour
{
    public static QuestDetailsUI instance;
    public TMP_Text titleText;
    public TMP_Text descriptionText;
    public TMP_Text progressText;
    private void Awake()
    {
        instance = this;
    }

    public void ShowQuest(Quest quest)
    {
        titleText.text = quest.questName;
        descriptionText.text = quest.description;
        progressText.text = quest.currentAmount + "/" + quest.requiredAmount;
    }
}
