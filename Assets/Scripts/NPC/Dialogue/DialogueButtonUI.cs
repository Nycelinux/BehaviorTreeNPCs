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
        DialogueManager.instance.EndDialogue();    
    }
   
}
