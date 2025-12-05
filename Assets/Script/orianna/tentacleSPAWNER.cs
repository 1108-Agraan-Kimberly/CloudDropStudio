using System.Collections;
using UnityEngine;

public class tentacleSPAWNER : MonoBehaviour
{
    [SerializeField] private GameObject tentaclePrefab;
    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private float spawnRadius = 2f;

    private Transform player;
    private bool inRange = false;
    private Coroutine spawnCoroutine;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }

    private IEnumerator SpawnTentacles()
    {
        while (inRange)
        { 
            yield return new WaitForSeconds(spawnInterval);
            Vector2 place = player.GetComponent<Rigidbody2D>().linearVelocity.normalized;

            if(place == Vector2.zero)
            {
                place = Vector2.down;

            }
            Vector3 spawnOrigin = player.position + (Vector3)place * spawnRadius;
            Instantiate(tentaclePrefab, spawnOrigin, Quaternion.identity);
           
        }
    }

}

