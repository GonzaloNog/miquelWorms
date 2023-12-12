using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormManager : MonoBehaviour
{

    public static WormManager instance;

    private WormManager[] worms;

    private Transform wormCamera;

    private int currentWorm;

    private void Awake()
    {
        if (instance != null)
                Destroy(this);
        else
            instance = this;
    }

    public bool IsMyTurn(int i)
    {
        return i == currentWorm;
    }

    public void NextWorm()
    {

    }

}
