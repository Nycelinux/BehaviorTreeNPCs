using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestEntryUI : MonoBehaviour
{
    private Quest quest;
    public Image icon;
    public TMP_Text progressText;
    public TMP_Text questNameText;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(SelectQuest);
    }
    public void Setup(Quest newQuest)
    {
        quest = newQuest;
        questNameText.text = quest.questName;
        progressText.text = quest.currentAmount + "/" + quest.requiredAmount;
    }

    // Update is called once per frame
    public void SelectQuest()
    {
        QuestDetailsUI.instance.ShowQuest(quest);
    }
}
