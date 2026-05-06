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
    public CollectRessource(NavMeshAgent agent, NPCReaction reaction)
    {
        this.agent = agent;
        this.reaction = reaction;
    }

    public override NodeState Evaluate()
    {
        if (GameManager.instance == null)
        {
            Debug.LogError("GameManager fehlt!");
            return NodeState.FAILURE;
        }

        float fear = reaction != null ? reaction.GetFear() : 0f;

        float gatherTime = 5f;
        if (fear > 70) gatherTime = 8f;
        else if (fear > 30) gatherTime = 6f;
        //hier weiter 
        if (isWaiting)
        {
            waitTimer += Time.deltaTime;
            if(waitTimer >= waitTime)
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
            
        if(currentResource == null) {
            currentResource = ResourceManager.instance.GetFreeResource();
            if (currentResource == null)
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
                    UIManager.instance.AddResources(amount);
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
