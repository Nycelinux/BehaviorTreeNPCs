using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Time")]
    public float dayLength = 300f;

    private float timer = 0f;
    private float hungerTimer = 0f;

    public bool isNight;

    [Header("Reputation")]
    public float reputation = 50;

    [Header("Skybox")]
    public Material daybox;
    public Material nightbox;

    void Awake()
    {
        instance = this;
    }

    void Update()
    {
        UpdateTime();
        UpdateSkybox();
        hungerTimer += Time.deltaTime;
        if(hungerTimer >= 20f)
        {
            hungerTimer = 0f;
            ConsumeFood();
        }
    }

    void UpdateTime()
    {
        timer += Time.deltaTime;
        if (timer >= dayLength)
        {
            timer = 0f;
        }

        isNight = timer > (dayLength / 2f);
    }

    void UpdateSkybox()
    {
        if (isNight && RenderSettings.skybox != nightbox)
        {
            RenderSettings.skybox = nightbox;
        }
        else if (!isNight && RenderSettings.skybox != daybox)
        {
            RenderSettings.skybox = daybox;
        }
    }

    public void ChangeReputation(float amount)
    {
        reputation += amount;
        reputation = Mathf.Clamp(reputation, 0, 100);
    }

    void ConsumeFood()
    {
        int villagers = FindObjectsOfType<NPC_Villager>().Length;
        UIManager.instance.food -= villagers;
        if(UIManager.instance.food < 0)
        {
            UIManager.instance.food = 0;
            NPCReaction[] npcs = FindObjectsOfType<NPCReaction>();

            foreach(var npc in npcs)
            {
                if (UIManager.instance.food > 0){
                    UIManager.instance.food--;
                    npc.hunger -= 30f;
                }
                else
                {
                    npc.loyality -= 5f;
                    npc.hunger += 20f;
                    npc.fear += 10f;
                }
                
            }
            Debug.Log("Die Bewohner hungern!");
        }
    }
}
