using UnityEngine;
using TMPro; 
public class PotionCounter : MonoBehaviour
{
    public static PotionCounter Instance;

    public TextMeshProUGUI counterText; 
    private int potionsConsumed = 0;

    // Call this when the player consumes a potion
    public void AddPotion(int amount = 1)
    {
        potionsConsumed += amount;
        UpdateUI();
    }

    private void UpdateUI()
    {
        counterText.text = "Potions: " + potionsConsumed;
    }

    
    public void ResetCounter()
    {
        potionsConsumed = 0;
        UpdateUI();
    }
}