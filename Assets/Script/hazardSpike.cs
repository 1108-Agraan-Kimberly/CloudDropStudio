using UnityEngine;

public class hazardSpike : MonoBehaviour
{
    [Tooltip("Damage applied when spikes hit the player (use positive value).")]
    public int damage = 1;

    [Tooltip("Seconds the player must stand on the trap before it deals damage.")]
    public float timeToHurt = 0.5f;

    [Tooltip("Seconds the trap waits after dealing damage before it can deal damage again.")]
    public float rearmDelay = 1.0f;

    bool playerOn = false;
    float stayTimer = 0f;
    float rearmTimer = 0f;
    bool ready = true;

    Health targetHealth = null;

    void Update()
    {
        if (!ready)
        {
            if (rearmTimer > 0f)
            {
                rearmTimer -= Time.deltaTime;
                if (rearmTimer <= 0f)
                {
                    ready = true;
                    stayTimer = 0f;
                }
            }

            return;
        }

        if (playerOn && targetHealth != null)
        {
            stayTimer += Time.deltaTime;
            if (stayTimer >= timeToHurt)
            {
                DealDamage();
                ready = false;
                rearmTimer = rearmDelay;
            }
        }
    }

    void DealDamage()
    {
        if (targetHealth != null)
        {
            targetHealth.healthState(-damage);
            // optional: play animation or sound here
        }
    }

    void BeginContact(GameObject other)
    {
        if (!other) return;
        if (other.CompareTag("Player"))
        {
            playerOn = true;
            stayTimer = 0f;
            targetHealth = other.GetComponent<Health>();
        }
    }

    void EndContact(GameObject other)
    {
        if (!other) return;
        if (other.CompareTag("Player"))
        {
            playerOn = false;
            stayTimer = 0f;
            targetHealth = null;
        }
    }

    // 3D trigger
    void OnTriggerEnter(Collider other)
    {
        BeginContact(other.gameObject);
    }

    void OnTriggerExit(Collider other)
    {
        EndContact(other.gameObject);
    }

    // 2D trigger
    void OnTriggerEnter2D(Collider2D other)
    {
        BeginContact(other.gameObject);
    }

    void OnTriggerExit2D(Collider2D other)
    {
        EndContact(other.gameObject);
    }
}
