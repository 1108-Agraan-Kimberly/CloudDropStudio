using UnityEngine;
using UnityEngine.UI;

public class heartDisplay : MonoBehaviour
{
    public Health healthScript;
    public Image[] hearts_display;

    private void Start()
    {
        if (healthScript == null)
        {
            // try to find player's Health by tag as a fallback
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                healthScript = p.GetComponent<Health>();
        }

        if (healthScript != null)
        {
            healthScript.OnHealthChanged += UpdateHeartDisplay;
            UpdateHeartDisplay(healthScript.currentHealth);
        }
        else
        {
            Debug.LogWarning("heartDisplay: Health reference not set and Player tagged object not found.");
        }
    }

    private void OnDestroy()
    {
        if (healthScript != null)
            healthScript.OnHealthChanged -= UpdateHeartDisplay;
    }

    private void UpdateHeartDisplay(int currentHealth)
    {
        if (hearts_display == null) return;

        for (int i = 0; i < hearts_display.Length; i++)
        {
            Image img = hearts_display[i];
            if (img == null) continue;
            img.enabled = i < currentHealth;
        }
    }
}
