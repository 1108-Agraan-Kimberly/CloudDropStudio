using System.Collections;
using UnityEngine;

public class wolfie_movement : MonoBehaviour
{
    public Transform waypointsParent;
    public float movespeed = 2f;
    public float waitTime = 2f;
    public bool loopWaypoints = true;

    private Transform[] waypoints;
    private int currentPoint;
    private bool isWaiting;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        waypoints = new Transform[waypointsParent.childCount];
        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = waypointsParent.GetChild(i);
        }
    }

    void FixedUpdate()
    {
        MoveToPoint();
    }

    void MoveToPoint()
    {
        if (isWaiting)
        {
            rb.linearVelocity = Vector2.zero;
            return;
        }

        Transform target = waypoints[currentPoint];
        Vector2 direction = ((Vector2)target.position - rb.position).normalized;

        rb.linearVelocity = direction * movespeed;

        if (Vector2.Distance(rb.position, target.position) < 0.1f)
        {
            StartCoroutine(WaitAtPoint());
        }
    }

    IEnumerator WaitAtPoint()
    {
        isWaiting = true;
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(waitTime);

        currentPoint = loopWaypoints ?
            (currentPoint + 1) % waypoints.Length :
            Mathf.Min(currentPoint + 1, waypoints.Length - 1);

        isWaiting = false;
    }
}
