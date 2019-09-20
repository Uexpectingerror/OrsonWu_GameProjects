using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldController : MonoBehaviour
{
    bool isShieldActive = false;
    public ColorState mColorstate = ColorState.Clean;
    private Vector3 shieldSize;
    [SerializeField] float updateInterval = 0.02f;
    [SerializeField] float expandTime = 0.8f;
    float t = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        shieldSize = transform.localScale;
        transform.localScale = new Vector3(0.00f, 0.00f, 0.00f);
        mColorstate = GetComponentInParent<BaseController>().mColorState;
    }

    //helper func
    public void ActivateShield()
    {
        StartCoroutine("ShieldActivate");
    }

    //helper func 
    public void ShutdownShield()
    {
        StartCoroutine("ShieldShutdown");
    }

    private IEnumerator ShieldActivate()
    {
        //stop shield shutdown
        StopCoroutine("ShieldShutdown");
        //start to ativate 
        isShieldActive = true;
        while (transform.localScale.x < shieldSize.x && isShieldActive)
        {
            float f = t / expandTime;
            //print("shieldactiveT: " + t + " f: " + f);
            //a slow down lerp function 
            f = -1 * (f - 1) * (f - 1) + 1;
            //print("shieldactiveT: " + t + " f: " + f);
            Vector3 scale = Vector3.Lerp(new Vector3(0.00f, 0.00f, 0.00f), shieldSize, f);
            transform.localScale = scale;

            yield return new WaitForSeconds(updateInterval);
            t += updateInterval;
        }
        t = 0.0f;
        transform.localScale = shieldSize;
        //StartCoroutine("ShieldShutdown");
    }

    private IEnumerator ShieldShutdown()
    {
        //stop shield activate
        StopCoroutine("ShieldActivate");
        //stop shield shutdown
        isShieldActive = false;
        while (transform.localScale.x > 0.0f && !isShieldActive)
        {
            float f = t / expandTime;
            //a slow down lerp function 
            f = -1 * (f - 1) * (f - 1) + 1;
            f = 1 - f;
            Vector3 scale = Vector3.Lerp(new Vector3(0.00f, 0.00f, 0.00f), shieldSize, f);
            transform.localScale = scale;

            yield return new WaitForSeconds(updateInterval);
            t += updateInterval;
        }
        t = 0.0f;
        transform.localScale = new Vector3(0.00f, 0.00f, 0.00f);
    }

    /*
    private void OnTriggerEnter(Collider other)
    {
        //        print("collider: " + other.name);

        if (other.transform.tag == "paintBall")
        {
            Destroy(other.gameObject);

           
            if (other.gameObject.GetComponent<LaserBoxController>() != null)
            {
                if (other.gameObject.GetComponent<LaserBoxController>().mColor != GetComponentInParent<BaseController>().mColorState)
                {
                    print("parentcolorstate: " + GetComponentInParent<BaseController>().mColorState);
                    print("colliderstateSuper: " + other.gameObject.GetComponent<LaserBoxController>().mColor);
                    Destroy(other.gameObject);
                }
            }

            if (other.gameObject.GetComponent<Projectile>() != null)
            {
                if (other.gameObject.GetComponent<Projectile>().myPaintState != GetComponentInParent<BaseController>().mColorState)
                {
                    print("parentcolorstate: " + GetComponentInParent<BaseController>().mColorState);
                    print("colliderstateBullet: " + other.gameObject.GetComponent<Projectile>().myPaintState);
                    Destroy(other.gameObject);
                }
            }
        }
    }
    */
}
