using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager instance;
    public List<RessourceNode> resourcePoints = new List<RessourceNode>();
    private bool lastNightState = false;
    void Awake()
    {
        instance = this;
        resourcePoints.AddRange(FindObjectsOfType<RessourceNode>());
    }

    void Update()
    {
        if(GameManager.instance.isNight && !lastNightState)
        {
            ResetAllResources();
        }
        lastNightState = GameManager.instance.isNight;
    }

    public RessourceNode GetFreeResource(Ressourcetyp type)
    {
        foreach(var point in resourcePoints)
        {
            if (point.ressourcetyp!= type)
            {
                Debug.Log(point.name + " type=" + point.ressourcetyp + " requested=" + type);
                continue;
            }
            if (!point.HasResources())
            {
                continue;
            }
            if (point.isOccupied)
            {
                continue;
            }
   
            point.Reserve();
            return point;
            
        }
        foreach(var point in resourcePoints)
        {
            if (!point.HasResources())
            {
                continue;
            }
            if (point.isOccupied)
            {
                continue;
            }
            point.Reserve();
            return point;
        }

        return null;
    }

    private void ResetAllResources()
    {
        Debug.Log(" Ressourcen zurücksetzen");
        foreach(var point in resourcePoints)
        {
            point.ResetResources();
        }
    }

    public void ReduceResources()
    {
        foreach(var point in resourcePoints)
        {
            point.maxUses = Mathf.Max(1, point.maxUses - 1);
        }
        Debug.Log("Ernte verschlechtert");
    }
}
