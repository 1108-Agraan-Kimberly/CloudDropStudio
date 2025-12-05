using System.Collections;
using UnityEngine;

public class tentacleSPAWNER : MonoBehaviour
{
    [SerializeField] private GameObject tentaclePrefab;
    [SerializeField] private float spawnInterval = 1f;
    [SerializeField] private float spawnRadius = 2f;

    private Transform player;
    private Coroutine spawnCoroutine;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        player = other.transform;

        if (tentaclePrefab == null)
        {
            Debug.LogWarning("tentacleSPAWNER: tentaclePrefab not assigned.");
            return;
        }

        if (spawnCoroutine == null)
            spawnCoroutine = StartCoroutine(SpawnTentacles());
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }

        player = null;
    }

    private IEnumerator SpawnTentacles()
    {
        while (player != null)
        {
            yield return new WaitForSeconds(spawnInterval);

            // Determine spawn direction from player's velocity if available
            Vector2 dir = Vector2.down;
            Rigidbody2D prb = player.GetComponent<Rigidbody2D>();
            if (prb != null)
            {
                Vector2 vel = prb.linearVelocity;
                if (vel.sqrMagnitude > 0.01f)
                    dir = vel.normalized;
            }

            Vector3 spawnOrigin = player.position + (Vector3)dir * spawnRadius;
            GameObject inst = Instantiate(tentaclePrefab, spawnOrigin, Quaternion.identity);

            // Ensure spawned tentacle is tagged as Enemy so melee detection finds it
            if (!string.IsNullOrEmpty(inst.tag) && inst.tag != "Untagged")
            {
                // keep existing tag
            }
            else
            {
                inst.tag = "Enemy";
            }
        }

        spawnCoroutine = null;
    }

    private void OnDisable()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
            spawnCoroutine = null;
        }
    }
}

