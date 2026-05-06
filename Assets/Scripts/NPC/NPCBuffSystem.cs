using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCBuffSystem : MonoBehaviour
{
    public static NPCBuffSystem instance;
    void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    public void ApplyCalmBuff()
    {
        NPCReaction[] npcs = FindObjectsOfType<NPCReaction>();
        foreach(var npc in npcs)
        {
            npc.ReduceFearInstant(30f);
        }
        Debug.Log("Alle NPCs beruhigt!");
    }
}
