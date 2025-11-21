using UnityEngine;

// This attribute allows you to create instances of the ScriptableObject 
// directly from the Unity Editor's Assets menu.
[CreateAssetMenu(fileName = "NewPowerUp", menuName = "Game/PowerUp")]
public abstract class PowerUpSO : ScriptableObject
{
    public string powerUpName;
    public Sprite powerUpIcon;
    public float duration; // Duration of the power-up

    // Abstract method to be implemented by specific power-ups
    public abstract void ApplyEffect(GameObject target);
}

