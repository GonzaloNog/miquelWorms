using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CombatManager : MonoBehaviour
{
    public int idTurn = 0;
    private GameObject target;
    private GameObject targetCam;

    public PlayerControler[] worms;
    public int[] wormsTems;
    private void Awake()
    {
        for(int a = 0; a < worms.Length; a++)
        {
            worms[a].name = "Player " + (a + 1);
            worms[a].teamID = wormsTems[a];
        }
        worms[idTurn].isTurn = true;
        targetCam = worms[idTurn].gameObject;
    }
    private void Start()
    {
        for (int a = 0; a < wormsTems.Length; a++)
        {
            ChangeLife(wormsTems[a]);
        }
    }

    public IEnumerator ChangeTurn(GameObject shoot)
    {
        targetCam = shoot;
        yield return new WaitForSeconds(2f);
        LevelManager.instance.Ui.newTurn();
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
    public void TurnChangeDead()
    {
        LevelManager.instance.Ui.newTurn();
        idTurn++;
        if (idTurn == worms.Length)
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
    public GameObject TargetShoot(GameObject _target, int _teamID)
    {
        GameObject target = null;
        float distancia = 10000f;
        for (int a = 0; a < worms.Length; a++)
        {
            if (_teamID != worms[a].teamID)
            {
                //Debug.Log(worms[a].gameObject.name + " Distancia: " + Vector3.Distance(_target.transform.position, worms[a].transform.position));
                if(Vector3.Distance(_target.transform.position, worms[a].transform.position) < distancia)
                {
                    distancia = Vector3.Distance(_target.transform.position, worms[a].transform.position);
                    target = worms[a].gameObject;
                }
            }
        }
        return target;
    }
    public void ChangeLife(int teamId)
    {
        int life = 0;
        for(int a = 0; a < worms.Length; a++)
        {
            if (worms[a].teamID == teamId)
            {
                life += worms[a].life;
            }
        }
        LevelManager.instance.Ui.newTeamLife(life,teamId);
    }
}
