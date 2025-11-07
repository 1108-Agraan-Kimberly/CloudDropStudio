using UnityEngine;
using Unity.Cinemachine;

public class map_transition : MonoBehaviour
{
    [SerializeField] PolygonCollider2D mapBoundary;
    CinemachineConfiner2D confiner;
    [SerializeField] Direction direction;

    enum Direction { Up, Down, Left, Right }
    private void Awake()
    {
        confiner = FindObjectOfType<CinemachineConfiner2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            confiner.BoundingShape2D = mapBoundary;
            UpdatePlayerPosition(collision.gameObject);
        }
    }

    private void UpdatePlayerPosition(GameObject player)
    {
        Vector3 newPosition = player.transform.position;
        switch (direction)
        {
            case Direction.Up:
                newPosition.y += 20f;
                break;
            case Direction.Down:
                newPosition.y -= 20f;
                break;
            case Direction.Left:
                newPosition.x -= 20f;
                break;
            case Direction.Right:
                newPosition.x += 20f;
                break;
        }
        player.transform.position = newPosition;
    }
}
