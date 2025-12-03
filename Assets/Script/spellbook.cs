using UnityEngine;

public class spellbook : MonoBehaviour
{
    public bool collected;

    private void Start()
    {
        collected = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !collected)
        {
            Debug.Log("Spellbook collected!");
            collected = true;
            Destroy(gameObject);
        }
    
    }
}
