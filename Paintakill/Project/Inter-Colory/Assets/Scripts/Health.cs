using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Health : MonoBehaviour
{
    public int livesAtStart = 100;
    public int lives = 100;
    public float health = 3000f;
    public float maxHealth = 3000f;
    public bool isAlive = false;
    private Text healthTextObj = null;
    [SerializeField] private Slider healthBarObj = null;
    [SerializeField] private GameObject bluePlayer = null;
    [SerializeField] private GameObject redPlayer = null;
    [SerializeField] private MetricsManager metrics = null;
    [SerializeField] private FadeInOut fadeInOut = null;

    public GameObject healthBar = null;
    public GameObject amoBar = null;


    private void Awake()
    {
        lives = livesAtStart;
        isAlive = true;
        UpdateHealthBar();
        if (fadeInOut != null)
        {
            fadeInOut.FadeToClear(true);
        }
        metrics = GameObject.Find("GameManager").GetComponent<MetricsManager>();
    }


    public void ChangeHealth(float change)
    {
        float curHealth = health;
        curHealth += change;

        if (curHealth <= maxHealth)
        {
            health += change;
            UpdateHealthBar();

            if (isAlive == true && health <= 0)
            {
                print(gameObject.name + " Has been Killed!");
                lives -= 1;
                
                
                isAlive = false;
                ResetHealth();
                //add experience to another player when it gets killed by another player
                if(gameObject.tag == "Player")
                {
                    metrics.setDeaths(gameObject.GetComponent<PlayerController>().myPlayerID, 1);

                    if (gameObject.GetComponent<PlayerController>().myPaintState == ColorState.Blue)
                    {
                        redPlayer.GetComponent<ExpController>().AddExp(200);
                        metrics.setKills(1, 1);
                    }
                    else if (gameObject.GetComponent<PlayerController>().myPaintState == ColorState.Red)
                    {
                        bluePlayer.GetComponent<ExpController>().AddExp(200);
                        metrics.setKills(0, 1);
                    }

                    transform.GetChild(1).gameObject.SetActive(false);
                    print("TURNED OFF" + transform.GetChild(2).gameObject.name);
                    gameObject.GetComponent<PlayerController>().enabled = false;
                    gameObject.GetComponent<CapsuleCollider>().enabled = false;
                    if (GetComponentInChildren <GiantProjController>())
                    {
                        Destroy(GetComponentInChildren<GiantProjController>().gameObject);
                        print("destroy giangt proojjjjjsdjsjd");
                    }
                    healthBar.SetActive(false);
                    amoBar.SetActive(false);

                    if (fadeInOut != null)
                    {
                        fadeInOut.FadeToBlack(true);
                        print("Start fading to black");
                    }
                }
                else
                {
                    gameObject.SetActive(false);
                }
                

                
                
            }
        }
        else if (curHealth > maxHealth)
        {
            health = maxHealth;
            UpdateHealthBar();
        }
        else if (curHealth < 0)
        {
            curHealth = 0;
            UpdateHealthBar();
        }
    }


    void UpdateHealthBar()
    {
        healthBarObj.value = (health / maxHealth) * 100;
    }


    /*
    void UpdateHealthText()
    {
        healthTextObj.text = "Health: " + health.ToString();
    }*/


    public void ResetHealth()
    {
        ChangeHealth(maxHealth);

        if (fadeInOut != null)
        {
            fadeInOut.FadeToClear(true);
        }
    }
}
