using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CollectRessource : Node
{
    private float timer = 0f;
    private bool destinationSet = false;
    private RessourceNode currentResource;
    private NavMeshAgent agent;
    private NPCReaction reaction;
    private NPC_Villager villager;
    private static HashSet<NPC_Villager> allowedGathers = new();
    public CollectRessource(Blackboard blackboard ,NavMeshAgent agent, NPCReaction reaction, NPC_Villager villager):base(blackboard)
    {
        this.agent = agent;
        this.reaction = reaction;
        this.villager = villager;
    }

    public override NodeState Evaluate()
    {
        if (!villager.CanAutoGather())
        {
            ReleaseRessource();
            return NodeState.FAILURE;
        }
        if (GameManager.instance == null)
        {
            Debug.LogError("GameManager fehlt");
            return NodeState.FAILURE;
        }
        if (GameManager.instance.isNight)
        {
            ReleaseRessource();
            return NodeState.FAILURE;
        }
        if (agent == null||!agent.isOnNavMesh)
        {
            Debug.LogError("Navagent fehlt/ istnicht auf NavMesh!");
            return NodeState.FAILURE;
        }

        if (currentResource == null)
        {
            //Ressourcetyp desiredType = villager.preferredResource;

            //if (reaction != null && reaction.hunger > 70)
            //desiredType = Ressourcetyp.Food;
            currentResource = ResourceManager.instance.GetFreeResource(villager.preferredResource);
            Debug.Log(villager.npcName + " sucht " + villager.preferredResource);

            if (currentResource == null)
            {
                Debug.LogWarning(villager.npcName + " findet keine Resource vom Typ: " + villager.preferredResource);

                return NodeState.RUNNING;
            }
            //currentResource.Reserve();
            destinationSet = false;
            timer = 0f; 
        }

        if (!destinationSet)
        {
            agent.SetDestination(currentResource.transform.position);
            destinationSet = true;
            Debug.Log(villager.npcName + " läuft zu " + currentResource.name);

        }
        if (currentResource== null||!currentResource.HasResources())
        {
            ReleaseRessource();
            return NodeState.FAILURE;
        }

        float distance = agent.remainingDistance;
        //Debug.Log(villager.npcName + " Distanz: " + distance + " StopDistance: " + agent.stoppingDistance);
        Debug.Log(villager.npcName +" remainingDistance=" +agent.remainingDistance +" pathPending=" +agent.pathPending);
        if (agent.pathPending)
            return NodeState.RUNNING;
        if (agent.remainingDistance > agent.stoppingDistance )
        {
            return NodeState.RUNNING;
        }
            
        Debug.Log(" sammelt jetzt ");
        float gatherTime = 2f;
                
        if (reaction != null)
        {
            gatherTime += reaction.hunger * 0.05f;
            if (reaction.GetFear() > 70)
                gatherTime += 3f;
            if (reaction.faith > 80)
                gatherTime -= 1f;
            if (reaction.loyality> 70)
                gatherTime -= 1f;
        }
        Debug.Log(villager.npcName + "Timer:" + timer + " / "+ gatherTime);
        timer += Time.deltaTime;
        if (timer < gatherTime)
            return NodeState.RUNNING;
                
        float reputation = GameManager.instance.reputation;
        int amount = 1;
        if (reputation > 70) amount = 3;
        else if (reputation > 40) amount = 2;
        else if (reputation > 20) amount = 1;
        else amount = 0;
        Debug.Log(" sammelt: " + amount);
        Debug.Log("FINAL GATHER: " + villager.npcName + " -> " + currentResource.ressourcetyp);
        currentResource.UseResource();
        if (UIManager.instance != null)
            UIManager.instance.AddResources(currentResource.ressourcetyp, amount);
        if (StoryManager.instance.currentStage == StoryStage.GatherWood && currentResource.ressourcetyp == Ressourcetyp.Wood)
            QuestManager.instance.ProgressQuest(QuestID.GatherWood, amount);

  
        ReleaseRessource();
        timer = 0f;
        destinationSet = false;
        return NodeState.SUCCESS;


    }
      

    private void ReleaseRessource()
    {
        if(currentResource != null)
        {
            currentResource.Release();
            currentResource = null;
        }
        timer = 0f;
        destinationSet = false;
    }

    void OnDisable()
    {
        ReleaseRessource();
    }
}
