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
            return;

        Debug.Log("Choice selected: " + choice.choiceText);
        GameManager.instance.reputation += choice.reputationChange;

        var relation = currentNPC.GetComponent<RelationshipData>();
        if (relation != null)
        {
            relation.ApplyEffect(choice.relationShipEffect);
        }

        var memory = currentNPC.GetComponent<NPC_Memory>();
        if (memory != null)
        {
            memory.AddMemory("Player choice: " + choice.choiceText);
        }
        /* (choice.startQuest)
        {
            QuestManager.instance.AddQuest(new Quest { 
                questID= choice.questID,
                questName= choice.questID.ToString(),
                description= "",
                requiredAmount=1,
                currentAmount=0,
                questType= Quest.QuestType.Investigate
            });
        }

        if (choice.changeStoryChange)
        {
            var priest = currentNPC.GetComponent<NPC_Priest>();
            if (priest != null)
                StoryManager.instance.StartStage(choice.nextStage);
            else
                Debug.Log("Story Stage Change blockiert: kein Priester NPC");
        }*/
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
