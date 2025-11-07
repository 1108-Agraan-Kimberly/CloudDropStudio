using UnityEngine;
using UnityEngine.SceneManagement; 
using Unity.Cinemachine;

public class restart_exit : MonoBehaviour
{
    [SerializeField] PolygonCollider2D mapBoundary;
    CinemachineConfiner2D confiner;
    
    private Vector3 playerStartPos;

    private void Awake()
    {
        confiner = FindObjectOfType<CinemachineConfiner2D>();
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerStartPos = player.transform.position;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            confiner.BoundingShape2D = mapBoundary;
            ResetPlayerPosition(collision.gameObject);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void ResetPlayerPosition(GameObject player)
    {
        player.transform.position = playerStartPos;
        //player.GetComponent<Health>().ResetHealth();
    }
}
