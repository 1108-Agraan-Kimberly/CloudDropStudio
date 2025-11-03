using UnityEngine;
using UnityEngine.UI;

public class heartDisplay : MonoBehaviour
{
    public Health healthScript;
    public Image[] hearts_display;

    private void Start()
    {
        healthScript.OnHealthChanged += UpdateHeartDisplay;
        UpdateHeartDisplay(healthScript.currentHealth);
    }

    private void OnDestroy()
    {
        healthScript.OnHealthChanged -= UpdateHeartDisplay;
    }

    private void UpdateHeartDisplay(int currentHealth)
    {
        for (int i = 0; i < hearts_display.Length; i++)
        {
            hearts_display[i].enabled = i < currentHealth;
        }
    }
}
