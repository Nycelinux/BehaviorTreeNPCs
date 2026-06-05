using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConvaiContextProvider : MonoBehaviour
{
    private NPC_Villager villager;
    private NPC_Memory memory;
    private NPCReaction reaction;
    private RelationshipData relationshipData;

    void Awake()
    {
        villager = GetComponent<NPC_Villager>();
        memory = GetComponent<NPC_Memory>();
        reaction = GetComponent<NPCReaction>();
        relationshipData = GetComponent<RelationshipData>();
    }

    public string BuildContext()
    {
        string context = "";
        if(villager != null)
        {
            context += "Name: " + villager.npcName + "\n";
            context += "Problem: " + villager.PersonalProblem + "\n";
        }
        if (memory != null)
        {
            context += "Memories: \n";
            context += memory.GetMemoryText();
        }
        if (reaction != null)
        {
            context += "Hunger: " + reaction.hunger + "\n";
            context += "Faith: " + reaction.faith + "\n";
            context += "Fear: " + reaction.fear + "\n";
            context += "Loyality: " + reaction.loyality + "\n";
        }
        if (relationshipData != null)
        {
            context += relationshipData.GetRelationShipSummary() + "\n";
        }
        return context;
    }
}
