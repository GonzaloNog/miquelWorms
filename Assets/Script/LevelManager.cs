using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public CombatManager combat;
    public UIManager Ui;
    public float timeTurn = 10f;
    private void Awake()
    {
        if(instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    public CombatManager GetCombatManager()
    {
        return combat;
    }

}
