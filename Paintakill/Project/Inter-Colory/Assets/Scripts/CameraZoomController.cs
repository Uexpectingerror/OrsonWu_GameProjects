using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomController : MonoBehaviour
{
    [SerializeField] private float zoomLastTime = 0.3f;
    [SerializeField] private float zoomInTime = 0.3f;
    [SerializeField] private float zoomOutTime = 0.3f;

    [SerializeField] private float superFOV = 40;
    [SerializeField] private float ultraSuFOV = 55;
    private float t = 0;
    float defaultFov = 0;
    private float targetFOV = 0;
    bool isZoomOut = false;
    // Start is called before the first frame update
    private void OnDisable()
    {
        gameObject.GetComponent<Camera>().fieldOfView = defaultFov;
    }
    void Start()
    {
        defaultFov = gameObject.GetComponent<Camera>().fieldOfView;
    }

    public void ZoomOutCamera(int type)
    {
        StopCoroutine("CameraZoom");
        print("zozozozozozozozo");
        if(type == 0)
        {
            targetFOV = superFOV;
        }
        else
        {
            targetFOV = ultraSuFOV;
        }
        StartCoroutine("CameraZoom");
    }

    private IEnumerator CameraZoom ()
    {
        if(gameObject.GetComponent<Camera>().fieldOfView < targetFOV)
        {
            print("zoom out");
            while (gameObject.GetComponent<Camera>().fieldOfView < targetFOV)
            {
                float f = t / zoomOutTime;
                //if f > 1, it has to be stopped at 1 or the f value will go back down instead of staying above one 
                //a slow down lerp function 
                if (f>1)
                {
                    f = 1;
                }
                else 
                {
                    f = -1 * (f - 1) * (f - 1) + 1;
                }
                //print("shieldactiveT: " + t + " f: " + f);
                float curFov = Mathf.Lerp(defaultFov, targetFOV, f);
                gameObject.GetComponent<Camera>().fieldOfView = curFov;
                //print("f:" + f);
                yield return null;
                t += Time.deltaTime;
            }
            print("zoomout time:" + t);
            t = 0.0f;
            gameObject.GetComponent<Camera>().fieldOfView = targetFOV;
            isZoomOut = true;
        }
        else
        {
            print("zoom in");
            while (gameObject.GetComponent<Camera>().fieldOfView > defaultFov)
            {
                print("zsdfsfdoom in");
                float f = t / zoomInTime;
                //if f > 1, it has to be stopped at 1 or the f value will go back down instead of staying above one 
                if (f > 1)
                {
                    f = 1;
                }
                else
                {
                    f = -1 * (f - 1) * (f - 1) + 1;
                }

                //print("shieldactiveT: " + t + " f: " + f);
                float curFov = Mathf.Lerp(targetFOV, defaultFov, f);
                gameObject.GetComponent<Camera>().fieldOfView = curFov;

                yield return null;
                t += Time.deltaTime;
            }
            print("zoomin time:" + t);
            t = 0.0f;
            gameObject.GetComponent<Camera>().fieldOfView = defaultFov;
        }

        //if just zoomed out it will run this below to zoom in back 
        if (isZoomOut)
        {
            print("prepare zoom back in");
            //zoom wait time 
            yield return new WaitForSeconds(zoomLastTime);
            print("waited a time");
            //make sure it is false so that i wont be a infinite loop
            isZoomOut = false;
           
            StartCoroutine("CameraZoom");
            print("zoomed back in");
            isZoomOut = false;
        }
    }


}
