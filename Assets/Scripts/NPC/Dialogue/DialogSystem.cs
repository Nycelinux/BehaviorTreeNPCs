using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class DialogSystem : MonoBehaviour
{
    public static DialogSystem instance;
    private void Awake()
    {
        instance = this;
    }
    public void OpenPriestDialogue()
    {
        switch (StoryManager.instance.currentStage)
        {
            case StoryStage.TempleVisits:
                OpenTempleIntroduction();
                break;
            case StoryStage.GatherWood:
                OpenWoodDialogue();
                break;
            case StoryStage.GainPriestTrust:
                OpenPriestTrustDialogue();
                break;
            default:
                    Debug.Log("Die Priesterin schweigt");
                break;
        }
    }
    void OpenTempleIntroduction()
    {
        Debug.Log("Priesterin: Willkommen Bürgermeister");
    }
    void OpenWoodDialogue()
    {
        GameManager.instance.reputation += 5;
        UIManager.instance.wood -= 5;
        Debug.Log("Zolle der Priesterin Respekt");
      
    }
   
    void OpenPriestTrustDialogue()
    {
        Debug.Log("Vielleicht kann ich euch vertrauen");
    }
}
