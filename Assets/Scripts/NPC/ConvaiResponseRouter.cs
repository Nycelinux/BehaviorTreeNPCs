using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Convai.Scripts.Runtime.Core;
using System;

public class ConvaiResponseRouter : MonoBehaviour
{
    public static ConvaiResponseRouter Instance;

    private ConvaiNPC activeNPC;
    private NPC_Villager activeVillager;

    public event Action<string, NPC_Villager> OnTextResponse;

    private void Awake()
    {
        Instance = this;
    }

    private bool isBusy;
    public void Send(NPC_Villager villager, string context)
    {
        if (isBusy)
        {
            Debug.LogWarning("Convai already running");
            return;
        }
        isBusy = true;
        if (villager == null)
        {
            isBusy = false;
            return;
        }

        if (!villager.hasConvai)
        {
            isBusy = false;
            Debug.LogWarning("Blocked Convai for: " + villager.npcName);
            return;
        }

        Debug.Log("=== ConvaiResponseRouter.Send ===");
        activeVillager = villager;
        activeNPC = villager.GetComponent<ConvaiNPC>();
        Debug.Log("activeNPC = " + activeNPC);

        if (activeNPC == null)
        {
            Debug.Log("NPC Object = " + activeNPC.gameObject.name);
        }
        Debug.Log("Villager = " + villager.npcName);
        Debug.Log("ConvaiNPC = " + activeNPC);
        if (activeNPC == null)
        {
            isBusy = false;
            NPC_Villager.convaiGlobalLock = false;
            Debug.LogError("ConvaiNPC fehlt auf " + villager.npcName);
            return;
        }

        Debug.Log("Convai SEND: " + context);

        var rel = villager.GetComponent<RelationshipData>();
        if (rel != null)
        {
            if (rel.trust < 0)
                context += "\nThe player is not trusted.";

            if (rel.fearOfPlayer > 70)
                context += "\nThe villager is afraid of the player.";

            if (rel.friendship > 60)
                context += "\nThe villager considers the player a friend.";
        }
        Debug.Log("Character ID = [" + activeNPC.characterID + "]");
        Debug.Log("Session ID = [" + activeNPC.sessionID + "]");
        Debug.Log("Is Character Active = " + activeNPC.isCharacterActive);
        Debug.Log("Client: " + activeNPC.GetType());
        ConvaiNPCManager.Instance.SetActiveConvaiNPC(activeNPC);
        activeNPC.SendTextDataAsync(context);
        Debug.Log("SEND WURDE AUSGEFÜHRT");
    }

    private void Update()
    {
        Debug.Log("Polling Convai...");
        if (activeNPC == null||activeVillager== null) return;

        TryReadResponse();
    }

    private void TryReadResponse()
    {
        var field = typeof(ConvaiNPC).GetField("_getResponseResponses",
            System.Reflection.BindingFlags.NonPublic |
            System.Reflection.BindingFlags.Instance);

        if (field == null) return;

        var queue = field.GetValue(activeNPC) as System.Collections.ICollection;
        if (queue == null || queue.Count == 0) return;

        foreach (var item in queue)
        {
            if (item == null) continue;

            var type = item.GetType();

            var audioProp = type.GetProperty("AudioResponse");
            if (audioProp == null) continue;

            var audio = audioProp.GetValue(item);
            if (audio == null) continue;

            var textProp = audio.GetType().GetProperty("TextData");
            if (textProp == null) continue;

            string text = textProp.GetValue(audio) as string;
            if (string.IsNullOrEmpty(activeNPC.sessionID))
            {
                Debug.LogError("SESSION NULL");
                return;
            }
            if (!string.IsNullOrEmpty(text))
            {
                var villager = activeVillager;
                var memory = activeVillager.GetComponent<NPC_Memory>();
                if (memory != null)
                {
                    memory.AddMemory(text);
                }

                OnTextResponse?.Invoke(text, activeVillager);
                activeNPC = null;
                activeVillager = null;
                isBusy = false;
                return;
            }
        }
        Debug.Log("Queue Count = " + queue.Count);
    }
}