using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class CollectRessource : Node
{
    private float timer = 0f;
    private float waitTimer = 0f;
    private float waitTime = 2f;
    private bool isWaiting = false;
    private bool destinationSet = false;
    private RessourceNode currentResource;
    private NavMeshAgent agent;
    private NPCReaction reaction;
    private NPC_Villager villager;
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
            Debug.LogError("GameManager fehlt!");
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
            Ressourcetyp desiredType = villager.preferredResource;

            if (reaction != null && reaction.hunger > 70)
                desiredType = Ressourcetyp.Food;
            currentResource = ResourceManager.instance.GetFreeResource(desiredType);


            if (currentResource == null)
            {
                Debug.LogWarning(villager.npcName + " findet keine Resource vom Typ: " + desiredType);
                return NodeState.FAILURE;
            }
            currentResource.Reserve();
            destinationSet = false;
        }

        if (!destinationSet)
        {
            agent.SetDestination(currentResource.transform.position);
            destinationSet = true;
        }
        if (!currentResource.HasResources())
        {
            ReleaseRessource();
            return NodeState.FAILURE;
        }


        
            if (!agent.pathPending && agent.hasPath && agent.remainingDistance <= agent.stoppingDistance +0.5f)
            {
                Debug.Log(" sammelt jetzt ");
                timer += Time.deltaTime;
                float gatherTime = 5f;
                if(reaction != null)
                {
                gatherTime += reaction.hunger * 0.05f;
                if (reaction.GetFear() > 70)
                    gatherTime += 3f;
                if (reaction.faith > 80)
                    gatherTime -= 1f;
                if (reaction.loyality> 70)
                    gatherTime -= 1f;
            }
            
                if (timer >= gatherTime)
                {
                    float reputation = GameManager.instance.reputation;
                    int amount = 1;
                    if (reputation > 70) amount = 3;
                    else if (reputation > 40) amount = 2;
                    else if (reputation > 20) amount = 1;
                    else amount = 0;
                    Debug.Log(" sammelt: " + amount);
                    currentResource.UseResource();
                    if (UIManager.instance != null)
                        UIManager.instance.AddResources(currentResource.ressourcetyp, amount);
                    if (StoryManager.instance.currentStage == StoryStage.GatherWood && currentResource.ressourcetyp == Ressourcetyp.Wood)
                        QuestManager.instance.ProgressQuest(QuestID.GatherWood, amount);

                    bool empty = !currentResource.HasResources();
                    timer = 0f;
                    ReleaseRessource();
                    if (empty)
                        Debug.Log("Resource leer");
                    isWaiting = true;

                    return NodeState.SUCCESS;
                }
                return NodeState.RUNNING;
            
        }
       return NodeState.RUNNING;
       
    }

    private void ReleaseRessource()
    {
        if(currentResource != null)
        {
            currentResource.Release();
            currentResource = null;
        }
    }

    void OnDisable()
    {
        ReleaseRessource();
    }
}
