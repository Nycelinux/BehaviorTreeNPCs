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
        if (currentNPC == null)
        {
            Debug.LogError(" currentNPC ist null");
            return;
        }

        Debug.Log("Choice selected: " + choice.choiceText);
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
        if (reaction != null)
        {
            Debug.Log("Fear: " + reaction.fear);
            Debug.Log("Loyality: " + reaction.loyality);
        }
       
        DialogueManager.instance.EndDialogue();
        var context = currentNPC.GetComponent<ConvaiContextProvider>();
        if (context != null)
        {
            ConvaiResponseRouter.Instance.Send(
                currentNPC.GetComponent<NPC_Villager>(),
                context.BuildContext()
            );
        }
    }
}
