using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvaiDialogueService : MonoBehaviour
{
    public static ConvaiDialogueService instance;
    private bool isBusy = false;
    private NPC_Villager activeVillager;
    private void Awake()
    {
        instance = this;
    }

    public static class ConvaiAcessManager
    {
        public static bool IsConvaiAllowed(NPC_Villager villager)
        {
            return villager != null && villager.hasConvai;
        }
    }

    public void StartConversation(NPC_Villager villager, string context)
    {
        Debug.Log("=== StartConversation ===");

        Debug.Log("villager = " + villager);
        Debug.Log("context = " + context);
       

        Debug.Log("ConvaiResponseRouter.Instance = " + ConvaiResponseRouter.Instance);
        if (villager == null || !villager.hasConvai)
        {
            Debug.LogWarning("Blocked Convai call");
            return;
        }
        if (isBusy)
        {
            Debug.LogWarning("Convai ist bereits aktiv – Request ignoriert");
            return;
        }
        isBusy = true;
        activeVillager = villager;

        if (ConvaiResponseRouter.Instance == null)
        {
            Debug.LogError("ConvaiResponseRouter fehlt in der Szene!");
            isBusy = false;
            return;
        }
        ConvaiResponseRouter.Instance.OnTextResponse -= HandleResponse;
        ConvaiResponseRouter.Instance.OnTextResponse += HandleResponse;
        ConvaiResponseRouter.Instance.Send(villager, context);
    }

    private void HandleResponse(string text, NPC_Villager villager)
    {
        ConvaiResponseRouter.Instance.OnTextResponse -= HandleResponse;
        isBusy = false;
        activeVillager = null;
        NPC_Villager.convaiGlobalLock = false;
        if (DialogueManager.instance == null) return;

        DialogueData data = ScriptableObject.CreateInstance<DialogueData>();
        data.npcName = villager.npcName;
        data.dialogueText = text;
        data.choices = DialogueChoiceFactory.GenerateBasicChoices();
        DialogueManager.instance.currentNpc=villager.gameObject;

        if(DialogueChoiceExecuter.instance != null)
        {
            DialogueChoiceExecuter.instance.SetContext(villager.gameObject);
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        DialogueManager.instance.dialogueUI.ShowDialogue(data);
    }
}