using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerControler : MonoBehaviour
{
    [SerializeField]
    private Rigidbody2D bulletPrefab;

    [SerializeField]
    private Transform currentGun;

    public float walkSpeed = 1.0f;
    private bool moveLeft = false;
    private bool moveRight = false;
    public float maxRelativeVelocity = 6.0f;
    public float missileForce = 15.0f;
    public int life = 100;
    private float missileForceShoot = 0.0f;
    private bool inmune = false;
    private Button bulletButton;
    private Button leftButton;
    private Button rightButton;
    private Button upButton;
    private Button downButton;
    private Button fingerButton;

    private string state = "null";
    public string name = "null";

    private Animator anim;

    public int wormID;
    public int teamID;

    private SpriteRenderer spriteRenderer;

    private Camera mainCam;

    public bool isTurn = false;
    public bool isLive = true;
    private WormHealth wormHealth;
    private bool isFingerCoroutineRunning = false;
    private bool isBulletDown = false;


    private Vector3 diff;

    //IA
    public bool IA = false;
    private bool iaIsPlay = false;
    private int hor2 = 0;
    private float timeWalk;
    private float apuntar;
    public float[] minShotIA;
    public float[] minDistanceIA;

    //UI
    public Slider shootUI;

    //fisica 
    Rigidbody2D rb;
    public float fuerzaDeImpulso = 10f;
    public GameObject fireEfectGO;
    private bool shootAnim = false;



    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        wormHealth = GetComponent<WormHealth>();
        mainCam = Camera.main;
        anim = GetComponent<Animator>();
        wormHealth.newName(name);
        shootUI.gameObject.SetActive(false);
        shootUI.maxValue = missileForce;
        wormHealth.setHealth(life);
        setColorTeam();

        GameObject bulletObject = GameObject.Find("bulletButton");
        GameObject leftObject = GameObject.Find("leftButton");
        GameObject rightObject = GameObject.Find("rightButton");
        GameObject upObject = GameObject.Find("upButton");
        GameObject downObject = GameObject.Find("downButton");
        GameObject fingerObject = GameObject.Find("fingerButton");

        /*
        bulletButton = bulletObject.GetComponent<Button>();
        leftButton = leftObject.GetComponent<Button>();
        rightButton = rightObject.GetComponent<Button>();
        upButton = upObject.GetComponent<Button>();
        downButton = downObject.GetComponent<Button>();
        fingerButton = fingerObject.GetComponent<Button>();
        */


        if (bulletObject != null)
        {
            bulletButton = bulletObject.GetComponent<Button>();

            if (bulletButton != null)
            {
                bulletButton.onClick.AddListener(buttonClick);
            }
            else
            {
                Debug.LogError("El objeto encontrado no tiene un componente Button.");
            }
        }
        


    }

    // Update is called once per frame
    void Update()
    {

        bool originalFlipX = spriteRenderer.flipX;
        //Debug.Log("FLIPX"+originalFlipX);

       // Debug.Log(rb.velocity);
        if (rb.velocity.x > 2f || rb.velocity.x < -2f || rb.velocity.y < -2f || rb.velocity.y > 2f)
        {
            if(!shootAnim)
                currentGun.gameObject.SetActive(false);
            anim.SetBool("isHit", true);
        }
        else
            anim.SetBool("isHit", false);
        if (isTurn)
        {
            if (isLive)
            {
                //Debug.Log(mainCam.ScreenToWorldPoint(Input.mousePosition));
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
                            fireEfectGO.SetActive(true);
                            shootAnim = true;
                            StartCoroutine(fireEfect());
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
            {
                isLive = false;
                LevelManager.instance.GetCombatManager().TurnChangeDead();

            }    
        }
        else if(!shootAnim)
            currentGun.gameObject.SetActive(false);
    }

    void RotateGun() {

        diff = mainCam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        diff.Normalize();

        float rot_Z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

        currentGun.rotation = Quaternion.Euler(0, 0, rot_Z + 180f);
    }

    void MovementAndShooting() {

        float hor = walkSpeed;

        if (moveLeft)
        {
            hor = -walkSpeed;   
        }
        else if (moveRight)
        {
            hor = walkSpeed;
        }
        else
        {
            hor = 0; 
        }


        if (hor == 0 && rb.velocity.x == 0 && rb.velocity.y == 0 && isFingerCoroutineRunning == false)
        {
            currentGun.gameObject.SetActive(true);
            //if (Input.GetKey(KeyCode.Q))
            if (Input.GetButton("bulletButton"))
            {
                missileForceShoot += Time.deltaTime * 8;
                if (missileForceShoot >= missileForce)
                    missileForceShoot = 0;
                //Debug.Log(missileForceShoot);
                shootUI.gameObject.SetActive(true);
                shootUI.value = missileForceShoot;
            }
            if (Input.GetButtonUp("bulletButton"))
            {
                fireEfectGO.SetActive(true);
                shootAnim = true;
                StartCoroutine(fireEfect());
                shootUI.gameObject.SetActive(false);
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
            if(rb.velocity.x == 0 && rb.velocity.y == 0)
            {
                if(!shootAnim)
                    currentGun.gameObject.SetActive(false);


                transform.position += Vector3.right * hor * Time.deltaTime * walkSpeed;

                spriteRenderer.flipX = Input.GetAxis("Horizontal") > 0;
            }
        }
    }

    void SetAnimationState()
    {
        if (moveLeft || moveRight)
        {
            anim.SetBool("IsWalking", true);
            Vector3 headPosition = currentGun.position;
            headPosition.x += 0.1f; // Puedes ajustar este valor según tus necesidades
            if (moveRight)
            {
                spriteRenderer.flipX = true;
            }
            else
            {
                spriteRenderer.flipX = false;
            }
        }
        else
        {
            StartCoroutine(finger());
        }


        //Debug.Log("Horizontal input: " + hor);


        //Debug.Log("IsWalking: " + anim.GetBool("IsWalking"));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            if (!inmune)
            {
                life -= LevelManager.instance.weaponHurt;
                wormHealth.ChangeHealth(-LevelManager.instance.weaponHurt);
                if (life <= 0)
                {
                    anim.SetBool("isDead", true);
                    isLive = false;
                }
                LevelManager.instance.GetCombatManager().ChangeLife(teamID);

                Vector2 bulletPosition = collision.transform.position;
                Vector2 playerPosition = transform.position;

                // Calcular la dirección entre la bala y el jugador
                Vector2 direction = (playerPosition - bulletPosition).normalized;

                // Aplicar fuerza al jugador en la dirección calculada
                rb.AddForce(direction * fuerzaDeImpulso, ForceMode2D.Impulse);

                if (isTurn)
                    WormManager.instance.NextWorm();
            }
        }
        else if(collision.gameObject.tag == "deadZone")
        {
            life = 0;
            wormHealth.ChangeHealth(-LevelManager.instance.weaponHurt);
            if (life <= 0)
            {
                anim.SetBool("isDead", true);
                isLive = false;
            }
            LevelManager.instance.GetCombatManager().ChangeLife(teamID);
            if (isTurn)
                WormManager.instance.NextWorm();
        }
    }
    IEnumerator newInmune()
    {
        inmune = true;
        yield return new WaitForSeconds(0.2f);
        inmune = false;
    }

    IEnumerator finger()
    {
        
        if (Input.GetKey("w"))
        {
            isFingerCoroutineRunning = true;
            currentGun.gameObject.SetActive(false);
            anim.SetBool("isFinger", true);
        
            yield return new WaitForSeconds(1f);
            anim.SetBool("isFinger", false);
            isFingerCoroutineRunning = false;
        }
        else
        {
            anim.SetBool("isFinger", false);
            anim.SetBool("IsWalking", false);
        }
    }

    IEnumerator iaPlayer()
    {
        state = "move";
        yield return new WaitForSeconds(timeWalk);
        anim.SetBool("IsWalking", false);
        //Debug.Log(LevelManager.instance.GetCombatManager().TargetShoot(this.gameObject, teamID).name);
        if (LevelManager.instance.GetCombatManager().TargetShoot(this.gameObject, teamID).transform.position.x < this.transform.position.x)
        {
            apuntar = Random.Range(-70f, 0f);
            spriteRenderer.flipX = false;
        }
        else
            apuntar = Random.Range(-180f, -120f);
        state = "apuntar";
        yield return new WaitForSeconds(1f);
        if (Vector3.Distance(LevelManager.instance.GetCombatManager().TargetShoot(this.gameObject, teamID).transform.position, this.transform.position) < minDistanceIA[0])
            missileForceShoot = Random.Range(minShotIA[0], missileForce);
        else if (Vector3.Distance(LevelManager.instance.GetCombatManager().TargetShoot(this.gameObject, teamID).transform.position, this.transform.position) < minDistanceIA[1])
            missileForceShoot = Random.Range(minShotIA[1], missileForce);
        else if (Vector3.Distance(LevelManager.instance.GetCombatManager().TargetShoot(this.gameObject, teamID).transform.position, this.transform.position) < minDistanceIA[2])
            missileForceShoot = Random.Range(minShotIA[2], missileForce);
        else
            missileForceShoot = Random.Range(missileForce/1.3f, missileForce);
        //Debug.Log(LevelManager.instance.GetCombatManager().TargetShoot(this.gameObject, teamID).name + " Distacia: " + Vector3.Distance(LevelManager.instance.GetCombatManager().TargetShoot(this.gameObject, teamID).transform.position, this.transform.position) + " Fuerza: " + missileForceShoot);
        state = "shoot";
    }
    IEnumerator fireEfect()
    {
        yield return new WaitForSeconds(0.5f);
        fireEfectGO.SetActive(false);
        shootAnim = false;
    }
    public void setColorTeam()
    {
        wormHealth.setColor(teamID);
    }

    public void pointerDownLeft()
    {
        moveLeft = true;
    }

    public void pointerUpLeft()
    {
        moveLeft = false;
    }

    public void pointerDownRight()
    {
        moveRight = true;
    }

    public void pointerUpRight()
    {
        moveRight = false;
    }

    void buttonClick()
    {
        Debug.Log("shooooooting");
    }

    void OnBulletDown()
    {
        isBulletDown = true;
    }

    void OnBulletUp()
    {
        isBulletDown = false;
    }
}
