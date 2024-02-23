using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BallCollision : MonoBehaviour
{
    public GameObject particleEffect;
    private SpriteRenderer sr;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            GameObject go = Instantiate(particleEffect);
            go.transform.position = transform.position;
            var temp = go.GetComponent<ParticleSystem>().main;
            temp.startColor = sr.color;
            Destroy(gameObject); // gameObject is the ball that the player is colliding with
        }
    }
}
