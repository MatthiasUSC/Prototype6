using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bubbleControl1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the collision is with an object tagged "Player 2"
        if (collision.gameObject.CompareTag("Player 2"))
        {
            // Destroy the projectile
            Destroy(gameObject);
        }
    }
}
