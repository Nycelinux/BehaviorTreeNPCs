using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Time")]
    public float dayLength = 300f;// 60 Sekunden = ein Tag
    private float timer = 0f;

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
}
