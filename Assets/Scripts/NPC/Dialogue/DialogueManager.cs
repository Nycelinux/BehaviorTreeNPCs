using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Convai.Scripts.Runtime.Core;
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;
    public DialogueUI dialogueUI;
    public GameObject currentNpc;
    private void Awake()
    {
        instance = this;
    }

    public void StartDialogue(DialogueData dialogue, GameObject npc)
    {
        Debug.Log("StartDialogue aufgerufen");
        currentNpc = npc;

        /*if (dialogueUI == null)
        {
            Debug.LogError("DialogueUI fehlt!");
            return;
        }

        if (dialogue == null)
        {
            Debug.LogError("DialogueData fehlt!");
            return;
        }*/
        DialogueChoiceExecuter.instance.SetContext(npc);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        dialogueUI.ShowDialogue(dialogue);
    }

    public void StartConvaiDialogue (NPC_Villager npc, string context)
    {
        Debug.Log("StartConvaiDialogue aufgerufen");
        currentNpc = npc.gameObject;
       
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        ConvaiDialogueService.instance.StartConversation(npc, context);
    }


    public void EndDialogue()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        dialogueUI.HideDialogue();
        /*NPC_Priest priest = FindObjectOfType<NPC_Priest>();

        if (priest != null)
        {
            priest.ResumeMovement();
        }*/
    }
}
