using UnityEngine;

[CreateAssetMenu(fileName = "SpeedBoostPowerUp", menuName = "Game/PowerUp/SpeedBoost")]
public class SpeedBoostPowerUpSO : PowerUpSO
{
    public float speedMultiplier = 2f;

    public override void ApplyEffect(GameObject target)
    {
        // Find the player's movement component and apply the speed boost
        PlayerMovement playerMovement = target.GetComponent<PlayerMovement>();
        if (playerMovement != null)
        {
            playerMovement.ApplySpeedBoost(speedMultiplier, duration);
        }
    }
}

