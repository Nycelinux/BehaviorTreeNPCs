using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueChoiceExecuter : MonoBehaviour
{
    public static DialogueChoiceExecuter instance;

    //private NPC_Villager currentVillager;
    private GameObject currentNPC;

    private void Awake()
    {
        instance = this;
    }

    public void SetContext(GameObject npc)
    {
        currentNPC = npc;
    }

    public void ExecuteChoice(DialogueChoice choice)
    {
        Debug.Log("=== ExecuteChoice ===");

        if (choice == null)
        {
            Debug.LogError("Choice ist NULL");
            return;
        }
        if (currentNPC == null)
        {
            Debug.LogError(" currentNPC ist null");
            return;
        }

        Debug.Log("Choice selected: " + choice.choiceText + " for NPC: "+currentNPC.name);
        GameManager.instance.reputation += choice.reputationChange;

        var relation = currentNPC.GetComponent<RelationshipData>();
        if (relation != null)
        {
            relation.ApplyEffect(choice.relationShipEffect);
            Debug.Log("Friendship: " + relation.friendship);
            Debug.Log("Trust: " + relation.trust);
            Debug.Log("Respect: " + relation.respect);
        }

        var memory = currentNPC.GetComponent<NPC_Memory>();
        if (memory != null)
        {
            memory.AddMemory("Player choice: " + choice.choiceText);
        }

        NPCReaction reaction = currentNPC.GetComponent<NPCReaction>();
        if (reaction != null && choice.relationShipEffect!= null)
        {
            reaction.fear +=choice.relationShipEffect.fearChange;

            reaction.loyality +=choice.relationShipEffect.loyalityChange;
            Debug.Log("Fear: " + reaction.fear);
            Debug.Log("Loyality: " + reaction.loyality);
        }

        NPC_Villager villager = currentNPC.GetComponent<NPC_Villager>();
        DialogueManager.instance.EndDialogue();

        if (villager != null && villager.hasConvai && ConvaiResponseRouter.Instance != null)
        {
            var context = currentNPC.GetComponent<ConvaiContextProvider>();

            if (context != null && ConvaiResponseRouter.Instance != null)
            {
                ConvaiResponseRouter.Instance.Send(villager, context.BuildContext());
            }
        }
        currentNPC = null;
    }
}
