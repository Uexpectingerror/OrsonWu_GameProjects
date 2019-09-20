using System.Collections;

using System.Collections.Generic;

using UnityEngine;



public class BaseController : MonoBehaviour
{
    public ColorState mColorState;
    private List<GameObject> mTilesBelow = new List<GameObject>();
    [SerializeField] private List<GameObject> mDefenTowers = new List<GameObject>();
    [SerializeField] private float shieldDownDelay = 0.5f;
    [SerializeField] private GameObject redLine;
    [SerializeField] private GameObject blueLine;
    [SerializeField] private int defenTowerNum = 3;
    [SerializeField] private float healthRegenPT = 100.0f;
    [SerializeField] private float HRinterval = 1.0f;
    [SerializeField] private GameObject gameManager = null;
    private bool isHealRegeing = false;
    private List<GameObject> mActiveDefeTow = new List<GameObject>();
    private bool isShieldReady = false;
    private bool isShieldReadyLF = false;
    bool once = true;
    IEnumerator coroutine;

    //list to store all the lines 
    private List<GameObject> lineList = new List<GameObject>();
    // Start is called before the first frame update

    void Start()
    {
        gameManager = GameObject.Find("GameManager");
    }

    void DrawLine (Transform targetPos)
    {
        if (mColorState == ColorState.Red)
        {
            GameObject line = Instantiate(redLine, targetPos);
            line.GetComponent<LineRenderer>().SetPosition(0, transform.position);
            line.GetComponent<LineRenderer>().SetPosition(1, targetPos.position);
        }

        else if (mColorState == ColorState.Blue)
        {
            GameObject line = Instantiate(blueLine, targetPos);
            line.GetComponent<LineRenderer>().SetPosition(0, transform.position);
            line.GetComponent<LineRenderer>().SetPosition(1, targetPos.position);
        }
    }



    void CheckDefenTowers()
    {
        //add 
        foreach (GameObject i in mDefenTowers)
        {
            if (i.GetComponent<Tower>().myPaintState == mColorState && !mActiveDefeTow.Contains(i))
            {
                mActiveDefeTow.Add(i);
                DrawLine(i.transform);
            }
        }

        //remove 
        if(mActiveDefeTow.Count!=0)
        {
            foreach (GameObject i in mActiveDefeTow)
            {
                if (i.GetComponent<Tower>().myPaintState != mColorState)
                {
                    mActiveDefeTow.Remove(i);
                    LineRenderer[] lineArray = i.transform.GetComponentsInChildren<LineRenderer>();
                    foreach (LineRenderer j in lineArray)
                    {
                        if (j.gameObject.tag == "LineOfBase")
                        {
                            Destroy(j.gameObject);
                        }
                    }

                }
            }

        }
    }



    void CheckHealthRegen()
    {
        CheckDefenTowers();

        if(mActiveDefeTow.Count > 0 && !isHealRegeing)
        {
            StartCoroutine("HealthRege");
        }
    }



    IEnumerator HealthRege()
    {
        isHealRegeing = true;
        GetComponent<Health>().ChangeHealth(healthRegenPT * mActiveDefeTow.Count);
        yield return new WaitForSeconds(1.0f);
        isHealRegeing = false;
    }



    void CheckIsShieldReady()
    {
        isShieldReadyLF = isShieldReady;
        isShieldReady = true;

        //check if all defense towers are occupied 
        foreach (GameObject i in mDefenTowers)
        {
            if (i.GetComponent<Tower>().myPaintState != mColorState)
            {
                isShieldReady = false;
            }
        }

        if(isShieldReady && !isShieldReadyLF)
        {
            GetComponentInChildren<ShieldController>().ActivateShield();
        }

        if(!isShieldReady && isShieldReadyLF)
        {
            StartCoroutine("ShieldDownDelay");
        }
    }



    IEnumerator ShieldDownDelay()
    {
        yield return new WaitForSeconds(shieldDownDelay);
        GetComponentInChildren<ShieldController>().ShutdownShield();
    }



    // Update is called once per frame

    void Update()
    {
        if(once)
        {
            ControlTilesBelow();

            once = false;
        }
        else
        {
            RecoverTilesBelow();
        }
        CheckIsShieldReady();
        CheckHealthRegen();
    }



    private void OnTriggerEnter(Collider col)
    {
        GameObject hitObj = col.gameObject;
        if (hitObj != null)
        {
            if (hitObj.CompareTag("Tile") == true)
            {
                mTilesBelow.Add(hitObj);
            }
        }
    }

    private void RecoverTilesBelow()
    {

        for (int i = 0; i < mTilesBelow.Count; i++)
        {

            Tile belowTileScr = mTilesBelow[i].GetComponent<Tile>();

            //prevent conflicts when a tile is under two towers' control 

            belowTileScr.StopDecay();

            if (belowTileScr.myPaintState != mColorState)
            {
                // print("found a not same one");
                //coroutine = belowTileScr.RecoverToState(mColorState, transform.position);
                //StartCoroutine(coroutine);
                belowTileScr.ColorRecoverTower(mColorState, transform.position);

            }

        }

    }
    //paint tiles below and make them not decayable at the moment when the tower is painted  
    private void ControlTilesBelow()
    {
        for (int i = 0; i < mTilesBelow.Count; i++)
        {

            Tile belowTileScr = mTilesBelow[i].GetComponent<Tile>();

            belowTileScr.StopDecay();

            //stop color recover in case that tiles are recover to a previous color 

            belowTileScr.StopColorRecover();

            belowTileScr.TakeTowerState(mColorState, transform.position);
        }
    }


    void OnDisable()
    {
        gameManager.SendMessage("GameOver");
    }


}

