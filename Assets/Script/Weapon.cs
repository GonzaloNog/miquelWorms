using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public GameObject bullet;
    public Transform positionShoot;
    void Start()
    {
        StartCoroutine(salto());
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void shoot()
    {
        Instantiate(bullet,positionShoot.position,this.transform.rotation);
    }

    IEnumerator salto()
    {
        yield return new WaitForSeconds(1f);
        Debug.Log("hola");
        yield return new WaitForSeconds(5f);
        Debug.Log("chau");
    }
}
