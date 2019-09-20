using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantProjController : MonoBehaviour
{
    public int mPlayerID;
    [SerializeField] private float ChargeTime = 2.0f;
    [SerializeField] private float ProjectileSpeed = 0.5f;
    [SerializeField] private float FlyDistance = 10.0f;

    [SerializeField] private float DamageToPlayer;
    [SerializeField] private float DamageToBase;

    [SerializeField] private GameObject myProjectileBox;
    [SerializeField] private Transform mChargeIndicator;
    [SerializeField] private Transform myChargeFrame;

    private float LaserChargeIndicatorMaxLength;
    //for sounds 
    [SerializeField] private AudioSource mAudioSource = null;
    [SerializeField] private AudioClip superChargeClip;
    [SerializeField] private AudioClip superFireClip;

    public void PlayOneShot(AudioClip clip)
    {
        mAudioSource.PlayOneShot(clip);
    }
    //destroy itself when itself is diabled by the respawning system
    private void OnDisable()
    {
        Destroy(gameObject);
    }

    // Start is called before the first frame update
    void Start()
    {
        LaserChargeIndicatorMaxLength = mChargeIndicator.localScale.x;
        Vector3 temScale = mChargeIndicator.localScale;
        temScale.x = 0;
        mChargeIndicator.localScale = temScale;
        mPlayerID = gameObject.GetComponentInParent<PlayerController>().myPlayerID;
        //play super chage sound 
        PlayOneShot(superChargeClip);
        StartCoroutine("LaserCharge");
    }

    // Update is called once per frame
    void Update()
    {

    }


    //laser charging function 
    IEnumerator LaserCharge()
    {
        while (mChargeIndicator.localScale.x < LaserChargeIndicatorMaxLength)
        {
            Vector3 temScale = mChargeIndicator.localScale;
            temScale.x += LaserChargeIndicatorMaxLength * Time.deltaTime / ChargeTime;
            mChargeIndicator.localScale = temScale;
            yield return new WaitForEndOfFrame();

        }
        myChargeFrame.gameObject.SetActive(false);
        mChargeIndicator.gameObject.SetActive(false);
        //play super fire sound 
        PlayOneShot(superFireClip);
        StartCoroutine ("LaserFire");
        
    }

    //fire the laser box 
    IEnumerator LaserFire()
    {
        myProjectileBox.SetActive(true);
        LaserBoxController mMboxController = myProjectileBox.GetComponent<LaserBoxController>();
        mMboxController.mPlayerID = mPlayerID;
        mMboxController.laserDamageToBase = DamageToBase;
        mMboxController.laserDamageToPlayer = DamageToPlayer;
        myProjectileBox.transform.parent = null;
        Vector3 dir = transform.forward;
        while (myProjectileBox != null && (myProjectileBox.transform.position - transform.position).magnitude < FlyDistance )
        {
            myProjectileBox.transform.Translate(dir * ProjectileSpeed *Time.deltaTime, Space.World);
            yield return null;
        }
        Destroy(myProjectileBox);
        Destroy(gameObject);
    }


}
