using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    private float destroySpeed = 1f;
    public GameObject originPlayer;
    
    Vector2 perpDir;
    public float curveForce = 10f;
    // Start is called before the first frame update
    void Start()
    {
        Vector2 initVel = GetComponent<Rigidbody2D>().velocity.normalized;
        perpDir = new Vector2(initVel.y, -initVel.x);

        if(originPlayer.GetComponent<TraitList>().hasTrait("xraybullet")){
            LayerMask mask = LayerMask.GetMask("Wall");
            GetComponent<Collider2D>().excludeLayers = mask;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(GetComponent<Rigidbody2D>().velocity.magnitude <= destroySpeed){
            Destroy(this.gameObject);
        }

        if(originPlayer.GetComponent<TraitList>().hasTrait("curveleft")){
            GetComponent<Rigidbody2D>().AddForce(-perpDir * curveForce);
        }   

        if(originPlayer.GetComponent<TraitList>().hasTrait("curveright")){
            GetComponent<Rigidbody2D>().AddForce(perpDir * curveForce);
        }
        


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player"))
        {
            // if(collision.gameObject.CompareTag("Bullet_p1") || collision.gameObject.CompareTag("Bullet_p2")){
            //     if (collision.gameObject.tag == "Robot") {
            //         Physics2D.IgnoreCollision(collision.collider, GetComponent<Collider2D>());
            //     }

            //     if(GetComponent<Rigidbody2D>().velocity.magnitude <= collision.gameObject.GetComponent<Rigidbody2D>().velocity.magnitude){
            //         Destroy(this.gameObject);
            //     }
            // } else {
            Destroy(this.gameObject);
            // }
            
        }
    }

}
