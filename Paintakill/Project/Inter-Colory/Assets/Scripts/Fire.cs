using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class Fire : MonoBehaviour
{
    //public player data
    [HideInInspector] public PlayerController playerController = null;
    [HideInInspector] public int myPlayerID = 0;
    [HideInInspector] public Player player = null;

    [HideInInspector] public bool canFire = true;

    [SerializeField] private Transform[] firePoint = null;
    [SerializeField] private GameObject projectilePref = null;
    private Text ammoTextObj = null;
    [SerializeField] private Slider ammoSliderObj = null;

    [SerializeField] private int maxPaint = 100;
    [SerializeField] private int curPaint = 100;

    [SerializeField] private float maxFireForce = 10f;
    [SerializeField] private float minFireForce = 1f;
    [SerializeField] private float curFireForce = 0f;



    //[SerializeField] private GameObject hoseScaler = null;
    //[SerializeField] private ParticleSystem paintParticleSystem = null;
    //[SerializeField] private Material myMaterial = null;
    //[SerializeField] private BoxCollider hoseCollider = null;

    //[SerializeField] private float maxFireRange = 30f;
    //[SerializeField] private float minFireRange = 1f;
    //[SerializeField] private float dropDist = 6f;

    //[SerializeField] private float hoseDepletionRate = 0.1f;


    [SerializeField] private float replenishFirstWait = 0.5f;
    [SerializeField] private float replenishNormalWait = 0.2f;
    [SerializeField] private int replenishRate = 1;
    //[SerializeField] private float damageAmount = 250f;
    //[SerializeField] private float maxParticleSpeed = 100f;


    [SerializeField] private GameObject paintTarget = null;
    [SerializeField] private Transform startMarker = null;
    [SerializeField] private Transform endMarker = null;

    [SerializeField] private AudioSource audioSource = null;
    [SerializeField] private AudioClip reloadSFX = null;
    [SerializeField] private AudioClip missFireSFX = null;
    [SerializeField] private AudioClip shootPaintSFX = null;
    [SerializeField] private AudioClip restockSFX = null;

        


    [SerializeField] private float reloadTime = 0.1f;

    private bool isUsingKeyboard = false;

    //[SerializeField] private GameObject paintTarget = null;
    //[SerializeField] private Transform startMarker = null;
    //[SerializeField] private Transform endMarker = null;
    

    float minFire = 0.4f;
    float rayDist = 0f;
    public bool firstRestock = true;
    bool reloading = false;
    public bool addingPaint = false;

    public Coroutine reloadCo = null;
    public Coroutine addPaintCo = null;

    public void IncreMaxPaint (int paint)
    {
        maxPaint += paint;
        ammoSliderObj.maxValue = maxPaint;
        UpdateUIInfo();
    }

    void Start()
    {
        UpdateUIInfo();
        curFireForce = minFireForce;
        isUsingKeyboard = gameObject.GetComponentInParent<PlayerController>().isUsingKeyboard;
        ammoSliderObj.maxValue = maxPaint;
    }


    void Update()
    {
        CheckFire();
        isUsingKeyboard = gameObject.GetComponentInParent<PlayerController>().isUsingKeyboard;

    }

    //Can the player fire
    void CheckFire()
    {
        //if out of paint !canFire
        if (curPaint <= 0)
        {
            canFire = false;
        }

        //Move the retical on the ground
        paintTarget.transform.position = Vector3.Lerp(startMarker.position, endMarker.position, player.GetAxis("RT"));

        //Check for input from player
        if (canFire)
        {
            //Start Firing when not using keyboard 
            if(!isUsingKeyboard)
            {
                if (player.GetAxis("RT") > minFire)
                {
                    StartFire();
                }
                else if (player.GetAxis("RT") <= minFire)
                {
                    StopFire();
                }
            }

            //start firing when using keyboard for debug only
            else
            {
                if(Input.GetMouseButton(0))
                {
                    StartFire();
                }
                else
                {
                    StopFire();
                }
            }




        }
        else
        {
            //using gamepad
            if (!isUsingKeyboard)
            {
                if (player.GetAxis("RT") <= minFire) // && !reloading)
                {

                    
                    if (reloadCo != null && reloading == true)
                    {
                        print("HALTED reload");
                        StopCoroutine(reloadCo);
                        firstRestock = true;
                        reloading = false;
                    }

                    
                    reloading = false;
                    canFire = true;

                    PlayOneShot(missFireSFX);
                }
            }

            //using keyboad for debug only 
            else
            {
                if (!Input.GetMouseButton(0))
                {
                    canFire = true;
                }
            }
            

        }
    }


    void StartFire()
    {
        ChangeFireForce(1f);

        while (reloading == false)
        {
            for (int i = 0; i < firePoint.Length; i++)
            {
                GameObject curPaintBall = Instantiate(projectilePref, firePoint[i].transform.position, firePoint[i].transform.rotation);

                Projectile curProjScr = curPaintBall.GetComponent<Projectile>();
                curProjScr.myPlayerID = myPlayerID;

                curProjScr.fireForce = curFireForce;
                
            }

            //take away <array length> of ammo
            ChangePaint(-firePoint.Length);

            PlayOneShot(shootPaintSFX);
            PlayOneShot(reloadSFX);

            //Wait for reload
            reloadCo = StartCoroutine("Reload");
        }

        //Update Ammo Textmesh
        UpdateUIInfo();
    }


    void StopFire()
    {
        ChangeFireForce(-1f);

    }


    int ChangeFireForce(float change)
    {
        curFireForce += change;

        if (curFireForce > maxFireForce)
        {
            curFireForce = maxFireForce;
        }
        else if (curFireForce < minFireForce)
        {
            curFireForce = minFireForce;
        }

        return Mathf.RoundToInt(curFireForce);
    }

    //creates an interval for firing projectiles
    IEnumerator Reload()
    {
        print("Reloading");
        reloading = true;
        yield return new WaitForSeconds(reloadTime);

        reloading = false;
    }


    public void ChangePaint(int change)
    {
        if (curPaint <= maxPaint)
        {
            curPaint += change;
        }
        else if (curPaint > maxPaint)
        {
            curPaint = maxPaint;
        }
    }


    public void PlayOneShot(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }


    IEnumerator AddPaint(float time)
    {
        print("addingPaint");
        addingPaint = true;
        yield return new WaitForSeconds(time);
        ChangePaint(replenishRate);
        UpdateUIInfo();
        PlayOneShot(restockSFX);
        addingPaint = false;
    }

    /*
    void StopHose()
        {

            if (audioSourceHose.volume > 0)
            {
                audioSourceHose.volume -= 0.1f;
            }

            if (main.startSpeed.constant > 0f)
            {
                main.startSpeed = main.startSpeed.constant - fireSpeed * 10;
            }

            if (hoseScaler.transform.localScale.y > 1f)
            {
                hoseScaler.transform.localScale -= new Vector3(0, fireSpeed / 2, 0);
            }

            if (hoseScaler.transform.localScale.z > minFireRange)
            {
                hoseScaler.transform.localScale -= new Vector3(0, 0, fireSpeed * 2);
            }
            else if (hoseScaler.transform.localScale.z <= 1f)
            {
                hoseCollider.enabled = false;
                spraying = false;
                main.startSpeed = 0f;
                paintParticleSystem.Stop(true);
            }

            ///Play sound
            //
        }
                */

   
    void UpdateUIInfo()
    {
        int tempCurPaint = 0;
        if (curPaint <= 0f)
        {
            curPaint = 0;
        }
        else
        {
            tempCurPaint = Mathf.RoundToInt(curPaint);
            if (tempCurPaint > maxPaint)
            {
                tempCurPaint = maxPaint;
                curPaint = maxPaint;
                
            }
        }

        UpdateAmmoBar();
    }

    void UpdateAmmoBar()
    {
        ammoSliderObj.value = curPaint;
    }

    /*
    void UpdateAmoText(int tempCurPaint)
    {
        ammoTextObj.text = "Ammo: " + tempCurPaint.ToString();
    }*/


    public void Restock()
    {
        if (playerController.swimming == true)
        {
            canFire = false;

            if (curPaint < maxPaint && !addingPaint)
            {
                if(firstRestock == false)
                {
                    print("Normal Wait Time");
                    addPaintCo = StartCoroutine("AddPaint", replenishNormalWait);
                }
                else
                {
                    print("First Wait Time");
                    firstRestock = false;
                    addPaintCo = StartCoroutine("AddPaint", replenishFirstWait);
                    
                }

            }
        }
    }


    public void ResetFire()
    {
        ChangePaint(maxPaint);
        UpdateUIInfo();
    }
    
    
    


    /*
    void OnTriggerEnter(Collider col)
    {
        GameObject hitObj = col.gameObject;

        if (hitObj != null)
        {
            if (hitObj.CompareTag("Colorable"))
            {
                Tile hitTile = hitObj.GetComponent<Tile>();
                
                if (hitTile.paintTime == false)
                {
                    //use to register this hit 
                    hitTile.getHitThisFrame = true;
                    hitObj.layer = gameObject.layer + 2;
                    hitObj.GetComponent<Renderer>().material = myMaterial;
                    hitTile.myOwnerID = myPlayerID;
                }
            }
            else if (hitObj.CompareTag("Destructible"))
            {
                hitObj.GetComponent<Health>().ChangeHealth(-damageAmount);
            }
        }

    }


    void OnTriggerStay(Collider col)
    {
        GameObject hitObj = col.gameObject;

        if (hitObj != null)
        {
            if (hitObj.CompareTag("Colorable"))
            {
                Tile hitTile = hitObj.GetComponent<Tile>();

                if (hitTile.paintTime == true)
                {
                    Tower hitTower = hitTile.GetComponent<Tower>();

                    while (hitTower.filling == false)
                    {
                        hitTower.StartCoroutine("Fill");

                        if (hitObj.layer == 0 || hitObj.layer == playerController.myColorLayer)
                        {
                            if (hitTower.curTower < hitTower.fullTower)
                            {
                                hitTower.curTower++;
                                audioSourcePaint.PlayOneShot(paintSplat);
                            }
                        }
                        else
                        {
                            if (hitTower.curTower > 0)
                            {
                                hitTower.curTower--;
                            }
                        }

                        
                    }

                    print("Fill Amount: " + hitTower.curTower + " " + hitTower.fullTower);


                    if (hitTower.curTower >= hitTower.fullTower)
                    {
                        hitObj.layer = gameObject.layer + 2;
                        hitObj.GetComponent<Renderer>().material = myMaterial;
                        hitTile.myOwnerID = myPlayerID;
                    }
                }
            }
        }

    }
    */


    
    
    





    /*
    void FireWithKeyboad()
    {
        if (curAmoStock <= 0)
        {
            canFire = false;
        }

        if (canFire)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                canFire = false;
                GameObject curPaintBall = Instantiate(paintBallPref, firePoint[0].transform.position, firePoint[0].transform.rotation);
                PaintBall cPBScr = curPaintBall.GetComponent<PaintBall>();
                cPBScr.parentID = myPlayerID;
                cPBScr.setMove = true;
                Rigidbody cPBRB = curPaintBall.GetComponent<Rigidbody>();
                cPBRB.AddForce(firePoint[0].transform.forward * fireSpeed, ForceMode.VelocityChange);

                curAmoStock -= 1;

                //for (int i = 0; i < firePoint.Length; i++)
                //{
                //}

                ///TODO///
                ///PlayFireSound

                UpdateUIInfo();

                //Wait for reload//
                StartCoroutine("ReloadTimer");
            }
        }
        else
        {
            if (!reloading)
            {
                canFire = true;
                ///TODO///
                ///PlayReadyToFireSound
            }
        }
    }
    */
}
