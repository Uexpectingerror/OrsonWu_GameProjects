using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;


[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public int myPlayerID = 0;
    public int invertControls = 0;
    [HideInInspector] public Player player = null;


    public bool isUsingKeyboard = false;
    public int fireDis = 5;
    public GameObject curTile = null;
    public ColorState myPaintState = ColorState.Clean;


    [HideInInspector] public int tempLayer = 0;
    [HideInInspector] public int myColorCheck = 0;
    [HideInInspector] public float myColorLayer = 0;
    [HideInInspector] public int layerToTakeDamageFrom = 0;


    public bool canSwim = false;
    public bool swimming = false;
    [HideInInspector] public Health myHealth = null;
    [HideInInspector] public Fire myFire = null;
    [HideInInspector] public SuperAttackController mySuperController = null;
    [HideInInspector] public TowerManager myTowerManager = null;
    

    [SerializeField] private Animator animator = null;
    [SerializeField] private GameObject bodyToRotate = null;
    [SerializeField] private GameObject swimVFX = null;
    [SerializeField] private int towersOwned = 0;
    [SerializeField] private float moveSpeed = 4f;
    [SerializeField] private float normalSpeed = 0;
    [SerializeField] private float fastSpeed = 7f;
    [SerializeField] private float maxVelChange = 10f;
    [SerializeField] private float rotSpeed = 1f;
    [SerializeField] private float diveSpeed = 10f;

    [SerializeField] private AudioClip jumpInPaint = null;
    [SerializeField] private AudioClip jumpOutPaint = null;


    Rigidbody rb = null;
    Quaternion desiredDir;
    float inputDeadZone = 0.3f;
    float swimDepth = -1f;
    bool hold = false;
    GameObject gameManager = null;
    public RespawnManager respawnManager = null;
    public float respawnTime = 0f;
    public GameObject cam = null;
    public FadeInOut fadeInOut = null;
    private SFXManager mySFXManager = null;


    void Start()
    {
        player = ReInput.players.GetPlayer(myPlayerID);
        gameManager = GameObject.Find("GameManager");
        respawnManager = gameManager.GetComponent<RespawnManager>();
        rb = GetComponent<Rigidbody>();
        myHealth = GetComponent<Health>();
        mySFXManager = GetComponent<SFXManager>();
        myFire = GetComponentInChildren<Fire>();
        myFire.player = player;
        myFire.playerController = this;
        myFire.myPlayerID = myPlayerID;
        mySuperController = GetComponentInChildren<SuperAttackController>();
        mySuperController.myPlayerController = this;
        myTowerManager = gameManager.GetComponent<TowerManager>();
        mySuperController.myPlayerID = myPlayerID;
        fadeInOut = cam.GetComponent<FadeInOut>();
        

        normalSpeed = moveSpeed;

        //num 00
        tempLayer = (gameObject.layer + 2);
        //bitshifted layer number 0000
        myColorCheck = 1 << tempLayer;
        //layer 00
        myColorLayer = Mathf.Log(myColorCheck, 2);
        
        print(gameObject.name + " " + myColorLayer);


        switch (myColorLayer)
        {
            case 8:
                layerToTakeDamageFrom = 11;
                break;
            case 9:
                layerToTakeDamageFrom = 10;
                break;
        }

    }
    

    void Update()
    {
        if (myHealth.isAlive == true)
        {
            if (!isUsingKeyboard)
            {
                Move();
                Rotate();
                Swim();
            }
            else
            {
                //MoveWithKeyBoard();
                //RotateWithMouse();
                //SwimWithKeyBoard();
            }

            CheckForDamage();
        }
    }


    


    void CheckTowersOwned()
    {
        towersOwned = 0;

        for (int i = 0; i < myTowerManager.mapTowers.Capacity; i++)
        {
            if (myTowerManager.mapTowers[i].GetComponent<Tower>().myPaintState == myPaintState)
            {
                towersOwned++;
            }


            //if the towers distance is needed
            //float tempDist = Vector3.Distance(myTowerManager.mapTowers[i].transform.position, transform.position);
        }
    }

   /*
    #region Keyboard For Debug
    //key board controll functions*******************************************************************************************************************
    //*****************************************
    void MoveWithKeyBoard()
    {
        if (swimming)
        {
            moveSpeed = fastSpeed;
        }
        else
        {
            moveSpeed = normalSpeed;
        }
        int x = 0;
        int z = 0;

        if (Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.S))
        {
            z = 1;
        }
        else if (!Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.S))
        {
            z = -1;
        }
        else
        {
            z = 0;
        }

        if (Input.GetKey(KeyCode.A) && !Input.GetKey(KeyCode.D))
        {
            x = -1;
        }
        else if (!Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.D))
        {
            x = 1;
        }
        else
        {
            x = 0;
        }

        Vector3 movDireation = new Vector3(x, 0, z);

        movDireation = transform.TransformDirection(movDireation);
        movDireation *= moveSpeed;

        Vector3 velocity = rb.velocity;
        Vector3 velocityChange = (movDireation - velocity);
        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelChange, maxVelChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelChange, maxVelChange);
        velocityChange.y = 0;
        rb.AddForce(velocityChange, ForceMode.VelocityChange);
    }

    void RotateWithMouse()
    {
        RaycastHit onPlaneMousePos;
        LayerMask layerMast = 1 << 14;
        //layerMast = ~layerMast;
        Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out onPlaneMousePos, Mathf.Infinity,layerMast);
        Vector3 hitPoint = onPlaneMousePos.point;

        Debug.DrawLine(Camera.main.ScreenToWorldPoint(Input.mousePosition), hitPoint);


        desiredDir = Quaternion.LookRotation(new Vector3(hitPoint.x - transform.position.x, 0, hitPoint.z - transform.position.z));
        bodyToRotate.transform.rotation = Quaternion.Lerp(transform.rotation, desiredDir, rotSpeed * 100f * Time.deltaTime);

    }
    /*
    void SwimWithKeyBoard()
    {
        GameObject hitObj = null;

        RaycastHit myHit;

        if (Physics.Raycast(new Vector3(transform.position.x, 0.4f, transform.position.z), Vector3.down, out myHit, 0.6f))
        {
            hitObj = myHit.collider.gameObject;
            if (hitObj.layer == 0 || hitObj.CompareTag("Tile"))
            {
                curTile = hitObj;
            }
        }

        if (swimming == false)
        {
            if (hitObj.layer == myColorLayer)
            {
                canSwim = true;
            }

            rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

            if (myHit.collider == null)
            {
                canSwim = false;
            }
        }
        else if (swimming == true)
        {
            if (transform.position.y <= 0)
            {
                myFire.Restock();
            }

            if (!Input.GetKey(KeyCode.Space))
            {
                PopOut();
            }
        }


        if (canSwim == true)
        {
            float step = diveSpeed * Time.deltaTime;

            if (swimming == false)
            {
                if (Input.GetKey(KeyCode.Space))
                {
                    rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

                    Physics.IgnoreLayerCollision(gameObject.layer, gameObject.layer + 2, true);

                    rb.AddForce(Vector3.down * 8f, ForceMode.Impulse);
                    print(gameObject.name + " Diving Into Paint");

                    Vector3 tilePos = new Vector3(hitObj.transform.position.x, transform.position.y, hitObj.transform.position.z);

                    transform.position = Vector3.MoveTowards(transform.position, tilePos, step);

                    //push player over tile x,y
                    if (transform.position.y < 0f)
                    {

                        swimming = true;

                    }
                }
            }
        }


        if (transform.position.y <= -0.8f && swimming == false && canSwim == false)
        {
            PopOut();
        }


        if (transform.position.y <= -1f && curTile.layer != myColorLayer)
        {
            PopOut();
            myHealth.ChangeHealth(-curTile.GetComponent<Tile>().popDamage);
        }
    }
    


    //*****************************************
    //key board controll functions ************************************
    #endregion
    */

    #region Gamepad
    //Gamepad Controller code *****************************************
    //*****************************************
    void Move()
    {
        if (swimming == true)
        {
            moveSpeed = fastSpeed;

            if(mySFXManager.canPlaySwim == true)
            {
                mySFXManager.SwimLoopStart();
            }
        }
        else if (swimming == false)
        {
            moveSpeed = normalSpeed;

            if(mySFXManager.SwimLoop != null)
            {
                mySFXManager.StopCoroutine("SwimLoop");
                //mySFXManager.audioSource.Stop();
                mySFXManager.canPlaySwim = false;
            }
        }




        Vector3 targetVel = new Vector3(player.GetAxis("L Horizontal"), 0, player.GetAxis("L Vertical"));


        targetVel = transform.TransformDirection(targetVel * invertControls);
        targetVel *= moveSpeed;

        Vector3 velocity = rb.velocity;
        Vector3 velocityChange = (targetVel - velocity);

        velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelChange, maxVelChange);
        velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelChange, maxVelChange);
        velocityChange.y = 0;

        rb.AddForce(velocityChange, ForceMode.VelocityChange);

        //rb.AddForce(bodyToRotate.transform.position * moveSpeed, ForceMode.Force);



        if (rb.velocity.magnitude > 0.3f)
        {
            animator.SetBool("moving", true);

            if (mySFXManager.canPlayWalk == true)
            {
                mySFXManager.WalkLoopStart();
            }
        }
        else
        {
            animator.SetBool("moving", false);

            if (mySFXManager.WalkLoop != null)
            {
                mySFXManager.StopCoroutine("WalkLoop");
                //mySFXManager.audioSource.Stop();
                mySFXManager.canPlayWalk = false;
            }
        }

        
    }


    void Rotate()
    {
        string stickCheck = "R";
        
        /* Set stickCheck to null and uncomment to use this code again
        //Change to || also have super charged
        if (swimming == true)
        {
            stickCheck = "R";
        }
        else
        {
            stickCheck = "R";
        }
        */

        Vector2 lookDir = new Vector2(player.GetAxis(stickCheck + " Horizontal"), player.GetAxis(stickCheck + " Vertical"));
        float lookAngle = Mathf.Atan2(player.GetAxis(stickCheck + " Horizontal") * invertControls, player.GetAxis(stickCheck + " Vertical") * invertControls) * Mathf.Rad2Deg;
        

        //Vector2 lookDir = new Vector2(player.GetAxis("R Horizontal"), player.GetAxis("R Vertical"));
        //float lookAngle = Mathf.Atan2(player.GetAxis("R Horizontal") * invertControls, player.GetAxis("R Vertical") * invertControls) * Mathf.Rad2Deg;

        if (lookDir.sqrMagnitude > 0.15f)
        {
            desiredDir = Quaternion.AngleAxis(lookAngle, new Vector3(0, 1, 0));
        }

        bodyToRotate.transform.rotation = Quaternion.Lerp(bodyToRotate.transform.rotation, desiredDir, 10 * Time.deltaTime);


        //Old Rot with no rotSpeed
        /*
        if (lookDir.sqrMagnitude > 0.15f)
        {
            desiredDir = Quaternion.LookRotation(new Vector3(lookDir.x, 0, lookDir.y));
            bodyToRotate.transform.rotation = Quaternion.Lerp(transform.rotation, desiredDir, rotSpeed * 100 * Time.deltaTime);
        }*/
    }


    void Swim()
    {
        if (swimming == true && transform.position.y < -1.0f)
        {
            myFire.Restock();
        }

        GameObject hitObj = null;
        Vector3 tilePos;

        RaycastHit myHit;

        if (Physics.Raycast(new Vector3(transform.position.x, 1.5f, transform.position.z), Vector3.down, out myHit, 1.6f))
        {
            hitObj = myHit.collider.gameObject;

            if (myHit.collider == null)
            {
                canSwim = false;
            }
            else if (hitObj.CompareTag("Tile"))
            {
                curTile = hitObj;
            }


            tilePos = new Vector3(hitObj.transform.position.x, transform.position.y, hitObj.transform.position.z);
            Tile curTileScr = curTile.GetComponent<Tile>();

            if (curTileScr.myPaintState == myPaintState)
            {
                canSwim = true;

            }
            else if (curTileScr.myPaintState != myPaintState)
            {
                canSwim = false;
                rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

                //is the player in the wrong color?
                if (swimming == true && transform.position.y < -0.1f)
                {
                    PopOut();
                    myHealth.ChangeHealth(-curTile.GetComponent<Tile>().popDamage);
                }

                //Rumble players controller
                if (curTileScr.myPaintState != ColorState.Clean)
                {
                    player.SetVibration(0, 1, 0.01f);
                }

            }

            if (canSwim == true)
            {
                float step = diveSpeed * Time.deltaTime;

                //check if player wants to swim
                if (swimming == false)
                {
                    //dive in
                    if (player.GetAxis("LT") >= inputDeadZone)
                    {
                        Physics.IgnoreLayerCollision(gameObject.layer, gameObject.layer + 2, true);

                        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

                        //push player over tile x,y
                        transform.position = Vector3.MoveTowards(transform.position, tilePos, step);

                        rb.AddForce(Vector3.down * diveSpeed * 50, ForceMode.Force);
                        print(gameObject.name + " Diving Into Paint");



                        //player is swimming
                        if (transform.position.y <= -0.2f)
                        {
                            //5 is to play dive sound
                            mySFXManager.PlayOneShot(5);
                            print("swimming");
                            swimming = true;
                            swimVFX.SetActive(true);
                            animator.SetBool("swimming", swimming);
                            rb.velocity = Vector3.zero;
                        }
                    }
                }
                //While the player is swimming
                else if (swimming == true)
                {
                    //is the player in the wrong color?

                    if (player.GetAxis("LT") < inputDeadZone || transform.position.y >= -0.1f)
                    {
                        PopOut();
                    }
                }
            }
            else if (canSwim == false)
            {
                if (transform.position.y < -0.1f || transform.position.y >= 0.1f || swimming == true)
                {
                    swimming = false;
                    animator.SetBool("swimming", swimming);
                    PopOut();
                }
            }
        }
    }


    #endregion

    public void PopOut()
    {
        swimVFX.SetActive(false);
        print("Popping Out");
        if (myFire.addPaintCo != null)
        {
            print("HALTED: " + myFire.addPaintCo.ToString());
            myFire.StopCoroutine(myFire.addPaintCo);
            myFire.addingPaint = false;
        }
        

        float step = diveSpeed * Time.deltaTime;

        Vector3 popPos = new Vector3(transform.position.x, 0, transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, popPos, step * 2);

        if (transform.position.y >= -0.05f)
        {
            mySFXManager.PlayOneShot(6);
            Physics.IgnoreLayerCollision(gameObject.layer, gameObject.layer + 2, false);
            print("Popped");
            canSwim = false;
            swimming = false;
            
            animator.SetBool("swimming", swimming);
            myFire.canFire = true;
            myFire.firstRestock = true;
            rb.velocity = Vector3.zero;
        }


        /*
        rb.AddForce(Vector3.up * 8f, ForceMode.Impulse);


        if (transform.position.y, popPos) < 0.1f)
        {
            rb.velocity = Vector3.zero;



        }*/
        //rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
        //StartCoroutine("StopSwimming");
    }



    void CheckForDamage()
    {
        //If player is in the wrong color damage them
        if (curTile.layer != myColorLayer && curTile.layer != 0 && hold == false)
        {
            print(gameObject.name + " Is on the wrong color!");
            StartCoroutine("DamageOverTime");
        }
    }


    IEnumerator DamageOverTime()
    {
        hold = true;
        Tile curTileScr = curTile.GetComponent<Tile>();
        myHealth.ChangeHealth(-curTileScr.popDamage);
        yield return new WaitForSeconds(curTileScr.damageWaitTime);
        hold = false;

    }

    /*
    IEnumerator StopSwimming()
    {
        yield return new WaitForSeconds(0.2f);
        Physics.IgnoreLayerCollision(gameObject.layer, gameObject.layer + 2, false);
        swimming = false;
    }*/

    
    public void RespawnReset()
    {
        myHealth.ResetHealth();
        myFire.ResetFire();
        respawnTime = 0;
        hold = false;
        myHealth.isAlive = true;

    }
}
