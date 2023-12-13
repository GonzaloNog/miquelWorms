using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WormHealth : MonoBehaviour
{

    public int health;
    public int maxHealth = 100;

    [SerializeField]
    private Text healthTxt;

    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        healthTxt.text = health.ToString();
    }

    public void ChangeHealth(int change)
    {
        health += change;

        if (health > maxHealth)
            health = maxHealth;
        else if (health <= 0)
            health = 0;

        healthTxt.text = health.ToString();

    }
}