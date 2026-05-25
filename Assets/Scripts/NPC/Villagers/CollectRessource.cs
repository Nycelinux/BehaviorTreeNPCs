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
            return NodeState.FAILURE;
        }
        if (GameManager.instance == null)
        {
            Debug.LogError("GameManager fehlt!");
            return NodeState.FAILURE;
        }

        float fear = reaction != null ? reaction.GetFear() : 0f;

        float gatherTime = 5f;
        if (reaction != null)
        {
            gatherTime += reaction.hunger * 0.05f;
            if (reaction.GetFear() > 70)
                gatherTime += 3f;
            if (reaction.faith > 80)
                gatherTime -= 1f;
            if (reaction.loyality > 70)
                gatherTime -= 1.5f;
        }
        //hier weiter 
        if (isWaiting)
        {
            waitTimer += Time.deltaTime;
            if (waitTimer >= waitTime)
            {
                isWaiting = false;
                waitTimer = 0f;
            }
            return NodeState.RUNNING;
        }

        if (GameManager.instance.isNight)
        {
            ReleaseRessource();
            return NodeState.FAILURE;
        }

        if (currentResource == null) {
            Ressourcetyp desiredType = villager.preferredResource;
            
            if (reaction.hunger > 70)
                desiredType = Ressourcetyp.Food;
            currentResource = ResourceManager.instance.GetFreeResource(desiredType);
            if (currentResource == null)
            {
                Debug.LogWarning(villager.npcName + " findet keine Resource vom Typ: " + desiredType);
                return NodeState.FAILURE;
            }
        }


       
        if (agent == null)
        {
            Debug.LogError("NavMesh agent missing! ");
            return NodeState.FAILURE;
        }
        agent.SetDestination(currentResource.transform.position);

        if (Vector3.Distance(agent.transform.position,currentResource.transform.position)<1.5f)
        {

            timer += Time.deltaTime;
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
                if(UIManager.instance !=null)
                    UIManager.instance.AddResources(currentResource.ressourcetyp,amount);
                if (StoryManager.instance.currentStage == StoryStage.GatherWood)
                    QuestManager.instance.ProgressQuest(QuestID.GatherWood, amount);
                timer = 0f;

                if(!currentResource.HasResources())
                    ReleaseRessource();
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
            currentResource.isOccupied = false;
            currentResource = null;
        }
    }

    void OnDisable()
    {
        ReleaseRessource();
    }
}
