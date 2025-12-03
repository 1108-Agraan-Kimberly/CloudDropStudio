using UnityEngine;
using UnityEngine.SceneManagement; 
using Unity.Cinemachine;

public class transitiontoTWO : MonoBehaviour
{
 
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
       
            ResetPlayerPosition(collision.gameObject);
            SceneManager.LoadSceneAsync(2);
        }
    }

    private void ResetPlayerPosition(GameObject player)
    {
        player.transform.position = playerStartPos;
    }
}
