using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    // Hold down spacebar for boost in the direction player was pointing when pressed down, but also when boosting the rotation is stopped (and no decay happens) 
    public float force = 10;
    public float addedAngVel = 10;
    public GameObject boostEffect;

    public GameObject scoreCounter;
    int score = 0;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //GetComponent<Rigidbody2D>().AddForce(transform.up * (force / Time.deltaTime));
            GetComponent<Rigidbody2D>().velocity = (Vector2)transform.up * force;
            //GetComponent<Rigidbody2D>().AddTorque(torque / Time.deltaTime);
            GetComponent<Rigidbody2D>().angularVelocity += addedAngVel;
            boostEffect.GetComponent<ParticleSystem>().Play();
        }

        scoreCounter.GetComponent<TextMesh>().text = "Score: " + score.ToString();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.tag == "circle")
        {
            score += 1;
            // Destroy(other.gameObject);
        }
    }
}
