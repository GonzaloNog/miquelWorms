using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public int idTurn = 0;
    private GameObject target;
    private GameObject targetCam;

    public PlayerControler[] worms;
    private void Awake()
    {
        for(int a = 0; a < worms.Length; a++)
        {
            worms[a].name = "Player " + (a + 1);
        }
        worms[idTurn].isTurn = true;
        targetCam = worms[idTurn].gameObject;
    }

    public IEnumerator ChangeTurn(GameObject shoot)
    {
        targetCam = shoot;
        yield return new WaitForSeconds(2f);
        idTurn++;
        if(idTurn == worms.Length)
            idTurn = 0;
        for (int a = 0; a < worms.Length; a++)
        {
            worms[a].isTurn = false;
        }
        worms[idTurn].isTurn = true;
        targetCam = worms[idTurn].gameObject;
    }
    public GameObject getTargetCam()
    {
        return targetCam;
    }
}
