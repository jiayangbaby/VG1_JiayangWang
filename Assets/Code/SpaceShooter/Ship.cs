using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship : MonoBehaviour
{
    //Outlet
    public GameObject projectilePrefab;
    //State Tracking
    public float firingDelay = 1f;

    // Methods
    void Start()
    {
        StartCoroutine("FiringTimer");
    }
    void Update()
    {
        float yPosition = Mathf.Sin(GameController.instance.timeElapsed) * 3f;
        transform.position = new Vector2(0, yPosition);
    }
    void FireProjectile() {
        Instantiate(projectilePrefab, transform.position, Quaternion.identity);
   
    }
    IEnumerator FiringTimer()
    {
        //Wait
        yield return new WaitForSeconds(firingDelay);
        //Spawn
        FireProjectile();
        //Repeat
        StartCoroutine("FiringTimer");
    }
}
