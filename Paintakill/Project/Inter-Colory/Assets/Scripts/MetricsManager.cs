using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class MetricsManager : MonoBehaviour
{
    [SerializeField] private int numOfPlayers = 2;
    
    
    //Kills
    //Deaths
    //KDA
    
    [HideInInspector] public int[] kills;
    [HideInInspector] public int[] deaths;


    //NumSupersUsed
    //GameLength
    //TimeIsSwimming

    [HideInInspector] public float gameLength = 0;
    [HideInInspector] public int[] supersUsed;
    [HideInInspector] public float[] timeSwimming;


    //NTowersPainted
    //NTilesPainted
    //NPlayersPainted

    [HideInInspector] public int[] towersPainted;
    [HideInInspector] public int[] tilesPainted;
    [HideInInspector] public int[] playersPainted;


    //NTTowerIsPainted

    [HideInInspector] public Dictionary< string, int> timesTowerIsPainted;
    [HideInInspector] public int towers = 12;


    //PlayerPositionData

    //[HideInInspector] public ArrayList[] playerPositionArrays;
    private void Awake()
    {
        kills = new int[numOfPlayers];
        deaths = new int[numOfPlayers];
        supersUsed = new int[numOfPlayers];
        timeSwimming = new float[numOfPlayers];
        towersPainted = new int[numOfPlayers];
        tilesPainted = new int[numOfPlayers];
        playersPainted = new int[numOfPlayers];
        timesTowerIsPainted = new Dictionary<string, int>();
    }

    void Start()
    {


    }


    public void setKills(int playerID, int valToAdd)
    {
        kills[playerID] += valToAdd;
    }

    public void setDeaths(int playerID, int valToAdd)
    {
        deaths[playerID] += valToAdd;
    }

    public void setSupersUsed(int playerID, int valToAdd)
    {
        supersUsed[playerID] += valToAdd;
    }

    public void setTimeSwimming(int playerID, float timeToAdd)
    {
        timeSwimming[playerID] += timeToAdd;
    }

    public void setTowersPainted(int playerID, int valToAdd)
    {
        towersPainted[playerID] += valToAdd;
    }

    public void setTilesPainted(int playerID, int valToAdd)
    {
        tilesPainted[playerID] += valToAdd;
    }

    public void setPlayersPainted(int playerID, int valToAdd)
    {
        playersPainted[playerID] += valToAdd;
    }

    public void setTimesTowerIsPainted(string towerName, int valToAdd)
    {
        if(timesTowerIsPainted.Count == 0)
        {
            timesTowerIsPainted.Add(towerName, valToAdd);
        }
        else if (timesTowerIsPainted.ContainsKey (towerName))
        {
            timesTowerIsPainted[towerName] += valToAdd;
        }
        else
        {
            timesTowerIsPainted.Add(towerName, valToAdd);
        }
    }


    private string ConvertMetricsToString()
    {
        string metrics = "Player Metrics:\n";

        //basic player data
        for(int i = 0; i < numOfPlayers; i++)
        {
            metrics += "\n";   
            metrics += "Player Number: " + i + "\n";
            metrics += "\tGame Length: " + gameLength.ToString() + "\n";
            metrics += "\tKills: " + kills[i].ToString() + "\n";
            metrics += "\tDeaths: " + deaths[i].ToString() + "\n";
            metrics += "\tSupers Used: " + supersUsed[i].ToString() + "\n";
            metrics += "\tTime In Paint: " + timeSwimming[i].ToString() + "\n";
            metrics += "\tTowers Painted: " + towersPainted[i].ToString() + "\n";
            metrics += "\tTiles Painted: " + tilesPainted[i].ToString() + "\n";
            metrics += "\tPlayers Painted: " + playersPainted[i].ToString() + "\n";
            //Do somethign with position data
        }

        //the tower data
        metrics += "\n";
        metrics += "Times Hhit Per Tower Data: " + "\n";
        foreach (string towerName in timesTowerIsPainted.Keys)
        {
            metrics += "\t" + towerName + ": " + timesTowerIsPainted[towerName] + "\n";
            //print(towerName + ": " + timesTowerIsPainted[towerName] + "\n");
        }

        return metrics;
    }


    private string CreateUniqueFileName()
    {
        string dateTime = System.DateTime.Now.ToString();
        dateTime = dateTime.Replace("/", "_");
        dateTime = dateTime.Replace(":", "_");
        dateTime = dateTime.Replace(" ", "___");
        return "Paintakill_metrics_" + dateTime + ".txt";
    }


    // Generate the report that will be saved out to a file.
    private void WriteMetricsToFile()
    {
        string totalReport = "Report generated on " + System.DateTime.Now + "\n\n";
        totalReport += "Total Report:\n";
        totalReport += ConvertMetricsToString();
        totalReport = totalReport.Replace("\n", System.Environment.NewLine);
        string reportFile = CreateUniqueFileName();

#if !UNITY_WEBPLAYER
        File.WriteAllText(reportFile, totalReport);
#endif
        print(totalReport);

    }


    private void OnApplicationQuit()
    {
        WriteMetricsToFile();
    }
}

