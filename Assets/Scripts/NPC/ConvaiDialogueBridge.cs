using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Convai.Scripts.Runtime.Core;


public class ConvaiDialogueBridge : MonoBehaviour
{
    public static ConvaiDialogueBridge instance;
    private ConvaiNPC currentActiveConvaiNPC;
    private NPC_Villager currentVillager;
    private bool isListening;
    private string lastResponse = "";
    private void Awake()
    {
        instance = this;
    }

    public void RequestDialogue(NPC_Villager nPC_Villager,string context)
    {
        currentVillager = nPC_Villager;
        currentActiveConvaiNPC = nPC_Villager.GetComponent<ConvaiNPC>();
        if(currentActiveConvaiNPC == null)
        {
            Debug.LogError($"objekt {nPC_Villager.npcName} hat keine ConvaiKomponente");
            return;
        }
        Debug.Log("Senset Context an Convai...");

        isListening = true;
        currentActiveConvaiNPC.SendTextDataAsync(context);
        StartCoroutine(PollResponse());
    }

    private IEnumerator PollResponse()
    {
        float timeout = 15f;
        float timer = 0f;

        while (isListening && timer < timeout)
        {
            timer += Time.deltaTime;

            string response = TryGetLatestTextResponse();

            if (!string.IsNullOrEmpty(response) && response != lastResponse)
            {
                lastResponse = response;
                HandleConvaiResponse(response);
                yield break;
            }

            yield return new WaitForSeconds(0.1f);
        }

        Debug.LogWarning("Convai Response Timeout");
        isListening = false;
    }

    private string TryGetLatestTextResponse()
    {
        var field = typeof(ConvaiNPC).GetField("_currentResponseText",
            System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.Instance);

        if (field != null)
        {
            return field.GetValue(currentActiveConvaiNPC) as string;
        }

        return null;
    }
   

    private void HandleConvaiResponse(string responseContext)
    {
        Debug.Log("Context AI Convai: " + responseContext);
        DialogueData dialogue = ScriptableObject.CreateInstance<DialogueData>();
        dialogue.npcName = currentVillager.npcName;
        dialogue.dialogueText = responseContext;
        dialogue.choices = DialogueChoiceFactory.GenerateBasicChoices();
        DialogueManager.instance.dialogueUI.ShowDialogue(dialogue);
        isListening = false;
      
    }
    public void UnsubscribeFromNpc()
    {
        isListening = false;
        currentActiveConvaiNPC = null;
        currentVillager = null;
    }

}
