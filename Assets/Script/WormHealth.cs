using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WormHealth : MonoBehaviour
{

    public int health;
    public int maxHealth = 100;

    [SerializeField]
    private TextMeshProUGUI healthTxt;
    [SerializeField]
    private TextMeshProUGUI name;

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
    public void newName(string _name)
    {
        name.text = _name;
    }
}