using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControler : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D bulletPrefab;

    [SerializeField]
    private Transform currentGun;

    public float walkSpeed = 1.0f;
    public float maxRelativeVelocity = 6.0f;
    public float missileForce = 5.0f;

    public int wormID;

    private SpriteRenderer spriteRenderer;

    private Camera mainCam;

    //public bool isTurn{}
    //WormHealth wormHealth;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
