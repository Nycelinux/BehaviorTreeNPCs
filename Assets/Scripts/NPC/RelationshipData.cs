using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelationshipData : MonoBehaviour
{
    [Range(-100, 100)]
    public float trust = 0;

    [Range(0, 100)]
    public float fearOfPlayer = 0;

    [Range(0, 100)]
    public float friendship = 0;

    [Range(0, 100)]
    public float respect = 50;

    public void ChangeTrust(float amount)
    {
        trust = Mathf.Clamp(trust + amount,-100,100);
    }

    public void ChangeFear(float amount)
    {
        fearOfPlayer = Mathf.Clamp(fearOfPlayer + amount,0, 100);
    }

    public void ChangeFriendship(float amount)
    {
        friendship = Mathf.Clamp(friendship + amount,0, 100);
    }

    public void ChangeRespect(float amount)
    {
        respect = Mathf.Clamp(respect + amount,0, 100);
    }
    public void ApplyEffect(DialogueRelationShipEffect effect)
    {
        trust += effect.trustChange;
        fearOfPlayer+= effect.fearChange;
        friendship += effect.friendshipChange;
        respect += effect.respectChange;
        Debug.Log("trust: " + trust + ", fear of player: " + fearOfPlayer + ", friendship: "+ friendship + ", respect: "+ respect);

        NPCReaction reaction = GetComponent<NPCReaction>();
        if(reaction != null)
        {
            reaction.loyality += effect.loyalityChange;
            reaction.loyality = Mathf.Clamp(reaction.loyality,0,100);
            Debug.Log("loyality: " + reaction.loyality);
        }

        trust = Mathf.Clamp(trust,-100, 100);
        fearOfPlayer = Mathf.Clamp(fearOfPlayer,0, 100);
        friendship = Mathf.Clamp(friendship,0, 100);
        respect = Mathf.Clamp(respect,0, 100);
    }
    public string GetRelationShipState()
    {
        if (trust< -50)
            return "Hostile";
        if (trust> 50)
            return "Trusted";
        if (fearOfPlayer> 70)
            return "Terrified";
        if (friendship> 40 && friendship <= 80)
            return "Friend";
        if (friendship > 80)
            return "Bestfriend";
        return "Neutral";
    }

    public string GetRelationShipSummary()
    {
        return "Trust: " + trust + ", Fear: " + fearOfPlayer + ", Friendship: " + friendship + ", State: " + GetRelationShipState();
    }
}
