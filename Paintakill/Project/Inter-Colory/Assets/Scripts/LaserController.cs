using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour
{

    [SerializeField] private float LaserChargeTime = 2.0f;
    [SerializeField] private float LaserLastingTime = 0.5f;

    [SerializeField] private float LaserDamageToPlayer;
    [SerializeField] private float LaserDamageToBase;

    [SerializeField] private GameObject myLaserBox;
    [SerializeField] private Transform myLaserChargeIndicator;
    [SerializeField] private Transform myLaserChargeFrame;

    private float LaserChargeIndicatorMaxLength;
    //for sounds 
    [SerializeField] private AudioSource mAudioSource = null;
    [SerializeField] private AudioClip superChargeClip;
    [SerializeField] private AudioClip superFireClip;

    public void PlayOneShot(AudioClip clip)
    {
        mAudioSource.PlayOneShot(clip);
    }

    // Start is called before the first frame update
    void Start()
    {
        LaserChargeIndicatorMaxLength = myLaserChargeIndicator.localScale.x;
        Vector3 temScale = myLaserChargeIndicator.localScale;
        temScale.x = 0;
        myLaserChargeIndicator.localScale = temScale;
        mAudioSource = GetComponent<AudioSource>();
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
        while (myLaserChargeIndicator.localScale.x < LaserChargeIndicatorMaxLength)
        {
            Vector3 temScale = myLaserChargeIndicator.localScale;
            temScale.x += LaserChargeIndicatorMaxLength * Time.deltaTime / LaserChargeTime;
            myLaserChargeIndicator.localScale = temScale;
            yield return new WaitForEndOfFrame();

        }
        myLaserChargeFrame.gameObject.SetActive(false);
        myLaserChargeIndicator.gameObject.SetActive(false);

        //play super fire sound 
        PlayOneShot(superFireClip);
        StartCoroutine ("LaserFire");
    }

    //fire the laser box 
    IEnumerator LaserFire()
    {
        myLaserBox.SetActive(true);
        myLaserBox.GetComponent<LaserBoxController>().laserDamageToBase = LaserDamageToBase;
        myLaserBox.GetComponent<LaserBoxController>().laserDamageToPlayer = LaserDamageToPlayer;

        yield return new WaitForSeconds(LaserLastingTime);
        Destroy(gameObject);
    }


}
