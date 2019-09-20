using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tiles are things on the world grid that needs to be painted.
/// THIS IS AN INHERITED CLASS AND SHOULD STAY AS GENERAL AS POSSIBLE
/// </summary>
public class Tile : MonoBehaviour
{
    #region Tile variables
    public int myOwnerID = 0;
    public bool isDecayable = true;
    public int popDamage = 100;
    public float damageWaitTime = 0.5f;
    public float decayTime = 5f;
    public ColorState myPaintState = ColorState.Clean;
    protected bool isDecaying = false;
    #endregion
    private Renderer mRenderer;
    #region tile recover to tower color variables
    [SerializeField] private float recoverWaitTime = 0.8f;
    [SerializeField] private bool isRecovering = false;
    #endregion

    protected GameObject mGM = null;
    protected ColorManager mCM = null;

    //for contested tiles between two towers
    [SerializeField] private float disToTower = Mathf.Infinity;
    [SerializeField] private Vector3 towerPosition = new Vector3(0.0f, 0.0f, 0.0f);
     IEnumerator coroutine;
    bool isDebug = false;
    virtual public void Start()
    {
        mGM = GameObject.Find("GameManager");
        mCM = mGM.GetComponent<ColorManager>();
        
        //myPaintState = ColorState.Clean;
        mRenderer = GetComponent<Renderer>();
    }

    void Update()
    {
        CheckForDecay();
    }

    #region restore color to towere color 
    //function that will be called by tower to recover the color state to targeted state in a period of time 

    public void ColorRecoverTower (ColorState state, Vector3 towerPos)
    {
        float dis = Mathf.Abs((towerPos - transform.position).magnitude);
        if (!isRecovering && dis < disToTower)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                //reset distance to tower value 
            }
            coroutine = RecoverToState(state, dis, towerPos);
            StartCoroutine(coroutine);
 
        }
    }

    public IEnumerator RecoverToState(ColorState state, float dis, Vector3 towerPos)
    {
        //print("recoveringtilesfrom the tower: " + gameObject.name);
        isRecovering = true;
        yield return new WaitForSeconds(recoverWaitTime);
        ChangePaintState(state);
        //print("recoveringtilesfrom the tower finished: "+state);
        //print(myPaintState);
        isRecovering = false;

        //set disToTower to dis after the state is changed in case it got stopped in the middle 
        disToTower = dis;
        towerPosition = towerPos;
    }
    //just clean the coroutine for tower controll
    public void StopColorRecover()
    {
        if(coroutine != null)
        {
            StopCoroutine(coroutine);
            //reset distance to tower value 
        }
        isRecovering = false;
    }
    //reset tile to before tower control state
    public void EndColorRecover(Vector3 towerPos)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        //reset distance to tower value when the tower control ends
        float dis = Mathf.Abs((towerPos - transform.position).magnitude);
        if (!(dis > disToTower))
        {
            disToTower = Mathf.Infinity;
            towerPosition = new Vector3(0, 0, 0);
        }
        isRecovering = false;
    }
    #endregion

    #region decay part decay to clean color 
    virtual public void CheckForDecay()
    {
        //if this tile needs to decay
        if (isDecayable == true && myPaintState != ColorState.Clean && isDecaying == false)
        {
            //print(gameObject.name + " is decaying.");
            isDecaying = true;
            StartCoroutine("Decay");
        }
    }
    private IEnumerator Decay()
    {
        yield return new WaitForSeconds(decayTime);
        isDecaying = false;
        CleanPaintState();
    }
    #endregion

    //Applies needed projectile details to this tile
    /// What if we used structs, or globabl variables to hold the separate states of paint?
    virtual public void TakeProjectile(GameObject projectile, Projectile curProjScr)
    {
        //print("Painting Tile");
        //decay = true;
        if (isDecayable == true && isDecaying == true)
        {
            StopCoroutine("Decay");
            isDecaying = false;
        }
        //when the bullet with same color hit, the disToTower should not be reset 
        //in case that this tile is under a freindly tower and a enemy tower.
        //otherwise this tile will temporary gets controlled by the enemy tower
        if(curProjScr.myPaintState != myPaintState)
        {
            //to make sure the dis is set to infinit so that the tower control could work on a tile colored by bullet 
            disToTower = Mathf.Infinity;
            towerPosition = new Vector3(0, 0, 0);
            ChangePaintState(curProjScr.myPaintState);
        }
    }
    //for tower class to call to change state directly without delay
    public void TakeTowerState(ColorState towerState, Vector3 towerPos)
    {
        float dis = Mathf.Abs((towerPos - transform.position).magnitude);
        if (dis < disToTower)
        {
            ChangePaintState(towerState);
            disToTower = dis;
            towerPosition = towerPos;
        }
    }
    //sub function used to stop decay function and reset its variables 
    public void StopDecay()
    {
        StopCoroutine("Decay");
        isDecaying = false;
        isDecayable = false;
    }
    //taking in the colorstate and change every color related variables to corresponding color 
    virtual protected void ChangePaintState(ColorState targetColor)
    {

        ColorDataPack curPack = mCM.GetColorDataPack(targetColor, gameObject.tag);
        myOwnerID = curPack.mPlayerID;
        mRenderer.material = curPack.mPaintMaterial;
        gameObject.layer = (int) Mathf.Log(curPack.mLayer.value,2);
        myPaintState = curPack.mColorState;
    }
    //cleans tile paint details
    virtual public void CleanPaintState()
    {
        ChangePaintState(ColorState.Clean);
        disToTower = Mathf.Infinity;
        towerPosition = new Vector3(0, 0, 0);
        //print("Cleaned Tile");
    }

}
