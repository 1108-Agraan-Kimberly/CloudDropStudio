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

    void Start()
    {
        waypoints = new Transform[waypointsParent.childCount];

        for (int i = 0; i < waypoints.Length; i++)
        {
            waypoints[i] = waypointsParent.GetChild(i);
        }
    }

    void Update()
    {
        MoveToPoint();
    }

    void MoveToPoint()
    {
        if(isWaiting) return;
        Transform target = waypoints[currentPoint];

        transform.position = Vector2.MoveTowards(transform.position, target.position, movespeed * Time.deltaTime);

        if(Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            StartCoroutine(WaitAtPoint());
        }
    }

    IEnumerator WaitAtPoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);
        currentPoint = loopWaypoints ? (currentPoint + 1) % waypoints.Length : Mathf.Min(currentPoint + 1, waypoints.Length - 1);
        isWaiting = false;
    }

}
