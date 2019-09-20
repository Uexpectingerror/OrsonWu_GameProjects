using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamepadDebug : MonoBehaviour
{
    [SerializeField] private int gamepadNum = 1;

    [SerializeField] private Text joysticknum = null;
    [SerializeField] private Text curLT = null;
    [SerializeField] private Text curRT = null;
    [SerializeField] private Text curLB = null;
    [SerializeField] private Text curRB = null;
    [SerializeField] private Text curA_Button = null;
    [SerializeField] private Text curB_Button = null;
    [SerializeField] private Text curX_Button = null;
    [SerializeField] private Text curY_Button = null;
    [SerializeField] private Text curBack = null;
    [SerializeField] private Text curStart = null;
    [SerializeField] private Text curL_Horizontal = null;
    [SerializeField] private Text curL_Vertical = null;
    [SerializeField] private Text curR_Horizontal = null;
    [SerializeField] private Text curR_Vertical = null;
    [SerializeField] private Text curL_Button = null;
    [SerializeField] private Text curR_Button = null;
    [SerializeField] private Text curDPAD_Horizontal = null;
    [SerializeField] private Text curDPAD_Vertical = null;

    private void Start()
    {
        string[] controllers = Input.GetJoystickNames();
        for (int i = 0; i < controllers.Length; i++)
        {
            print(i + " " + controllers[i]);
        }
        
    }


    void Update()
    {
        joysticknum.text = "JoystickNum: " + gamepadNum;
        curLT.text = "CurLT: " + Input.GetAxis("LT" + gamepadNum);
        curRT.text = "CurRT: " + Input.GetAxis("RT" + gamepadNum);
        curLB.text = "CurLB: " + Input.GetButton("LB" + gamepadNum);
        curRB.text = "CurRB: " + Input.GetButton("RB" + gamepadNum);
        curA_Button.text = "CurA_Button: " + Input.GetButton("A_button" + gamepadNum);
        curB_Button.text = "CurB_Button: " + Input.GetButton("B_button" + gamepadNum);
        curX_Button.text = "CurX_Button: " + Input.GetButton("X_button" + gamepadNum);
        curY_Button.text = "CurY_Button: " + Input.GetButton("Y_button" + gamepadNum);
        curBack.text = "CurBack: " + Input.GetButton("back" + gamepadNum);
        curStart.text = "CurStart: " + Input.GetButton("start" + gamepadNum);
        curL_Horizontal.text = "CurL_Horizontal: " + Input.GetAxis("L_horizontal" + gamepadNum);
        curL_Vertical.text = "CurL_Vertical: " + Input.GetAxisRaw("L_vertical" + gamepadNum);
        curR_Horizontal.text = "CurR_Horizontal: " + Input.GetAxis("R_horizontal" + gamepadNum);
        curR_Vertical.text = "CurR_Vertical: " + Input.GetAxis("R_vertical" + gamepadNum);
        curL_Button.text = "CurL_Button: " + Input.GetButton("L_button" + gamepadNum);
        curR_Button.text = "CurR_Button: " + Input.GetButton("R_button" + gamepadNum);
        curDPAD_Horizontal.text = "CurDPAD_Horizontal: " + Input.GetAxis("DPAD_horizontal" + gamepadNum);
        curDPAD_Vertical.text = "CurDPAD_Vertical: " + Input.GetAxis("DPAD_vertical" + gamepadNum);
    }

}
