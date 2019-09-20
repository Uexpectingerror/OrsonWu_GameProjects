using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject[] towers;
    [SerializeField] private GameObject[] walls;
    [SerializeField] private GameObject[] playerObjs;
    [SerializeField] private PlayerController[] playerControllers;
    [SerializeField] private AudioClip[] audioClips;

    Player[] players = null;

    bool[] stop = { false, false, false };
    public bool[] wait = { false, false, false, false, false };

    int numOfPlayers = 2;
    AudioSource audioSource = null;

    private void Start()
    {
        playerControllers = new PlayerController[2];
        audioSource = GetComponent<AudioSource>();
        for (int i = 0; i < numOfPlayers; i++)
        {
            players[i] = ReInput.players.GetPlayer(i);
            playerControllers[i] = playerObjs[i].GetComponent<PlayerController>();
        }
    }


    void Update()
    {
        if (stop[2] == false)
        {
            for (int i = 0; i < numOfPlayers; i++)
            {
                if (towers[i].GetComponent<Tower>().isOwned == true)
                {
                    walls[i].SetActive(false);
                    stop[i] = true;
                }

            }

            if (stop[0] == true && stop[1] == true)
            {
                stop[2] = true;
                PlayOneShot(0);
            }
        }


        if(wait[2] == false)
        {
            for (int i = 0; i < numOfPlayers; i++)
            {
                if (playerObjs[i].GetComponent<ExpController>().curLvl >= 3)
                {
                    wait[i] = true;
                    print("Player: " + (i + 1) + " Reached LVL 3");
                }

            }


            if (wait[0] == true && wait[1] == true)
            {
                wait[2] = true;
                PlayOneShot(1);
            }
            
        }


        if (wait[3] == false)
        {
            for (int i = 0; i < numOfPlayers; i++)
            {
                if (wait[i] == true && players[i].GetButton("B_Button"))
                {
                    wait[3] = true;
                    PlayOneShot(2);
                }
            }


        }


        if (wait[4] == false)
        {
            GameObject hackySuperCheck = null;

            hackySuperCheck = GameObject.Find("Bullet");

            if (hackySuperCheck != null)
            {
                wait[4] = true;
                PlayOneShot(3);

            }

        }
        
    }


    public void PlayOneShot(int i)
    {
        audioSource.PlayOneShot(audioClips[i]);
    }


}
