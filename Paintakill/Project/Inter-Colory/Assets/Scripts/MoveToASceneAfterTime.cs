using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;

public class MoveToASceneAfterTime : MonoBehaviour
{
    [HideInInspector] public Player player = null;

    [SerializeField] private float timeToWait = 0;
    [SerializeField] private int sceneNum = 0;

    [SerializeField] private int switchScene = 0;
    
    void Start()
    {
        if (timeToWait != 0)
            StartCoroutine("WaitForSomeTime", timeToWait);
    }

    private void Update()
    {
        /*
        for (int i = 0; i < 2; i++)
        {
            player = ReInput.players.GetPlayer(i);

            if (player.GetAxis(24) >= 0.25f)
            {
                
            }
        }
        */

        if (Input.GetKeyDown(KeyCode.S))
        {
            ChangeScene(switchScene);
        }
        
    }

    private IEnumerator WaitForSomeTime(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);

        ChangeScene(sceneNum);
    }

    public void ChangeScene(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum, LoadSceneMode.Single);
    }

}
