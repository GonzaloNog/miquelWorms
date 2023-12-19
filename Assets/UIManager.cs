using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private float time;
    public TextMeshProUGUI timeTurn;
    public TextMeshProUGUI lifeTeam1;
    public TextMeshProUGUI lifeTeam2;
    void Start()
    {
        time = LevelManager.instance.timeTurn;
    }
    void Update()
    {
        time -= Time.deltaTime;
        timeTurn.text = Mathf.RoundToInt(time).ToString();
        if(time <= 0)
        {
            newTurn();
            LevelManager.instance.GetCombatManager().TurnChangeDead();
        }

    }
    public void newTurn()
    {
        time = LevelManager.instance.timeTurn;
    }
    public void newTeamLife(int life, int teamID)
    {
        switch (teamID)
        {
            case 1: lifeTeam1.text = life.ToString(); break;
            case 2: lifeTeam2.text = life.ToString(); break;
        }
    }
}
