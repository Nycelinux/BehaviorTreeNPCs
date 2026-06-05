using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueButtonUI : MonoBehaviour
{
    private DialogueChoice currentChoice;
    public TMP_Text buttonnText;
    public DialogueRelationShipEffect relationShipEffect;
    public void Setup(DialogueChoice choice)
    {
        currentChoice = choice;
        buttonnText.text = choice.choiceText;
        GetComponent<Button>()
            .onClick
            .RemoveAllListeners();
        GetComponent<Button>()
            .onClick
            .AddListener(Choose);

    }
    void Choose()
    {
        Debug.Log("Choice gewählt: " + currentChoice.choiceText);
        GameManager.instance.reputation += currentChoice.reputationChange;

        if(DialogueManager.instance.currentNpc != null)
        {
            RelationshipData relationshipdata = DialogueManager.instance.currentNpc.GetComponent<RelationshipData>();
                 
            if(relationshipdata != null && currentChoice.relationShipEffect != null)
            {
                relationshipdata.ApplyEffect(currentChoice.relationShipEffect);
            }

            NPCReaction reaction = DialogueManager.instance.currentNpc.GetComponent<NPCReaction>();
            if(reaction != null)
            {
                reaction.fear += currentChoice.relationShipEffect.fearChange;
                reaction.loyality += currentChoice.relationShipEffect.loyalityChange;
            }
            
            NPC_Memory memory = DialogueManager.instance.currentNpc.GetComponent<NPC_Memory>();
            if(memory != null)
            {
                memory.AddMemory(" Player sagte: " + currentChoice.choiceText);
            }
        }
        DialogueManager.instance.EndDialogue();
    }


    void ApplyChoice(DialogueChoice choice)
    {
        NPCReaction reaction = DialogueManager.instance.currentNpc.GetComponent<NPCReaction>();
        if(reaction != null)
        {
            reaction.loyality += choice.relationShipEffect.loyalityChange;
            reaction.fear += choice.relationShipEffect.fearChange;
        }

        GameManager.instance.reputation += choice.reputationChange;
        DialogueManager.instance.EndDialogue();
            
    }
   
}
