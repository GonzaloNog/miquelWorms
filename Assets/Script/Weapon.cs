using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bullet;
    public Transform positionShoot;

    // Update is called once per frame
    void Update()
    {
       
    }

    public void shoot()
    {
        Instantiate(bullet,positionShoot.position,this.transform.rotation);
    }
}
