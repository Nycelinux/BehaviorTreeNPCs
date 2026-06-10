using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerResourceGather : MonoBehaviour, IInteractable
{
    public InteractType GetInteractType() => InteractType.Resource;
    public int gatherAmount = 1;
    public RessourceNode ressourceNode;

    // Start is called before the first frame update
    public void Interact(Vector3 hitPoint)
    {
    Debug.Log("Resource gesammelt");

    if (ressourceNode == null)
            return;
        if (!ressourceNode.HasResources())
        {
            Debug.Log("Keine Ressourcen mehr");
            return;
        }

        ressourceNode.UseResource();
        if(UIManager.instance != null)
        {
            UIManager.instance.AddResources(ressourceNode.ressourcetyp, gatherAmount);
        }
        if (StoryManager.instance.currentStage == StoryStage.GatherWood && ressourceNode.ressourcetyp == Ressourcetyp.Wood)
        {
            Debug.Log("HOLZ QUEST TRIGGER");
            Debug.Log("Current Stage: " + StoryManager.instance.currentStage);
            Debug.Log("Resource Type: " + ressourceNode.ressourcetyp);
            QuestManager.instance.ProgressQuest(QuestID.GatherWood, gatherAmount);
        }
            

        Debug.Log("Spieler sammelt: " + ressourceNode.ressourcetyp);
    }

}
