using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RessourceNode : MonoBehaviour
{
    public bool isOccupied = false;
    public int maxUses = 2;
    private int currentUses = 0;

    public bool HasResources()
    {
        return currentUses < maxUses;
    }

    public void UseResource()
    {
        currentUses++;
    }

    public void ResetResources()
    {
        currentUses = 0;
        isOccupied = false;
    }

    public int GetRemaining()
    {
        return maxUses - currentUses;
    }
}
