using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : Tile
{
    public bool isOwned = false;
    [SerializeField] private GameObject bluePlayer;
    [SerializeField] private GameObject redPlayer;

    [SerializeField] private AudioClip filled = null;

    private AudioSource audioSource = null;
    public Material curPaintMaterial = null;
    [SerializeField] private int fullPaintLevel = 4;
    [SerializeField] private int curPaintLevel = 0;
    public List<GameObject> paintLevelIndicators = new List<GameObject>();
    public List<GameObject> mainTowerMeshList = new List<GameObject>();

    public List<GameObject> TilesBelow = new List<GameObject>();
    bool isRecovering = false;
    [SerializeField] private float recoverInterval = 0.7f;
    [SerializeField] private float recoverCoolDown = 1.8f;
    [SerializeField] private float curRecoverTimer = 0.0f;
    private ColorState curColorState = ColorState.Clean;
    [SerializeField] private Transform instantPT = null;
    [SerializeField] private GameObject ExplosionPref = null;

    [SerializeField] private bool canTileRecover = false;
    [SerializeField] private bool isStateChangedTF = false;
    //for contested mode
    private ColorState prevIncProjColor = ColorState.Clean;
    [SerializeField] private int contestedTimes = 0;
    [SerializeField] private int trigContesT = 5;
    [SerializeField] private float contesCounInterval = 1.5f;
    [SerializeField] private bool isContested = false;
    [SerializeField] private float contestStateTime = 5.0f;
    [SerializeField] private MetricsManager mMetricsManager = null;

    IEnumerator coroutine;

    [SerializeField] private bool isPreOccupied = false;
    //for new tower range indicator 
    public GameObject towerRangeIndicator = null;
    //for new tower occupy effect 
    [SerializeField] private GameObject TowerSpillEffect = null;
    [SerializeField] private Color blueSpillColor;
    [SerializeField] private Color redSpillColor;


    private void Awake()
    {
        bluePlayer = GameObject.Find("Player 1");
        redPlayer = GameObject.Find("Player 2");
        if (GameObject.Find("GameManager").GetComponent<MetricsManager>())
        {
            mMetricsManager = GameObject.Find("GameManager").GetComponent<MetricsManager>();
        }
        else
        {
            print("MetricsManager Not Found");
        }
    }

    override public void Start()
    {
        base.Start();
        //take a place in the mMetricsManager with 0 times hit
        mMetricsManager.setTimesTowerIsPainted(name, 0);
        audioSource = GetComponent<AudioSource>();
        foreach (Transform child in transform)
        {
            if (child.gameObject.name.Contains("part"))
            {
                paintLevelIndicators.Add(child.gameObject);
            }
        }
        //set the tower state at the beginning if needed 

    }

    void Update()
    {
        if (isPreOccupied && myPaintState != ColorState.Clean)
        {
            print("run");
            curPaintLevel = fullPaintLevel;
            curColorState = myPaintState;
            myPaintState = ColorState.Clean;
            isPreOccupied = false;
        }
        if (!isContested) 
        {
            CheckColorStateChange();
            if (isStateChangedTF)
            {
                ParticleSystem explodePS = Instantiate(ExplosionPref, instantPT.transform.position, instantPT.transform.rotation).GetComponent<ParticleSystem>();
                var main = explodePS.main;
                main.startColor = mCM.GetColorDataPack(myPaintState, gameObject.tag).mColor;

                audioSource.PlayOneShot(filled);
                isOwned = true;
                //metrics.setTowersPainted(aPlayerID, 1);
                isStateChangedTF = false;
            }

            CheckRecoverLevel();
            //print("1_isstatechangedTF: " + isStateChangedTF);
            UpdateTowerLook();
            //print("3_isstatechangedTF: " + isStateChangedTF);

            if (canTileRecover)
            {
                RecoverTilesBelow();
            }
        }

    }



    #region PaintLevelIndicator part ****************************************************
    //call the coroutine function when the timer is bigger than cool down and it is nor recovering right now 
    void CheckRecoverLevel()
    {
        //count timer when it is colored
        if (myPaintState != ColorState.Clean)
        {
            curRecoverTimer += Time.deltaTime;

            if (curRecoverTimer >= recoverCoolDown && !isRecovering)
            {
                isRecovering = true;
                StartCoroutine("RecoverPaintLevel");
            }
        }

    }

    //function to stop the coroutine and stop the whole recover process and reset its variables  
    void StopRecoveringLevel()
    {
        StopCoroutine("RecoverPaintLevel");
        isRecovering = false;
        curRecoverTimer = 0.0f;
    }

    //actual coroutine that increase curPaintLevel until full
    IEnumerator RecoverPaintLevel()
    {
        while (curPaintLevel < fullPaintLevel)
        {
            curPaintLevel++;
            CheckColorStateChange();
            IndicatePaintLevel();
            yield return new WaitForSeconds(recoverInterval);
        }
        isRecovering = false;
    }

    //parent function that update the tower mesh and indicator based on its color state 
    private void UpdateTowerLook()
    {
        CheckColorStateChange();
        //print("2_isstatechangedTF: " + isStateChangedTF);
        IndicatePaintLevel();
        //print("4_isstatechangedTF: " + isStateChangedTF);
    }

    //function aim to check if current fill level is ready for state change 
    void CheckColorStateChange()
    {
        //print("curLevel: " + curPaintLevel + "  " + myPaintState);
        //call change state function to change the state to clean when curPaintLevel is 0
        if (curPaintLevel == 0 && myPaintState != ColorState.Clean)
        {
            //print("get cleaned canRecoverTIle: " + canTileRecover);
            isStateChangedTF = true;
            ChangePaintState(ColorState.Clean);
            curColorState = ColorState.Clean;
            curRecoverTimer = 0.0f;
            CheckTileRecover();
            isOwned = false;
            //set spill effect to false 
            TowerSpillEffect.SetActive(false);
        }
        //call change state function to change the state to the target color when curPaintLevel is full
        if (curPaintLevel == fullPaintLevel && myPaintState == ColorState.Clean)
        {
            isStateChangedTF = true;
            ChangePaintState(curColorState);
            //add exp to player when it is colored to another state
            if(curColorState == ColorState.Blue)
            {
                ExpController expSc = bluePlayer.GetComponent<ExpController>();
                if(expSc != null)
                {
                    expSc.AddExp(100);
                }
                mMetricsManager.setTowersPainted(0, 1);
                //set blue spill color 
                TurnOnTowerSpill(blueSpillColor);
            }
            else
            {
                ExpController expSc = redPlayer.GetComponent<ExpController>();
                if (expSc != null)
                {
                    expSc.AddExp(100);
                }
                mMetricsManager.setTowersPainted(1, 1);
                TurnOnTowerSpill(redSpillColor);
            }
            CheckTileRecover();
            //print("*isstatechangedTF: " + isStateChangedTF);

        }
    }

    //controll the filllevel indicator children 
    private void IndicatePaintLevel()
    {
        for (int i = 0; i < curPaintLevel; i++)
        {
            paintLevelIndicators[i].GetComponent<Renderer>().material = mCM.GetColorDataPack(curColorState, gameObject.tag).mPaintMaterial;
        }

        for (int j = curPaintLevel; j < fullPaintLevel; j++)
        {
            paintLevelIndicators[j].GetComponent<Renderer>().material = mCM.GetColorDataPack(ColorState.Clean, "Tower").mPaintMaterial;
        }
    }

    //function that turn on the spillout effect to show the tower is occupied 
    private void TurnOnTowerSpill(Color color)
    {
        //set the color of the effect system
        TowerSpillEffect.SetActive(true);
        ParticleSystem spillEffect = TowerSpillEffect.GetComponent<ParticleSystem>();
        var main = spillEffect.main;
        main.startColor = color;
    }

    #endregion  ************************************************************************


    #region code for anti-decay system including recovering paint of tiles
    //check the recover state of tiles and whether to recover them 
    private void CheckTileRecover()
    {

        if (isStateChangedTF && myPaintState != ColorState.Clean)
        {
            isOwned = true;
            print("exert control not clean");
            canTileRecover = true;
            ControlTilesBelow();
        }
        else if (isStateChangedTF && myPaintState == ColorState.Clean)
        {
            isOwned = false;
   //         print("exert control clean");
            canTileRecover = false;
            ReleaseTilesBelow();
        }
    }

    //search through the tiles below list and find the one that has different color with the tower and fix them
    private void RecoverTilesBelow()
    {
        for (int i = 0; i < TilesBelow.Count; i++)
        {
            Tile belowTileScr = TilesBelow[i].GetComponent<Tile>();

            if (belowTileScr.myPaintState != myPaintState)
            {
                // print("found a not same one");
                //coroutine = belowTileScr.RecoverToState(myPaintState, transform.position);
                //("recover the tile: " + TilesBelow[i].name);
                belowTileScr.ColorRecoverTower(myPaintState, transform.position);
            }
            //prevent tile from decay when a tile is under two towers control and one of them become clean
            belowTileScr.StopDecay();
            belowTileScr.isDecayable = false;            
            //print("stopdecay");
        }
    }

    //paint tiles below and make them not decayable at the moment when the tower is painted  
    private void ControlTilesBelow()
    {
        for (int i = 0; i < TilesBelow.Count; i++)
        {
            Tile belowTileScr = TilesBelow[i].GetComponent<Tile>();
            belowTileScr.StopDecay();
            //stop color recover in case that tiles are recover to a previous color 
            belowTileScr.StopColorRecover();
            belowTileScr.TakeTowerState(myPaintState, transform.position);
        }
    }

    //release itsControl over tiles below
    private void ReleaseTilesBelow()
    {
       // print("releasetiles");
        for (int i = 0; i < TilesBelow.Count; i++)
        {
            Tile belowTileScr = TilesBelow[i].GetComponent<Tile>();
            belowTileScr.isDecayable = true;
            //stop color recover in case that tiles are recover to a previous color 
            belowTileScr.EndColorRecover(transform.position);
        }
    }



    private void OnTriggerEnter(Collider col)
    {
        GameObject hitObj = col.gameObject;

        if (hitObj != null)
        {
            if (hitObj.CompareTag("Tile") == true)
            {
                TilesBelow.Add(hitObj);
            }
        }


    }
    #endregion

    #region override member functions
    //make the tower version of clean paint: it can't change the state directly cause it has its own special scirpts help that in update  
    override public void CleanPaintState()
    {
        //print("clean tower: " + gameObject.name);
        curPaintLevel = 0;
    }

    //change state function by passing a state by assigning the color on the main tower meshes 
    override protected void ChangePaintState(ColorState targetColor)
    {
        ColorDataPack curPack = mCM.GetColorDataPack(targetColor, gameObject.tag);
        myOwnerID = curPack.mPlayerID;
        gameObject.layer = (int)Mathf.Log(curPack.mLayer.value, 2);
        myPaintState = curPack.mColorState;
        //paint the main meshes 
        foreach (GameObject i in mainTowerMeshList)
        {
            i.GetComponent<Renderer>().material = curPack.mPaintMaterial;
        }
        if (isContested)
        {
            foreach (GameObject i in paintLevelIndicators)
            {
                i.GetComponent<Renderer>().material = curPack.mPaintMaterial;
            }
        }
    }

    override public void TakeProjectile(GameObject projectile, Projectile curProjScr)
    {
        if(!isContested)
        {
            //adding data to metrics show how many times this tower gets hit 
            mMetricsManager.setTimesTowerIsPainted(name, 1);
            //change curFillColor into the projectile's paint 
            if (curPaintLevel == 0 && myPaintState == ColorState.Clean)
            {
                curColorState = curProjScr.myPaintState;
                curPaintLevel++;
            }
            //add to current paint level if incoming projectile is the same paint 
            else if (curPaintLevel < fullPaintLevel && curColorState == curProjScr.myPaintState)
            {
                curPaintLevel++;
            }
            else if (curPaintLevel > 0 && curColorState != curProjScr.myPaintState)
            {
                curPaintLevel--;
                //stop recovering the paint level when the tower is getting hits
                StopRecoveringLevel();
            }
            UpdateTowerLook();

            if (prevIncProjColor != ColorState.Clean && prevIncProjColor != curProjScr.myPaintState && myPaintState == ColorState.Clean)
            {
                //print("contested!!!!!");
                contestedTimes++;
                if (contestedTimes >= trigContesT)
                {
                    StartCoroutine("EnterContestedState");
                }
                StopCoroutine("CleanContestedTimes");
                StartCoroutine("CleanContestedTimes");
            }

            prevIncProjColor = curProjScr.myPaintState;
        }
    }
    #endregion

    //for contested state of the tower
    IEnumerator CleanContestedTimes()
    {
        yield return new WaitForSeconds(contestStateTime);
        contestedTimes = 0;
    }

    IEnumerator EnterContestedState()
    {
        isContested = true;
        ChangePaintState(ColorState.Contested);
        curColorState = ColorState.Clean;
        curPaintLevel = 0;
        yield return new WaitForSeconds(contestStateTime);
        isContested = false;
        ChangePaintState(ColorState.Clean);
    }
    //for the super attack to make the tower into contested state
    public void BustTheTower()
    {
        curPaintLevel = 0;
        CheckColorStateChange();
        StartCoroutine("EnterContestedState");
    }

}
