using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    public DialogueUI dialogueUI;
    private void Awake()
    {
        instance = this;
    }

    public void StartDialogue(DialogueData dialogue)
    {
        Debug.Log("StartDialogue aufgerufen");

        if (dialogueUI == null)
        {
            Debug.LogError("DialogueUI fehlt!");
            return;
        }

        if (dialogue == null)
        {
            Debug.LogError("DialogueData fehlt!");
            return;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        dialogueUI.ShowDialogue(dialogue);
    }

    public void EndDialogue()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        dialogueUI.HideDialogue();
        NPC_Priest priest = FindObjectOfType<NPC_Priest>();

        if (priest != null)
        {
            priest.ResumeMovement();
        }
    }
}
