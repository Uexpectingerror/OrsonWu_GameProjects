using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class ExpController : MonoBehaviour
{
    [SerializeField] private int Lvl1Exp = 300;
    [SerializeField] private int Lvl2Exp = 400;
    [SerializeField] private int Lvl3Exp = 400;
    [SerializeField] private int Lvl4Exp = 400;

    [SerializeField] private int curExp = 0;
    [SerializeField] private int curLvlExp = 1;

    public int curLvl = 1;
    [SerializeField] private Fire mFireScr;
    [SerializeField] private SuperAttackController mSupScr;

    [SerializeField] private Text levelText;
    [SerializeField] private Slider expBar;


    // Start is called before the first frame update
    void Start()
    {
        mFireScr = gameObject.GetComponentInChildren<Fire>();
        mSupScr = gameObject.GetComponentInChildren<SuperAttackController>();
        switch (curLvl)
        {
            case 1:
                curLvlExp = Lvl1Exp;
                break;
            case 2:
                curLvlExp = Lvl2Exp;
                break;
            case 3:
                curLvlExp = Lvl3Exp;
                break;
            case 4:
                curLvlExp = Lvl4Exp;
                break;
            default:
                break;
        }

        UpdateUI();
    }

    private void UpdateUI()
    {
        levelText.text = curLvl.ToString();
        expBar.maxValue = curLvlExp;
        expBar.value = curExp;
    }

    public void AddExp(int exp)
    {
        curExp += exp;
        switch (curLvl)
        {
            case 1:
                curLvlExp = Lvl1Exp;
                break;
            case 2:
                curLvlExp = Lvl2Exp;
                break;
            case 3:
                curLvlExp = Lvl3Exp;
                break;
            case 4:
                curLvlExp = Lvl4Exp;
                break;
            default:
                break;
        }

        if (curExp >= curLvlExp && curLvl < 5)
        {
            curLvl++;
            int expRemain = curExp - curLvlExp;
            curExp = expRemain;

            //applyging corresponding effect
            switch (curLvl)
            {
                case 1:

                    break;
                case 2:
                    mFireScr.IncreMaxPaint(3);
                    break;
                case 3:
                    mSupScr.isSuperUnclocked = true;
                    break;
                case 4:
                    mFireScr.IncreMaxPaint(6);
                    break;
                case 5:
                    mSupScr.isUltraSuperUnlocked = true;
                    break;
                default:
                    break;
            }
        }

        UpdateUI();
    }

}
