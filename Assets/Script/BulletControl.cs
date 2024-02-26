using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletControl : MonoBehaviour
{
    private float destroySpeed = 1f;

    private float homingForce = 10f;
    public GameObject originPlayer;

    private GameObject otherPlayer;
    
    Vector2 perpDir;
    public float curveForce = 10f;
    public bool friendlyFire = false; //bouncy trait
    public float firedTime;
    
    // Start is called before the first frame update
    void Start()
    {
        if(originPlayer.name == "Player1"){
            otherPlayer = GameObject.Find("Player2");
        } else {
            otherPlayer = GameObject.Find("Player1");
        }

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
        
        if(originPlayer.GetComponent<TraitList>().hasTrait("homingbullet")){
            Vector2 delta = (Vector2)(otherPlayer.transform.position - transform.position);
            GetComponent<Rigidbody2D>().AddForce(delta.normalized * homingForce);
        }


    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (!collision.gameObject.CompareTag("Player") && !friendlyFire)
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
