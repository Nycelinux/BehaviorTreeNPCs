using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VillageBuilding : MonoBehaviour
{
    public int buildingHealth = 100;

    public void TakeDamage(int damage)
    {
        buildingHealth -= damage;
        VillageDefenseSystem.instance.villageHealth -= damage;
        if (buildingHealth <= 0)
            Destroy(gameObject);
    }
}
