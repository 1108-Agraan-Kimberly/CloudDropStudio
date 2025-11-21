using System.Collections;   
using System.Collections.Generic;
using UnityEngine;

public class powerUp : MonoBehaviour
{
    public PowerUpEffect effect;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            effect.Apply(other.gameObject);
            Destroy(gameObject);
        }
    }
}
