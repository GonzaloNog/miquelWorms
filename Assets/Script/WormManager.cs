using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormManager : MonoBehaviour
{

    public static WormManager instance;

    private PlayerControler[] worms;

    private Transform wormCamera;

    private int currentWorm;

    private void Awake()
    {
        if (instance != null)
                Destroy(this);
        else
            instance = this;
    }

    private void Start()
    {
        worms = GameObject.FindObjectsOfType<PlayerControler>();
        wormCamera = Camera.main.transform;

        for(int i = 0; i < worms.Length; i++)
        {
            worms[i].wormID = i;
        }

        NextWorm();

    }


    public bool IsMyTurn(int i)
    {
        return i == currentWorm;
    }

    public void NextWorm()
    {
        StartCoroutine(_NextWorm());
    }

    public IEnumerator _NextWorm()
    {
        int nextWorm = currentWorm + 1;
        currentWorm -= 1;

        yield return new WaitForSeconds(2f);

        currentWorm = nextWorm;

        if (currentWorm >= worms.Length)
            currentWorm = 0;

        wormCamera.SetParent(worms[currentWorm].transform);
        wormCamera.localPosition = Vector3.zero + Vector3.back * 10f;
    }

}
