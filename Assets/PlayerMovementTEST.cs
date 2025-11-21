using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour
{
    public float baseSpeed = 5f;
    private float currentSpeed;

    void Start()
    {
        currentSpeed = baseSpeed;
    }

    void Update()
    {
        // Basic movement logic using currentSpeed
        transform.Translate(Vector3.right * currentSpeed * Time.deltaTime);
    }

    // Coroutine to manage the power-up duration
    public void ApplySpeedBoost(float multiplier, float duration)
    {
        StartCoroutine(SpeedBoostRoutine(multiplier, duration));
    }

    IEnumerator SpeedBoostRoutine(float multiplier, float duration)
    {
        currentSpeed *= multiplier; // Apply the effect
        yield return new WaitForSeconds(duration); // Wait for the duration
        currentSpeed = baseSpeed; // Revert the effect
    }
}

