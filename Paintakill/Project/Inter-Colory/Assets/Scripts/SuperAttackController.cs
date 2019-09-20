using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;


public class SuperAttackController : MonoBehaviour
{
    [HideInInspector] public int myPlayerID = 0;
    private ColorManager mCM = null;
    public PlayerController myPlayerController = null;
    [SerializeField] private GameObject LaserGameObject;
    [SerializeField] private GameObject UltraLaserGameObject;

    public GameObject RotationBody;
    //TowerRangIndicators
    public GameObject OldRangeDetectingIndicator;
    [SerializeField] private GameObject mTowerIndicator = null;
    [SerializeField] private Color towerReadyColor = Color.white;
    [SerializeField] private Color towerRangeIndColor = Color.white;

    //Towers in range data
    [SerializeField] private int numTowersInRange = 0;
    [SerializeField] private int numOfTowersNeedToUnleashSupper = 2;
    [SerializeField] private List<GameObject> towersInRangeList = new List<GameObject>();
    public List<GameObject> claimedTowersInRangeList = new List<GameObject>();
    [SerializeField] private List<GameObject> debugList = new List<GameObject>();
    [SerializeField] private List<GameObject> closestClaimedTowers = null;
    //used to unclock or lock players' super when the the level is reacher or the player is swimming 
    public bool isSuperUnclocked = false;

    //used to unclock the super ultra super when the player reached level 5
    public bool isUltraSuperUnlocked = false;

    private Player player = null;
    public bool isUsingKeyboard = false;

    //for super charge state 
    private bool isSuperCharged = false;
    [SerializeField] private GameObject superChargeEffect;

    //paint drop func
    int curPaintDrop = 0;
    [SerializeField] private GameObject paintDropPref;

    //camera zoom 
    [SerializeField] private GameObject playerCamera;

    private void OnDisable()
    {
        //print("hahsdhhfahs");
        towersInRangeList.Clear();
        claimedTowersInRangeList.Clear();
        closestClaimedTowers.Clear();
    }
    void Start()
    {
        GameObject mGM = GameObject.Find("GameManager");
        if (mGM != null)
        {
            mCM = mGM.GetComponent<ColorManager>();
        }
        player = myPlayerController.player;
        isUsingKeyboard = myPlayerController.isUsingKeyboard;
    }

    void Update()
    {
        player = myPlayerController.player;
        isUsingKeyboard = myPlayerController.isUsingKeyboard;

        //Turn on with a global debug mode
        //print("towers in range: " + towersInRangeList.Count);
        //print("claimedTowersInRange: " + claimedTowersInRangeList.Count);
        CountClaimedTowersInRange();
        closestClaimedTowers = FindTheClosestTowers(numOfTowersNeedToUnleashSupper, claimedTowersInRangeList);
        //old indicator 
        //CheckRangeIndicatorState();
        CheckRanIndiState();

        //using keyboard
        if (isUsingKeyboard)
        {
            shootLaserWithKeyboard();
        }
        else
        {
            shootLaser();
        }
    }
    #region Layer Fire control functions
    //main fireLaser function 
    private void LaserFire()
    {
        GameObject superProj = LaserGameObject;

        if (isUltraSuperUnlocked)
        {
            superProj = UltraLaserGameObject;
            playerCamera.GetComponent<CameraZoomController>().ZoomOutCamera(1);
        }
        else
        {
            playerCamera.GetComponent<CameraZoomController>().ZoomOutCamera(0);
        }
        GameObject mLaser = Instantiate(superProj, RotationBody.transform.position, RotationBody.transform.rotation, RotationBody.transform);


    }


    #endregion


    #region keyboard fire control

    //fire supperpower laser **************************************************************************************************
    void shootLaserWithKeyboard()

    {
        if (Input.GetKeyDown(KeyCode.R) && claimedTowersInRangeList.Count >= numOfTowersNeedToUnleashSupper)
        {

            //spawn laser box
            LaserFire();

            //change the towers back to default state

            List<GameObject> closestClaimedTowers = FindTheClosestTowers(numOfTowersNeedToUnleashSupper, claimedTowersInRangeList);

            //set towers into clean state and remove them from the claimed towers in range list 

            foreach (GameObject i in closestClaimedTowers)
            {
                i.GetComponent<Tower>().CleanPaintState();
                print("tower name: " + i.name);
                claimedTowersInRangeList.Remove(i);
            }
        }

    }
    #endregion


    #region gamepad fire control 
    //fire supperpower laser **************************************************************************************************
    void shootLaser()
    {
        if (player == null)
        {
            print("null player");
        }
        bool isSwimming = myPlayerController.swimming;
        //when there enough tower around & player is in the correct level && player is not swimming, charge the player
        if (player.GetButtonDown("B_Button") && claimedTowersInRangeList.Count >= numOfTowersNeedToUnleashSupper && isSuperUnclocked && !isSwimming && !isSuperCharged)
        {
            //set towers into clean state and remove them from the claimed towers in range list 
            for (int i = 0; i < closestClaimedTowers.Count; i++)
            {
                //print("index: " + i + "  " + closestClaimedTowers[i].name + " ListSize: "+ closestClaimedTowers.Count);
                //print("index: " + "1" + "  " + closestClaimedTowers[1].name + " ListSize: " + closestClaimedTowers.Count);
                closestClaimedTowers[i].GetComponent<Tower>().CleanPaintState();

                //create paint drops
                GameObject paintDrop = Instantiate(paintDropPref, closestClaimedTowers[i].transform.position, Quaternion.identity);
                paintDrop.GetComponent<PaintDropController>().targetTF = transform;
                ParticleSystem paintDropPS = paintDrop.GetComponent<ParticleSystem>();
                var main = paintDropPS.main;
                main.startColor = towerRangeIndColor;
                //clean it from the list
               // claimedTowersInRangeList.Remove(closestClaimedTowers[i]);
            }
        }
        //if the player is charged & not swimming & have enough level then unleash the super 
        else if (player.GetButtonDown("B_Button") && isSuperUnclocked && !isSwimming && isSuperCharged)
        {
            isSuperCharged = false;
            superChargeEffect.SetActive(false);
            LaserFire();
        }
    }
    #endregion

    //code for paint drops recieve 
    public void TakePaintDrop()
    {
        curPaintDrop++;
        if (curPaintDrop >= numOfTowersNeedToUnleashSupper)
        {
            //set super charge stuff
            isSuperCharged = true;
            superChargeEffect.SetActive(true);
            curPaintDrop = 0;
        }
    }

    //find the closest num of towers in a list 
    List<GameObject> FindTheClosestTowers(int num, List<GameObject> towerList)
    {
        List<GameObject> reVal = new List<GameObject>();


        if (towerList.Count != 0)
        {
            if (num > towerList.Count)
            {
                num = towerList.Count;
            }

            List<GameObject> temList = new List<GameObject>(towerList);
            for (int i = 0; i < num; i++)
            {
                GameObject min = temList[0];
                for (int j = 0; j < temList.Count; j++)
                {
                    float curDis = (temList[j].transform.position - gameObject.transform.position).magnitude;
                    float minDis = (min.transform.position - gameObject.transform.position).magnitude;
                    if (curDis < minDis)
                    {
                        min = temList[j];
                    }
                }
                reVal.Add(min);
                temList.Remove(min);
            }

            //turn off indicator on all not closest occupied towers 
            if (temList.Count != 0)
            {
                foreach (GameObject i in temList)
                {
                    i.GetComponent<Tower>().towerRangeIndicator.GetComponentInChildren<RangIndTController>().TurnOffIndicators();
                }
            }
        }


        return reVal;
    }


    //go through the tower listand count the claimed towers  **************************************************************************************************
    void CountClaimedTowersInRange()
    {
        foreach (GameObject i in towersInRangeList)
        {
            //print("i: " + i.layer);
            //print("parentLayer: " + myPlayerController.myColorLayer);

            if (i.layer == myPlayerController.myColorLayer)
            {
                //print("got a claimed tower");
                if (!claimedTowersInRangeList.Contains(i))
                {
                    claimedTowersInRangeList.Add(i);
                }
            }
            else
            {
                if (claimedTowersInRangeList.Contains(i))
                {
                    claimedTowersInRangeList.Remove(i);
                    //clean the indicator on it 
                    i.GetComponent<Tower>().towerRangeIndicator.GetComponentInChildren<RangIndTController>().TurnOffIndicators();
                }
            }
        }
    }

    #region for indicator old and new 
    //OLD**** control what indicator will be showen in the game grey or green **************************************************************************************************
    void CheckRangeIndicatorState()
    {
        bool isSwimming = myPlayerController.swimming;

        if (claimedTowersInRangeList.Count >= numOfTowersNeedToUnleashSupper && isSuperUnclocked && !isSwimming)
        {
            OldRangeDetectingIndicator.SetActive(true);

            if (gameObject.transform.parent.gameObject.GetComponent<PlayerController>().myPlayerID == 0)
            {
                OldRangeDetectingIndicator.GetComponent<SpriteRenderer>().color = new Color(0.5f, 0.85f, 1f);
            }
            else
            {
                OldRangeDetectingIndicator.GetComponent<SpriteRenderer>().color = new Color(0.86f, 0.41f, 0.55f);
            }
        }
        else
        {
            OldRangeDetectingIndicator.SetActive(false);
            //RangeDetectingIndicator.GetComponent<SpriteRenderer>().color = new Color(0.77f, 0.77f, 0.77f);
        }
    }
    //OLD****
    //NEW***
    //function that turn on a indicator
    private void CheckRanIndiState()
    {
        bool isSwimming = myPlayerController.swimming;
        //when the towers reach less towers than the super required, then the color of the indicator should change to show the change
        if (closestClaimedTowers.Count > 0 && closestClaimedTowers.Count < numOfTowersNeedToUnleashSupper && !isSwimming && isSuperUnclocked)
        {
            //turn on the indicator on the player 
            mTowerIndicator.SetActive(true);
            mTowerIndicator.GetComponent<SpriteRenderer>().color = towerRangeIndColor;
            foreach (GameObject i in closestClaimedTowers)
            {
                if (i.GetComponent<Tower>().towerRangeIndicator.GetComponent<RangIndTController>().mColor != towerRangeIndColor)
                {
                    //turn on its range indicator 
                    print("turn on the indicators");
                    i.GetComponent<Tower>().towerRangeIndicator.GetComponent<RangIndTController>().SetInitialVals(transform, towerRangeIndColor);
                }
            }
        }
        //when the towers reach enough towers the color of the indicator should change to show the change 
        else if (closestClaimedTowers.Count == numOfTowersNeedToUnleashSupper && !isSwimming && isSuperUnclocked)
        {
            //turn on the indicator on the player 
            mTowerIndicator.SetActive(true);
            mTowerIndicator.GetComponent<SpriteRenderer>().color = towerReadyColor;
            foreach (GameObject i in closestClaimedTowers)
            {
                //check if the incoming state different from the one right rightnow 
                if(i.GetComponent<Tower>().towerRangeIndicator.GetComponent<RangIndTController>().mColor != towerReadyColor)
                {
                    //turn on its range indicator 
                    print("turn on the readyddddd indicators");
                    i.GetComponent<Tower>().towerRangeIndicator.GetComponent<RangIndTController>().SetInitialVals(transform, towerReadyColor);
                }
            }
        }
        else
        {
            //turn off the indicator on the player
            mTowerIndicator.SetActive(false);
            foreach (GameObject i in claimedTowersInRangeList)
            {
                i.GetComponent<Tower>().towerRangeIndicator.GetComponentInChildren<RangIndTController>().TurnOffIndicators();
            }
        }
    }

    #endregion

    private void OnTriggerEnter(Collider col)
    {
        GameObject hitObj = col.gameObject;

        if (hitObj != null)
        {
            if (hitObj.name.Contains("Tower"))
            {
                towersInRangeList.Add(hitObj);
            }
        }
    }


    private void OnTriggerExit(Collider col)
    {
        GameObject hitObj = col.gameObject;

        if (hitObj != null)
        {
            GameObject targetedTower = towersInRangeList.Find(x => x.name == hitObj.name);

            if (targetedTower == null)
            {
                //nothing
            }
            else
            {
                towersInRangeList.Remove(targetedTower);
                //clean the indicator on it 


                if (claimedTowersInRangeList.Contains(targetedTower))
                {
                    targetedTower.GetComponent<Tower>().towerRangeIndicator.GetComponentInChildren<RangIndTController>().TurnOffIndicators();
                    claimedTowersInRangeList.Remove(targetedTower);
                }
            }
        }
    }
}

