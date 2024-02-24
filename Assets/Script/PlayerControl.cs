using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public int playerIndex = 1;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float projectileSpeed;
    private double nextFireTime;
    public static double fireCooldown = 2;
    public bool isDead = false;

    [SerializeField] private GameObject pointer;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject cooldownIndicator;

    // Audio variables
    [SerializeField] private AudioClip projectileSound;
    private AudioSource audioSource;


    //Traits Control
    public int EnermyDamage = 1;

    // Start is called before the first frame update
    void Start()
    {
        nextFireTime = Time.time; // Initialize next fire time

        // Get the AudioSource component attached to the GameObject
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isDead)
        {
            Move();

            // Fire projectile if cooldown has passed
            if (Time.time >= nextFireTime)
            {
                if ((playerIndex == 1 && Input.GetButtonDown("Fire_p1")) || (playerIndex == 2 && Input.GetButtonDown("Fire_p2")))
                {
                    //Debug.Log("in fire");
                    FireProjectile();
                    nextFireTime = Time.time + fireCooldown; // Set next fire time

                    // Play the projectile sound
                    audioSource.PlayOneShot(projectileSound);

                    // Reset the cooldown indicator scale
                    StartCoroutine(ResetCooldownIndicator());
                }
            }
        }
        
        
    }

    /* Method name: Move
     * Description: for regular movements*/
    private void Move()
    {
        float horizontal, vertical, rotation;

        //distinguish two players and getting inputs
        if (playerIndex == 1)
        {
            horizontal = Input.GetAxis("Horizontal_p1");
            vertical = Input.GetAxis("Vertical_p1");
            rotation = Input.GetAxis("Rotation_p1");
        }
        else
        {
            horizontal = Input.GetAxis("Horizontal_p2");
            vertical = Input.GetAxis("Vertical_p2");
            rotation = Input.GetAxis("Rotation_p2");
        }

        Vector3 movement = new Vector3(horizontal, vertical, 0f) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);// Move the GameObject
        //Debug.Log(rotation);
        pointer.transform.RotateAround(transform.position, Vector3.forward, rotation * rotationSpeed * Time.deltaTime);//rotate
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

        // Calculate direction towards pointer
        Vector3 direction = (pointer.transform.position - transform.position).normalized;

        // Apply force to the projectile in the direction of the pointer
        projectile.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("is collided");
        if ((collision.gameObject.CompareTag("Bullet_p1") && playerIndex == 2) ||
            (collision.gameObject.CompareTag("Bullet_p2") && playerIndex == 1))
        {
            GetComponent<PlayerHealth>().TakeDamage(EnermyDamage);
            Destroy(collision.gameObject);
        }
    }
}
