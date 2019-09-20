using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicLayerManager : MonoBehaviour
{
    [SerializeField] private AudioMixerSnapshot[] musicSnapshots;
    [SerializeField] private float fadeTime = 2f;
    [SerializeField] private float delayTime = 0f;
    [SerializeField] private int numOfPlayers = 2;
    [SerializeField] private int levelNeededToSwitch = 3;
   


    public ExpController[] expControllers;
    public GameObject[] players;

    bool addLayer = false;
    bool halt = false;

    private void Start()
    {
        musicSnapshots[0].TransitionTo(fadeTime);
        players = new GameObject[numOfPlayers];
        expControllers = new ExpController[numOfPlayers];
        for (int i = 0; i < numOfPlayers; i++)
        {
            players[i] = GameObject.Find("Player " + (i + 1));
            expControllers[i] = players[i].GetComponent<ExpController>();
        }
        

        
    }

    private void Update()
    {
        if (halt == false)
        {
            if (addLayer == false)
            {
                for (int i = 0; i < numOfPlayers; i++)
                {
                    if (expControllers[i].curLvl >= levelNeededToSwitch)
                    {
                        musicSnapshots[1].TransitionTo(fadeTime);
                        addLayer = true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < numOfPlayers; i++)
                {
                    if (expControllers[i].curLvl >= levelNeededToSwitch + 2)
                    {
                        musicSnapshots[2].TransitionTo(fadeTime);
                        halt = true;
                    }
                }

            }
        }
    }


    public void Silence()
    {
        musicSnapshots[3].TransitionTo(fadeTime * 3);
        musicSnapshots[4].TransitionTo(fadeTime * 2);
        musicSnapshots[5].TransitionTo(fadeTime);
    }



}
