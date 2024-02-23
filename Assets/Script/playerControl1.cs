using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControl1 : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float projectileSpeed;
    private double nextFireTime;
    public static double fireCooldown = 2;

    [SerializeField] private GameObject pointer;
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private GameObject cooldownIndicator;

    // Audio variables
    [SerializeField] private AudioClip projectileSound;
    private AudioSource audioSource;

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
        // Calculate movement based on WASD keys
        float horizontalInput = 0f;
        float verticalInput = 0f;

        if (Input.GetKey(KeyCode.W))
        {
            verticalInput = 1f;
        }
        else if (Input.GetKey(KeyCode.S))
        {
            verticalInput = -1f;
        }

        if (Input.GetKey(KeyCode.A))
        {
            horizontalInput = -1f;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            horizontalInput = 1f;
        }

        Vector3 movement = new Vector3(horizontalInput, verticalInput, 0f) * moveSpeed * Time.deltaTime;

        // Move the GameObject
        transform.Translate(movement);

        // Rotate the pointer around the circle sprite
        if (Input.GetKey(KeyCode.K))
        {
            pointer.transform.RotateAround(transform.position, Vector3.forward, -rotationSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.H))
        {
            pointer.transform.RotateAround(transform.position, Vector3.forward, rotationSpeed * Time.deltaTime);
        }

        // Fire projectile if cooldown has passed
        if (Input.GetKeyDown(KeyCode.J) && Time.time >= nextFireTime)
        {
            FireProjectile();
            nextFireTime = Time.time + fireCooldown; // Set next fire time

            // Play the projectile sound
            audioSource.PlayOneShot(projectileSound);

            // Reset the cooldown indicator scale
            StartCoroutine(ResetCooldownIndicator());
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

        // Calculate direction towards pointer
        Vector3 direction = (pointer.transform.position - transform.position).normalized;

        // Apply force to the projectile in the direction of the pointer
        projectile.GetComponent<Rigidbody2D>().velocity = direction * projectileSpeed;
    }
}
