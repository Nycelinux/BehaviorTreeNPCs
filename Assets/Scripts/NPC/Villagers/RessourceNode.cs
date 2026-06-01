using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Ressourcetyp
{
    Stone,
    Wood,
    //FaithArtifact,
    Food
}

public class RessourceNode : MonoBehaviour
{
    public Ressourcetyp ressourcetyp;
    public bool isOccupied = false;
    public int maxUses = 2;
    private int currentUses = 0;

    public bool HasResources()
    {
        return currentUses < maxUses;
    }

    public void Reserve()
    {
        isOccupied = true;
    }

    public void Release()
    {
        isOccupied = false;
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

     void Update()
    {
        if (currentUses >= maxUses)
            isOccupied = false;
    }
}
