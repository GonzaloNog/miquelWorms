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
        if (health > 60)
            healthTxt.color = Color.green;
        else if (health > 30)
            healthTxt.color = Color.yellow;
        else
            healthTxt.color = Color.red;

    }
    public void newName(string _name)
    {
        name.text = _name;
    }
    public void setHealth(int _health)
    {
        maxHealth = _health;
        health = maxHealth;
        healthTxt.text = health.ToString();
    }
    public void setColor(int teamID)
    {
        if(teamID == 1)
        {
            name.color = Color.blue;
        }
        else
        {
            name.color = Color.red;
        }
    }
}