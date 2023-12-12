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

    private Animator anim;

    public int wormID;

    private SpriteRenderer spriteRenderer;

    private Camera mainCam;

    public bool isTurn{ get { return WormManager.instance.IsMyTurn(wormID); } }
    //WormHealth wormHealth;

    private Vector3 diff;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        mainCam = Camera.main;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isTurn)
            return;

        RotateGun();

        MovementAndShooting();

        SetAnimationState();
    }

    void RotateGun() {
        diff = mainCam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        diff.Normalize();

        float rot_Z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

        currentGun.rotation = Quaternion.Euler(0, 0, rot_Z + 180f);
    }

    void MovementAndShooting() {

        float hor = Input.GetAxis("Horizontal");

        if (hor == 0)
        {
            currentGun.gameObject.SetActive(true);

            if (Input.GetKeyDown(KeyCode.Q))
            {
                Rigidbody2D bullet = Instantiate(bulletPrefab,
                    currentGun.position - currentGun.right,
                    currentGun.rotation);

                bullet.AddForce(-currentGun.right * missileForce, ForceMode2D.Impulse);

                if (isTurn)
                {
                    WormManager.instance.NextWorm();
                }
            }
          

        }
        else
        {
            currentGun.gameObject.SetActive(false);
            

            transform.position += Vector3.right * hor * Time.deltaTime * walkSpeed;

            spriteRenderer.flipX = Input.GetAxis("Horizontal") > 0;
            
        }
    }

    void SetAnimationState()
    {
        float hor = Input.GetAxis("Horizontal");

        Debug.Log("Horizontal input: " + hor);

        if (hor == 0)
        {
            anim.SetBool("IsWalking", false);
        }
        else
        {
            anim.SetBool("IsWalking", true);
            Vector3 headPosition = currentGun.position;
            headPosition.x += 0.1f; // Puedes ajustar este valor según tus necesidades
        }

        Debug.Log("IsWalking: " + anim.GetBool("IsWalking"));
    }

    


}
