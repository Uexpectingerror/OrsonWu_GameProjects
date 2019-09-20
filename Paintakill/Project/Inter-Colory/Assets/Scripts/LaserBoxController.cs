using System.Collections;

using System.Collections.Generic;

using UnityEngine;



public class LaserBoxController : MonoBehaviour

{
    [HideInInspector] public float laserDamageToPlayer = 1000.0f;
    [HideInInspector] public float laserDamageToBase = 2000.0f;
     public int mPlayerID = 0;
     public ColorState mColor = ColorState.Clean;
    [SerializeField] private GameObject ExplosionPref = null;
    //for sounds 
    [SerializeField] private AudioSource mAudioSource = null;
    [SerializeField] private AudioClip superExplosionClip = null;
    private GameObject mGM = null;
    private ColorManager mCM = null;

    public void PlayOneShot(AudioClip clip)
    {
        mAudioSource.PlayOneShot(clip);
    }


    // Start is called before the first frame update

    void Start()

    {
        if (mPlayerID == 0)
        {
            mColor = ColorState.Blue;
        }
        else if (mPlayerID == 1)
        {
            mColor = ColorState.Red;
        }
        //find reference 
        mAudioSource = GetComponent<AudioSource>();
        mGM = GameObject.Find("GameManager");
        mCM = mGM.GetComponent<ColorManager>();
    }



    // Update is called once per frame

    void Update()

    {
        if (mPlayerID == 0)
        {
            mColor = ColorState.Blue;
        }
        else if (mPlayerID == 1)
        {
            mColor = ColorState.Red;
        }
    }



    private void OnHitEffect()
    {
        PlayOneShot(superExplosionClip);
        Quaternion angle = Quaternion.Euler(90, 0, 0);
        ParticleSystem explodePS = Instantiate(ExplosionPref, transform.position, angle).GetComponent<ParticleSystem>();
        var main = explodePS.main;
        main.startColor = mCM.GetColorDataPack(ColorState.Contested, gameObject.tag).mColor;
    }

    //wipe out colors on the path 

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag=="Tile")
        {
            other.gameObject.GetComponent<Tile>().CleanPaintState();
        }

        else if (other.tag== "Tower")
        {
            //other.gameObject.GetComponent<Tower>().CleanPaintState();
            other.gameObject.GetComponent<Tower>().BustTheTower();
            //play sound and spawn explosion effect 
            OnHitEffect();
        }

        //else if (other.gameObject.GetComponent<PlayerController>()!=null && mPlayerID != other.gameObject.GetComponent<PlayerController>().myPlayerID)
        //{
        //    other.gameObject.GetComponent<Health>().ChangeHealth(-laserDamageToPlayer);
        //}

        else if (other.tag == "Base" )
        {
            if (other.gameObject.GetComponent<BaseController>().mColorState != mColor)
            {
                //print("hit the base");
                other.gameObject.GetComponent<Health>().ChangeHealth(-laserDamageToBase);
                OnHitEffect();
                Destroy(gameObject);
            }
        }
        else if (other.CompareTag("Shield"))
        {
            if (other.gameObject.GetComponent<ShieldController>().mColorstate != mColor)
            {
                OnHitEffect();
                Destroy(gameObject);
            }
        }
        else if (other.CompareTag("Player"))
        {
            if(other.gameObject.GetComponent<PlayerController>().myPlayerID != mPlayerID)
            {
                other.gameObject.GetComponent<Health>().ChangeHealth(-laserDamageToPlayer);
                OnHitEffect();
                Destroy(gameObject);
            }
        }

        //        print("hit the: "+ other.tag + other.name);
    }


}

