using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public int playerIndex = 1;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float slowSpeed = 4f;
    [SerializeField] private float fastSpeed = 6f;
    [SerializeField] private float bigSize = 1.25f;
    [SerializeField] private float smallSize = 0.75f;
    [SerializeField] private float bigBulletSize = 0.65f;

    private float shakyAimStrength = 600f;

    private float jammedGunChance = 0.25f;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float projectileSpeed;
    [SerializeField] private float slowProjectileSpeed;
    private double nextFireTime;
    public static float fireCooldown = 1f;
    public bool isDead = false;

    [SerializeField] private GameObject pointer;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject cooldownIndicator;

    // Audio variables
    [SerializeField] private AudioClip projectileSound;
    public AudioSource audioManager;
    public ParticleSystem collisionEffect;
    public ParticleSystem oppo_collisionEffect;
    public AudioClip pop;
    private AudioSource audioSource;


    //Traits Control
    public int EnermyDamage = 1;

    // Start is called before the first frame update
    void Start()
    {
        nextFireTime = Time.time; // Initialize next fire time

        //initialize traits
        if (GetComponent<TraitList>().hasTrait("smallsize") && !GetComponent<TraitList>().hasTrait("bigsize"))
        {
            transform.localScale = new Vector3(smallSize, smallSize, smallSize);
        }
        if (GetComponent<TraitList>().hasTrait("bigsize") && !GetComponent<TraitList>().hasTrait("smallsize"))
        {
            transform.localScale = new Vector3(bigSize, bigSize, bigSize);
        }
        if (GetComponent<TraitList>().hasTrait("slowbullets"))
        {
            projectileSpeed = slowProjectileSpeed;
        }

        /*isSlowBullets = ? true : false;


        if (GetComponent<TraitList>().hasTrait("slowspeed")) { sizeLevel = 0; }
        else if (GetComponent<TraitList>().hasTrait("fastspeed")) { sizeLevel = 2; }*/

        // Get the AudioSource component attached to the GameObject
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void FixedUpdate(){
        if (!isDead){
            Move();
        }
    }

    IEnumerator fireBurst(){
        FireProjectile();
        yield return new WaitForSeconds(fireCooldown / 4f);
        FireProjectile();
        yield return new WaitForSeconds(fireCooldown / 4f);
        FireProjectile();
    }

    void Update()
    {
        if (!isDead){
            // Fire projectile if cooldown has passed
            if (Time.time >= nextFireTime)
            {
                if ((playerIndex == 1 && Input.GetButtonDown("Fire_p1")) || (playerIndex == 2 && Input.GetButtonDown("Fire_p2")))
                {
                    //Debug.Log("in fire");
                    bool canFire = true;
                    if(GetComponent<TraitList>().hasTrait("jammedgun")){
                        if(Random.Range(0.0f, 1.0f) < jammedGunChance){
                            canFire = false;
                        }
                    }

                    if(canFire){
                        if(GetComponent<TraitList>().hasTrait("multibullet")){
                            StartCoroutine(fireBurst());
                        } else {
                            FireProjectile();
                        }

                        // Play the projectile sound
                        audioSource.PlayOneShot(projectileSound);
                    }

                    nextFireTime = Time.time + fireCooldown; // Set next fire time
                    

                    // Reset the cooldown indicator scale
                    StartCoroutine(ResetCooldownIndicator());
                }
            }
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    /* Method name: Move
     * Description: for regular movements*/
    private void Move()
    {
        float horizontal, vertical, rotation, ifReverse;
        ifReverse = GetComponent<TraitList>().hasTrait("invertedcontrols") ? -1f : 1f;
        bool AutoRotation = GetComponent<TraitList>().hasTrait("noaim") ? true : false;

        //distinguish two players and getting inputs
        if (playerIndex == 1)
        {
            horizontal = ifReverse * Input.GetAxis("Horizontal_p1");
            vertical = ifReverse * -Input.GetAxis("Vertical_p1");
            rotation = -Input.GetAxis("Rotation_p1");
        }
        else
        {
            horizontal = ifReverse * Input.GetAxis("Horizontal_p2");
            vertical = ifReverse * -Input.GetAxis("Vertical_p2");
            rotation = -Input.GetAxis("Rotation_p2");
        }

        if (GetComponent<TraitList>().hasTrait("slowspeed") && !GetComponent<TraitList>().hasTrait("fastspeed"))
        {
            moveSpeed = slowSpeed;
        }else if (GetComponent<TraitList>().hasTrait("fastspeed") && !GetComponent<TraitList>().hasTrait("slowspeed"))
        {
            moveSpeed = fastSpeed;
        }

        //Perform Movement
        Vector2 movement = new Vector2(horizontal, vertical) * moveSpeed;
        GetComponent<Rigidbody2D>().velocity = movement;

        //Perform Rotation
        if (AutoRotation)
        {
            pointer.transform.RotateAround(transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);//auto rotate
        }
        else
        {
            pointer.transform.RotateAround(transform.position, Vector3.forward, rotation * rotationSpeed * Time.deltaTime);//rotate with input

            if(GetComponent<TraitList>().hasTrait("shakyaim")){
                float delta = Random.Range(-1.0f, 1.0f);
                pointer.transform.RotateAround(transform.position, Vector3.forward, delta * shakyAimStrength * Time.deltaTime);//rotate with input
            }
            
        }
        
    }

    IEnumerator ResetCooldownIndicator()
    {
        // Set the scale of the cooldown indicator to zero
        cooldownIndicator.transform.localScale = Vector3.zero;

        // Define the target scale
        Vector3 targetScale = new Vector3(0.9f, 0.9f, 1f);

        // Define the duration over which to scale up
        float duration = (float)fireCooldown;
        float currentTime = 0f;

        while (currentTime < duration)
        {
            // Incrementally scale up the cooldown indicator over time
            cooldownIndicator.transform.localScale = Vector3.Lerp(Vector3.zero, targetScale, currentTime / duration);

            // Update the current time
            currentTime += Time.deltaTime;

            // Wait for the next frame
            yield return null;
        }

        // Ensure the cooldown indicator reaches the target scale
        cooldownIndicator.transform.localScale = targetScale;
    }


    void FireProjectile()
    {
        // Instantiate projectile at the position of the pointer
        GameObject projectile = Instantiate(projectilePrefab, pointer.transform.position, Quaternion.identity);
        //set up for friendly fire trait
        projectile.GetComponent<BulletControl>().friendlyFire = GetComponent<TraitList>().hasTrait("bouncebullets") ? true : false;
        projectile.GetComponent<BulletControl>().firedTime = Time.time;
        projectile.GetComponent<BulletControl>().audioSource = audioManager;
        projectile.GetComponent<BulletControl>().collisionEffect = collisionEffect;
        if (GetComponent<TraitList>().hasTrait("bigbullets"))
        {
            projectile.transform.localScale = new Vector3(bigBulletSize, bigBulletSize, bigBulletSize);
        }
        // Calculate direction towards pointer
        Vector3 direction = (pointer.transform.position - transform.position).normalized;

        // Apply force to the projectile in the direction of the pointer
        projectile.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
    
        projectile.GetComponent<BulletControl>().originPlayer = this.gameObject;
        audioSource.clip = projectileSound;
        audioSource.Play();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        BulletControl bulletControl = collision.gameObject.GetComponent<BulletControl>();
        if (bulletControl != null && ((collision.gameObject.CompareTag("Bullet_p1") && playerIndex == 2) ||
            (collision.gameObject.CompareTag("Bullet_p2") && playerIndex == 1) ||
            (collision.gameObject.GetComponent<BulletControl>().friendlyFire && Time.time - collision.gameObject.GetComponent<BulletControl>().firedTime> 0.2f)))
        {
            GetComponent<PlayerHealth>().TakeDamage(EnermyDamage);

            audioSource.clip = pop;
            audioSource.Play();
            oppo_collisionEffect.transform.position = transform.position;
            oppo_collisionEffect.Play();
            Destroy(collision.gameObject);
        }
    }
}
