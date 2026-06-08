using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvaiDialogueService : MonoBehaviour
{
    public static ConvaiDialogueService instance;

    private void Awake()
    {
        instance = this;
    }

    public void StartConversation(NPC_Villager villager, string context)
    {
        ConvaiResponseRouter.Instance.OnTextResponse += HandleResponse;
        ConvaiResponseRouter.Instance.Send(villager, context);
    }

    private void HandleResponse(string text, NPC_Villager villager)
    {
        ConvaiResponseRouter.Instance.OnTextResponse -= HandleResponse;

        DialogueData data = ScriptableObject.CreateInstance<DialogueData>();
        data.npcName = villager.npcName;
        data.dialogueText = text;
        data.choices = DialogueChoiceFactory.GenerateBasicChoices();

        DialogueManager.instance.dialogueUI.ShowDialogue(data);
    }
}