using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class midrange_enemy : MonoBehaviour
{
    [Header("Detection / Movement")]
    public float detectionRange = 8f;
    public float stopDistance = 2.5f; // mid-range distance to stop approaching
    public float moveSpeed = 3f;

    [Header("Patrol")]
    public Transform pointA; // optional patrol point A
    public Transform pointB; // optional patrol point B
    public float patrolSpeed = 2f;
    public float arrivalThreshold = 0.1f;

    [Header("Attack")]
    public int damage = 1;
    public float attackCooldown = 1f;

    public bool debugLogs = false;

    Rigidbody2D rb;
    Transform player;
    float nextAttackTime = 0f;

    // internal patrol state
    Transform currentPatrolTarget;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        // try to find the player at start (may be null in some load orders)
        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null)
            player = p.transform;
        else if (debugLogs)
            Debug.LogWarning("[midrange_enemy] Player not found on Start; will try again in FixedUpdate.");

        // initialize patrol target
        if (pointA != null && pointB != null)
            currentPatrolTarget = pointB;
    }

    void FixedUpdate()
    {
        // ensure we always try to locate the player if not found yet
        if (player == null)
        {
            GameObject p = GameObject.FindGameObjectWithTag("Player");
            if (p != null)
                player = p.transform;
            else
            {
                // no player in scene yet -> continue with patrol if configured
                HandlePatrol();
                return;
            }
        }

        float dist = Vector2.Distance(rb.position, player.position);

        if (dist <= detectionRange)
        {
            // Move toward player until within stopDistance
            if (dist > stopDistance)
            {
                Vector2 dir = ((Vector2)player.position - rb.position).normalized;
                rb.linearVelocity = dir * moveSpeed;
            }
            else
            {
                // at mid-range - stop moving
                rb.linearVelocity = Vector2.zero;

                // try to deal mid-range damage on a cooldown
                TryDamage(player.gameObject);
            }
        }
        else
        {
            // outside detection range - patrol between points if configured
            if (pointA != null && pointB != null)
            {
                HandlePatrol();
            }
            else
            {
                rb.linearVelocity = Vector2.zero;
            }
        }

        if (debugLogs && player != null)
            Debug.Log($"[midrange_enemy] dist={Vector2.Distance(rb.position, player.position):F2} vel={rb.linearVelocity}");
    }

    void HandlePatrol()
    {
        if (pointA == null || pointB == null)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        if (currentPatrolTarget == null) currentPatrolTarget = pointB;

        Vector2 targetPos = currentPatrolTarget.position;
        float step = patrolSpeed * Time.fixedDeltaTime;

        // use MovePosition for physics-friendly movement
        Vector2 newPos = Vector2.MoveTowards(rb.position, targetPos, step);
        rb.MovePosition(newPos);

        if (Vector2.Distance(rb.position, targetPos) <= arrivalThreshold)
        {
            // switch target
            currentPatrolTarget = currentPatrolTarget == pointA ? pointB : pointA;
            rb.linearVelocity = Vector2.zero;
            return;
        }
    }

    // Try to damage when colliding or triggering with the player
    void TryDamage(GameObject other)
    {
        if (Time.time < nextAttackTime) return;

        if (other.CompareTag("Player"))
        {
            // Try to find Health on the object, its parents or children to be robust
            Health h = other.GetComponent<Health>();
            if (h == null) h = other.GetComponentInParent<Health>();
            if (h == null) h = other.GetComponentInChildren<Health>();

            if (h != null)
            {
                h.healthState(-damage);
                nextAttackTime = Time.time + attackCooldown;
                if (debugLogs) Debug.Log("[midrange_enemy] Dealt damage to player");
            }
            else if (debugLogs)
            {
                Debug.Log("[midrange_enemy] Player has no Health component on object/parents/children");
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision) => TryDamage(collision.gameObject);
    void OnTriggerEnter2D(Collider2D other) => TryDamage(other.gameObject);

    // also attempt damage while touching so mid-range continuous hits work
    void OnCollisionStay2D(Collision2D collision) => TryDamage(collision.gameObject);
    void OnTriggerStay2D(Collider2D other) => TryDamage(other.gameObject);

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRange);
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, stopDistance);

        if (pointA != null && pointB != null)
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawLine(pointA.position, pointB.position);
            Gizmos.DrawWireSphere(pointA.position, 0.1f);
            Gizmos.DrawWireSphere(pointB.position, 0.1f);
        }
    }
}