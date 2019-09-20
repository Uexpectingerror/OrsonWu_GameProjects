using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public int myPlayerID = 0;
    [HideInInspector] public Material myMaterial = null;
    [HideInInspector] public float fireForce = 0;

    //Should just be assigned once in inspector instead of at runtime!!!!
    [HideInInspector] public ColorState myPaintState;


    [SerializeField] private GameObject hitPref = null;
    [SerializeField] private float damageAmount = 100f;
    [SerializeField] private float destroyTime = 2f;
    //[SerializeField] private GameObject paintSplatPref = null;

    [SerializeField] private MetricsManager metrics = null;

    private Rigidbody rb;


    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        myMaterial = gameObject.GetComponentInChildren<MeshRenderer>().material;
        metrics = GameObject.Find("GameManager").GetComponent<MetricsManager>();

        //check bullet paint state
        if (gameObject.layer == 8)
        {
            myPaintState = ColorState.Blue;
            myPlayerID = 0;
        }
        else
        {
            myPaintState = ColorState.Red;
            myPlayerID = 1;
        }

        Destroy(gameObject, destroyTime);

        rb.AddForce(gameObject.transform.forward * fireForce * 5, ForceMode.Impulse);

    }

    

    
    void OnTriggerEnter(Collider col)
    {
        GameObject hitObj = col.gameObject;

        if (hitObj != null)
        {
            

            if (hitObj.CompareTag("Tile"))
            {
                hitObj.GetComponent<Tile>().TakeProjectile(gameObject, this);
                metrics.setTilesPainted(myPlayerID, 1);
            }
            else if (hitObj.CompareTag("Tower"))
            {
                Instantiate(hitPref, transform.position, transform.rotation);
                hitObj.GetComponent<Tower>().TakeProjectile(gameObject, this);
                //metrics.setTimesTowerIsPainted(1, 1);
                Destroy(gameObject);
            }
            else if (hitObj.CompareTag("Player"))
            {
                Instantiate(hitPref, transform.position, transform.rotation);

                hitObj.GetComponent<PlayerController>().myHealth.ChangeHealth(-damageAmount);
                metrics.setPlayersPainted(myPlayerID, 1);

                Destroy(gameObject);
            }
            else if (hitObj.CompareTag("Shield"))
            {
                if (hitObj.gameObject.GetComponent<ShieldController>().mColorstate != myPaintState)
                {
                    Destroy(gameObject);
                }
            }
            else if (hitObj.CompareTag("Base"))
            {
                Instantiate(hitPref, transform.position, transform.rotation);
                col.gameObject.GetComponent<Health>().ChangeHealth(-damageAmount);
                //metrics.setTimesTowerIsPainted(1, 1);
                Destroy(gameObject);
            }
        }
        


        //Instantiate(paintSplatPref, transform.position, transform.rotation);
        ///TODO///
        //SpawnSplat

    }
}
