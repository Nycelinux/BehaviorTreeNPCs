using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthUI : MonoBehaviour
{
    [SerializeField] private Health playerHealth;
    [SerializeField] private Slider slider;

    private void Start()
    {
        if (playerHealth == null) return;
        slider.minValue = 0;
        slider.maxValue = playerHealth.maxHealth;
        playerHealth.OnHealthChanged += UpdateUI;
        UpdateUI(playerHealth.GetHealth(), playerHealth.maxHealth);
    }
    void UpdateUI(int currentHealth, int maxHealth)
    {
        slider.maxValue =maxHealth;
        slider.value = currentHealth;
    }

    private void OnDestroy()
    {
        if(playerHealth != null)
            playerHealth.OnHealthChanged -= UpdateUI;
    }
}
