using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    [SerializeField] private const int numOfPlayers = 2;
    [SerializeField] private int winner;
    [SerializeField] private AudioClip winSting = null;
    [SerializeField] private MusicLayerManager layerManager = null;
    

    [SerializeField] private GameObject[] winAlert = null;
    [SerializeField] private GameObject[] baseObj = null;
    
    private RespawnManager respManager = null;

    private AudioSource audioSource = null;

    void Start()
    {
        respManager = GetComponent<RespawnManager>();
        audioSource = GetComponent<AudioSource>();
    }

    public void GameOver()
    {
        layerManager.Silence();
        
        for(int i = 0; i < 2; i++)
        {
            if (baseObj[i].activeInHierarchy == true)
            {
                winner = i;
                print("Winner: " + winner);
            }
        }

        winAlert[winner].SetActive(true);

        audioSource.PlayOneShot(winSting);

        respManager.StartCoroutine("EndMatch");
    }


}
