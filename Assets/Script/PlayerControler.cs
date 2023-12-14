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
    public float missileForce = 15.0f;
    private float missileForceShoot = 0.0f;
    private bool inmune = false;

    private string state = "null";
    public string name = "null";

    private Animator anim;

    public int wormID;

    private SpriteRenderer spriteRenderer;

    private Camera mainCam;

    public bool isTurn = false;
    private WormHealth wormHealth;

    private Vector3 diff;

    //IA
    public bool IA = false;
    private bool iaIsPlay = false;
    private int hor2 = 0;
    private float timeWalk;
    private float apuntar;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        wormHealth = GetComponent<WormHealth>();
        mainCam = Camera.main;
        anim = GetComponent<Animator>();
        wormHealth.newName(name);
    }

    // Update is called once per frame
    void Update()
    {

        if (isTurn)
        {
            Debug.Log(mainCam.ScreenToWorldPoint(Input.mousePosition));
            if (!IA)
            {
                RotateGun();
                MovementAndShooting();
                SetAnimationState();
            }
            else
            {
                if (!iaIsPlay)
                {
                    timeWalk = Random.Range(0.5f, 3f);
                    StartCoroutine(iaPlayer());
                    iaIsPlay = true;
                    apuntar = Random.Range(-180f, 180f);
                    missileForceShoot = Random.Range(0f, missileForce);
                    int randomInt = Random.Range(1, 101);
                    if (randomInt > 50)
                        hor2 = 1;
                    else
                        hor2 = -1;
                }
                switch (state)
                {
                    case "move":
                        transform.position += Vector3.right * hor2 * Time.deltaTime * walkSpeed;
                        spriteRenderer.flipX = hor2 > 0;
                        if (hor2 == 0)
                        {
                            anim.SetBool("IsWalking", false);
                        }
                        else
                        {
                            anim.SetBool("IsWalking", true);
                            Vector3 headPosition = currentGun.position;
                            headPosition.x += 0.1f; // Puedes ajustar este valor según tus necesidades
                        }
                        break;
                    case "apuntar":
                        currentGun.gameObject.SetActive(true);
                        Vector3 currentRotation = currentGun.transform.eulerAngles;

                        currentRotation.z = apuntar;
                        currentGun.transform.eulerAngles = currentRotation;
                        break;
                    case "shoot":
                        StartCoroutine(newInmune());
                        Rigidbody2D bullet = Instantiate(bulletPrefab,
                            transform.position,
                            currentGun.rotation);

                        bullet.AddForce(-currentGun.right * missileForceShoot, ForceMode2D.Impulse);
                        missileForceShoot = 0;
                        if (isTurn)
                        {
                            StartCoroutine(LevelManager.instance.GetCombatManager().ChangeTurn(bullet.gameObject));
                            isTurn = false;
                            state = "null";
                            iaIsPlay = false;
                        }
                        break;
                }

            }
        }
        else
            currentGun.gameObject.SetActive(false);
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
            if (Input.GetKey(KeyCode.Q))
            {
                missileForceShoot += Time.deltaTime * 8;
                if (missileForceShoot >= missileForce)
                    missileForceShoot = 0;
                Debug.Log(missileForceShoot);
            }
            if (Input.GetKeyUp(KeyCode.Q))
            {
                StartCoroutine(newInmune());
                Rigidbody2D bullet = Instantiate(bulletPrefab,
                    transform.position,
                    currentGun.rotation);

                bullet.AddForce(-currentGun.right * missileForceShoot, ForceMode2D.Impulse);
                missileForceShoot = 0;
                if (isTurn)
                {
                    StartCoroutine(LevelManager.instance.GetCombatManager().ChangeTurn(bullet.gameObject));
                    isTurn = false;
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

        //Debug.Log("Horizontal input: " + hor);

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

        //Debug.Log("IsWalking: " + anim.GetBool("IsWalking"));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            if (!inmune)
            {
                wormHealth.ChangeHealth(-10);

                if (isTurn)
                    WormManager.instance.NextWorm();
            }
        }
    }
    IEnumerator newInmune()
    {
        inmune = true;
        yield return new WaitForSeconds(0.2f);
        inmune = false;
    }
    IEnumerator iaPlayer()
    {
        state = "move";
        yield return new WaitForSeconds(timeWalk);
        anim.SetBool("IsWalking", false);
        state = "apuntar";
        yield return new WaitForSeconds(1f);
        state = "shoot";
    }


}
