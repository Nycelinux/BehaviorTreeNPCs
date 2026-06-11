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
        Debug.Log("dialogue = " + dialogue);
        Debug.Log("npc = " + npc);
        Debug.Log("dialogueUI = " + dialogueUI);
        Debug.Log("DialogueChoiceExecuter.instance = " + DialogueChoiceExecuter.instance);
        currentNpc = npc;

        if (dialogue == null)
        {
            Debug.LogError("Dialogue ist NULL");
            return;
        }

        if (dialogueUI == null)
        {
            Debug.LogError("DialogueUI fehlt im Inspector");
            return;
        }

        currentNpc = npc;
        Debug.Log("Current NPC gesetzt: " + currentNpc.name);
        if (DialogueChoiceExecuter.instance != null)
        {
            DialogueChoiceExecuter.instance.SetContext(npc);
            Debug.Log(" DialogueChoiceExecuter gesetzt");
        }
        else
        {
            Debug.LogError("DialogueChoiceExecuter instance is NULL");
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log("ShowDialogue wird aufgerufen");
        dialogueUI.ShowDialogue(dialogue);
    }

    public void StartConvaiDialogue (NPC_Villager npc, string context)
    {
        Debug.Log("StartConvaiDialogue aufgerufen");
        currentNpc = npc.gameObject;
        npc.StartDialogueMode();
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (ConvaiDialogueService.instance == null)
        {
            Debug.LogError("ConvaiDialogueService.instance ist NULL");
            return;
        }
        ConvaiDialogueService.instance.StartConversation(npc, context);
    }


    public void EndDialogue()
    {
        
        if(currentNpc != null)
        {
            NPC_Villager villager = currentNpc.GetComponent<NPC_Villager>();
            if (villager != null) { 
                villager.EndDialogueMode();
                villager.ResetConvaiLock();
            }
        }
        dialogueUI.HideDialogue();
        currentNpc = null;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        Time.timeScale = 1f;

        /*NPC_Priest priest = FindObjectOfType<NPC_Priest>();

        if (priest != null)
        {
            priest.ResumeMovement();
        }*/
    }
}
