using System.Collections;
using UnityEngine;

public class wolfie_movement : MonoBehaviour
{
    public Transform waypointsParent;
    public float movespeed = 2f;
    public float waitTime = 2f;
    public bool loopWaypoints = true;
    public float arrivalThreshold = 0.05f;
    public bool debugLogs = false;

    private Vector2[] waypointPositions;
    private int currentPoint = 0;
    private bool isWaiting;

    void Start()
    {
        if (waypointsParent == null || waypointsParent.childCount == 0)
        {
            Debug.LogError("wolfie_movement needs a waypointsParent with at least one child.");
            enabled = false;
            return;
        }

        int count = waypointsParent.childCount;
        waypointPositions = new Vector2[count];
        for (int i = 0; i < count; i++)
            waypointPositions[i] = waypointsParent.GetChild(i).position;

        currentPoint = 0;
    }

    void Update()
    {
        if (isWaiting) return;
        MoveToPoint();
    }

    void MoveToPoint()
    {
        Vector2 currentPos = transform.position;
        Vector2 targetPos = waypointPositions[currentPoint];

        transform.position = Vector2.MoveTowards(currentPos, targetPos, movespeed * Time.deltaTime);

        if (Vector2.Distance(currentPos, targetPos) <= arrivalThreshold)
        {
            StartCoroutine(WaitAtPoint());
        }

        if (debugLogs)
            Debug.Log($"[wolfie_movement] moving to {targetPos} from {currentPos}");
    }

    IEnumerator WaitAtPoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);

        currentPoint = loopWaypoints
            ? (currentPoint + 1) % waypointPositions.Length
            : Mathf.Min(currentPoint + 1, waypointPositions.Length - 1);

        isWaiting = false;
    }
}
