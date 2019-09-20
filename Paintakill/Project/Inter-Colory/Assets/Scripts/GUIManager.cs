using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Rewired;

public class GUIManager : MonoBehaviour
{

    public void ChangeScene(int sceneNum)
    {
        SceneManager.LoadScene(sceneNum, LoadSceneMode.Single);
    }

    public void ChangeScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Application.Quit();
    }


}
