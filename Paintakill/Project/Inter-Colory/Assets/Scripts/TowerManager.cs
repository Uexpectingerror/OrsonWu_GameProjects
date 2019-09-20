using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    //public GameObject[] mapTowersA;
    public List<GameObject> mapTowers = new List<GameObject>();
    public List<GameObject> players = new List<GameObject>();

    bool hold = false;
    float wait = 0.25f;

    private void Start()
    {
        mapTowers.AddRange(GameObject.FindGameObjectsWithTag("Tower"));
        players.AddRange(GameObject.FindGameObjectsWithTag("Player"));
    }


    private void Update()
    {

        if (hold == false)
        {
            StartCoroutine("TimerToCheckTowersOwned");
        }
    }

    IEnumerator TimerToCheckTowersOwned()
    {
        hold = true;
        yield return new WaitForSeconds(wait);
        for(int i = 0; i < players.Capacity; i++)
        {
            players[i].SendMessage("CheckTowersOwned");
        }
        hold = false;
    }


}
