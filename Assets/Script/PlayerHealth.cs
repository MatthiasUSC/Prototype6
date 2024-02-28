using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public GameObject healthPrefab; // Assign in inspector
    public int maxHealth = 3;

    //arrangement for heart icons
    private float width = 1.2f;
    private float height = 5.8f;
    private float startPosition;

    private int currentHealth;
    private GameObject[] healthIcons;
    public TextMeshProUGUI WinUI;

    void Start()
    {
        //traits for high and low hp
        if (GetComponent<TraitList>().hasTrait("highhp") && !GetComponent<TraitList>().hasTrait("lowhp"))
        {
            maxHealth = 5;
        }
        if (GetComponent<TraitList>().hasTrait("lowhp") && !GetComponent<TraitList>().hasTrait("highhp"))
        {
            maxHealth = 1;
        }

        //heart UI start position for two players
        if (GetComponent<PlayerControl>().playerIndex == 1) {
            startPosition = -10f;
        }
        else
        {
            startPosition = 5.5f;
        }
        currentHealth = maxHealth;
        healthIcons = new GameObject[maxHealth];

        for (int i = 0; i < maxHealth; i++)
        {
            GameObject healthIcon = Instantiate(healthPrefab, new Vector3(startPosition + i * width, height, 0), Quaternion.identity);
            healthIcons[i] = healthIcon;
        }
    }

    public void TakeDamage(int damage)
    {
        Debug.Log("Damage is " + damage);
        currentHealth -= damage;
        if (currentHealth < 0) currentHealth = 0;

        if(GetComponent<TraitList>().hasTrait("teleporthit")){
            float horzExtent = Camera.main.orthographicSize * Screen.width / Screen.height;
            float x = Random.Range(-horzExtent, horzExtent);
            float y = Random.Range(-Camera.main.orthographicSize, Camera.main.orthographicSize);
            transform.position = new Vector3(x, y, 0);
        }

        // Update health display
        for (int i = 0; i < healthIcons.Length; i++)
        {
            if (i < currentHealth)
                healthIcons[i].SetActive(true);
            else
                healthIcons[i].SetActive(false);
        }

        //check if game over
        if (currentHealth <= 0)
        {
            Debug.Log("suppose to dead");
            GetComponent<PlayerControl>().isDead = true;
            if (GetComponent<PlayerControl>().playerIndex == 1)
            {
                WinUI.text = "Green Team Wins";
            }
            else
            {
                WinUI.text = "Red Team Wins";
            }
            WinUI.enabled = true;
        }
    }

    // Example function to customize max health
    public void SetMaxHealth(int newMaxHealth)
    {
        // Destroy existing health icons
        foreach (GameObject icon in healthIcons)
        {
            Destroy(icon);
        }

        // Update maxHealth and reset currentHealth
        maxHealth = newMaxHealth;
        currentHealth = maxHealth; // Fully restore health; adjust this line if you prefer a different logic

        // Reinitialize the healthIcons array to the new size
        healthIcons = new GameObject[maxHealth];

        // Instantiate new health icons
        for (int i = 0; i < maxHealth; i++)
        {
            GameObject healthIcon = Instantiate(healthPrefab, new Vector3(i * 1.5f, 5, 0), Quaternion.identity);
            healthIcons[i] = healthIcon;
            healthIcon.transform.SetParent(this.transform); 
        }
    }
}

