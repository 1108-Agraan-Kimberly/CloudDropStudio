using UnityEngine;

public class wolfieFight : MonoBehaviour
{
    public float detectionRange = 8f;
    public float fireCooldown = 1.5f;
    private float fireTimer = 0f;

    public Transform firePoint;
    public GameObject projectilePrefab;

    private Transform player;
    private bool playerInRange = false;

    [Header("Attack chances and amounts")]
    [Range(0f, 1f)]
    public float healChance = 0.2f; // chance that a fired projectile heals the player
    public int attackAmount = 1;    // damage amount when projectile hurts
    public int healAmount = 1;      // heal amount when projectile heals

    private wolfie_movement movement;

    void Start()
    {
        movement = GetComponent<wolfie_movement>();
        if (movement == null)
            Debug.LogWarning("wolfieFight: wolfie_movement not found on same GameObject. Waiting-check will be skipped.");
    }

    private void Update()
    {
        if (fireTimer > 0)
            fireTimer -= Time.deltaTime;

        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);
        playerInRange = distance <= detectionRange;

        if (playerInRange)
        {
            // Only attack if movement exists and the wolf is currently waiting.
            // If movement is missing, fallback to allow attacking.
            if (movement == null || movement.IsWaiting)
                ShootAtPlayer();
        }
    }

    private void ShootAtPlayer()
    {
        if (fireTimer > 0) return;
        if (firePoint == null || projectilePrefab == null) return;

        Vector2 direction = (player.position - firePoint.position).normalized;

        GameObject bullet = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);

        wolfpowers proj = bullet.GetComponent<wolfpowers>();
        if (proj != null)
        {
            proj.SetDirection(direction);

            // decide whether this shot heals or hurts
            bool willHeal = Random.value < healChance;
            proj.healsPlayer = willHeal;

            if (willHeal)
            {
                proj.healAmount = healAmount;
            }
            else
            {
                proj.damage = attackAmount;
            }
        }

        fireTimer = fireCooldown;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
    }
}
