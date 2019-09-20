using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangIndTController : MonoBehaviour
{
    [SerializeField] private GameObject mIndicator = null;
    [SerializeField] private Transform mTarget = null;
    public Color mColor = Color.white;
    public bool isIndicOn = false;
    private LineRenderer mLine = null;
    private ColorManager mCM = null;
    private GameObject mGM = null;
    // Start is called before the first frame update
    void Start()
    {
        mLine = mIndicator.GetComponent<LineRenderer>();
        mLine.SetPosition(0, transform.position);
        mLine.SetPosition(1, transform.position);
        mGM = GameObject.Find("GameManager");
        mCM = mGM.GetComponent<ColorManager>();

        mIndicator.SetActive(false);
    }

    public void SetInitialVals(Transform target, Color color)
    {
        isIndicOn = true;
        mTarget = target;
        mColor = color;
        mLine.SetPosition(0, transform.position);
        if (mTarget != null)
        {
            mLine.SetPosition(1, mTarget.position);
            print("connect line");
        }
        mIndicator.GetComponent<SpriteRenderer>().color = mColor;
        mLine.startColor = mColor;
        mLine.endColor = mColor;
        mIndicator.SetActive(true);
    }

    public void TurnOffIndicators()
    {
        isIndicOn = false;
        mLine.SetPosition(0, transform.position);
        mLine.SetPosition(1, transform.position);
        mIndicator.GetComponent<SpriteRenderer>().color = Color.white;
        mColor = Color.white;
        mLine.startColor = Color.white;
        mLine.endColor = Color.white;
        mIndicator.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        if(isIndicOn)
        {
            mLine.SetPosition(0, transform.position);
            if (mTarget != null)
            {
                mLine.SetPosition(1, mTarget.position);
                //print("connect line");
            }
            mIndicator.GetComponent<SpriteRenderer>().color = mColor;
            mLine.startColor = mColor;
            mLine.endColor = mColor;
        }
        else
        {
            mLine.SetPosition(0, transform.position);
            mLine.SetPosition(1, transform.position);
            mIndicator.GetComponent<SpriteRenderer>().color = Color.white;
            mLine.startColor = Color.white;
            mLine.endColor = Color.white;
            mIndicator.SetActive(false);
        }
    }
}
