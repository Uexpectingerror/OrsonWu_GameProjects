using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RespawnManager : MonoBehaviour
{
    [SerializeField] private const int numOfPlayers = 2;


    private GameObject[] players;
    private PlayerController[] playerControllers;
    private Transform[] playerSpawns;
    private bool[] respawning;

    bool gameOver = false;

    void Start()
    {
        players = new GameObject[numOfPlayers];
        playerControllers = new PlayerController[numOfPlayers];
        playerSpawns = new Transform[numOfPlayers];
        respawning = new bool[numOfPlayers];

        for(int i = 0; i < numOfPlayers; i++)
        {
            players[i] = GameObject.Find("Player " + (i + 1));
            playerControllers[i] = players[i].GetComponent<PlayerController>();
            playerSpawns[i] = GameObject.Find("Spawn " + (i + 1)).transform;
            respawning[i] = false;
        }

    }


    private void Update()
    {
        for (int i = 0; i < players.Length; i ++)
        {
            if (respawning[i] == false && playerControllers[i].myHealth.isAlive == false && playerControllers[i].myHealth.lives >= 0)
            {
                StartCoroutine("RespawnTimer", i);
            }

            if (gameOver == false && playerControllers[i].myHealth.lives <= 0)
            {
                gameOver = true;
                SendMessage("GameOver");
                StartCoroutine("EndMatch");
            }
        }

    }

    public void RespawnCheck(int lives, int i)
    {
        int playerLivesAtStart = players[i].GetComponent<Health>().livesAtStart;

        if (lives >= (0.98f * playerLivesAtStart))
        {
            playerControllers[i].respawnTime = 2;
        }
        else if (lives < (0.98f * playerLivesAtStart) && lives >= (0.9f * playerLivesAtStart))
        {
            playerControllers[i].respawnTime = 4;
        }
        else if (lives < (0.9f * playerLivesAtStart) && lives >= (0.8f * playerLivesAtStart))
        {
            playerControllers[i].respawnTime = 6;
        }
        else if (lives < (0.8f * playerLivesAtStart) && lives >= (0.7f * playerLivesAtStart))
        {
            playerControllers[i].respawnTime = 8;
        }
        else if (lives < (0.7f * playerLivesAtStart) && lives >= 0)
        {
            playerControllers[i].respawnTime = 10;
        }
    }


    public void ResetPos(int i)
    {
        players[i].transform.position = playerSpawns[i].transform.position;
    }


    IEnumerator RespawnTimer(int i)
    {
        respawning[i] = true;
        playerControllers[i].respawnManager.RespawnCheck(playerControllers[i].myHealth.lives, playerControllers[i].myPlayerID);
        players[i].GetComponent<Rigidbody>().isKinematic = true;
        playerControllers[i].respawnManager.ResetPos(playerControllers[i].myPlayerID);

        yield return new WaitForSeconds(playerControllers[i].respawnTime);

        playerControllers[i].myHealth.healthBar.SetActive(true);
        playerControllers[i].myHealth.amoBar.SetActive(true);
        players[i].transform.GetChild(1).gameObject.SetActive(true);
        players[i].GetComponent<CapsuleCollider>().enabled = true;
        playerControllers[i].enabled = true;
        playerControllers[i].RespawnReset();
        players[i].GetComponent<Rigidbody>().isKinematic = false;
        respawning[i] = false;
    }


    public IEnumerator EndMatch()
    {
        yield return new WaitForSeconds(4f);

        for (int i = 0; i < players.Length; i++)
        {
            if (playerControllers[i].enabled == true)
            {
                playerControllers[i].fadeInOut.FadeToBlack(true);
            }
        }

        yield return new WaitForSeconds(4f);
        gameObject.GetComponent<GUIManager>().ChangeScene(0);
    }


}
