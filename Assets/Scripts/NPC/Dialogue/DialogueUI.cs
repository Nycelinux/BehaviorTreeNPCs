using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DialogueUI : MonoBehaviour
{
    public static DialogueUI instance;
    [Header("UI")]
    public Image portraitImage;
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public TMP_Text npcNameText;

    [Header("Choices")]
    public Transform choicesParent;
    public GameObject choiceButtonPrefab;

    [Header("Hint UI")]
    public TMP_Text hintText;
    public GameObject hintPanel;
    private void Awake()
    {
        instance = this;
        dialoguePanel.SetActive(false);
        hintPanel.SetActive(false);
    }

    public void ShowDialogue(DialogueData dialogue)
    {
        
        Debug.Log("ShowDialogue gestartet");

        if (dialoguePanel == null)
        {
            Debug.LogError("DialoguePanel fehlt");
            return;
        }
        hintPanel.SetActive(false);
        dialoguePanel.SetActive(true);
        npcNameText.text = dialogue.npcName;
        dialogueText.text = dialogue.dialogueText;
        portraitImage.sprite = dialogue.portrait;
        ClearChoices();
        foreach (DialogueChoice choice in dialogue.choices)
            CreateChoiceButton(choice);
    }

    public void ShowHint(string message)
    {
        dialoguePanel.SetActive(false);
        StopAllCoroutines();
        StartCoroutine(ShowHintRoutine(message));
    }

    IEnumerator ShowHintRoutine(string message)
    {
        hintPanel.SetActive(true);
        hintText.text = message;
        yield return new WaitForSeconds(4f);
        hintPanel.SetActive(false);
    }

    void CreateChoiceButton(DialogueChoice choice)
    {
        GameObject buttonObject = Instantiate(choiceButtonPrefab, choicesParent);
        DialogueButtonUI buttonUI = buttonObject.GetComponent<DialogueButtonUI>();
        buttonUI.Setup(choice);
    }

    void ClearChoices()
    {
        foreach (Transform child in choicesParent)
            Destroy(child.gameObject);
    }

    public void CloseDialogue()
    {
        dialoguePanel.SetActive(false);
    }

    public void HideDialogue()
    {
        Debug.Log("Dialogue geschlossen");

        dialoguePanel.SetActive(false);

        ClearChoices();
    }
}
