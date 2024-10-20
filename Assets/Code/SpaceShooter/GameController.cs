using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance;
    //Outlets
    public Transform[] spawnPoints;
    public GameObject[] asteroidPrefabs;
    public GameObject explosionPrefab;
    //Configuration
    public float maxAsteroidDelay = 2f;
    public float minAsteroidDelay = 0.2f;
 
    //State Tracking
    public float timeElapsed;
    public float asteroidDelay;

    // Methods
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        StartCoroutine("AsteroidSpawnTimer");
    }

    void Update()
    {   //increment passage of time for each frame of the game
        timeElapsed += Time.deltaTime;
        //Computer Asteroid Delay
        float decreaseDelayOverTime = maxAsteroidDelay - ((maxAsteroidDelay-minAsteroidDelay)/30f*timeElapsed);
        asteroidDelay = Mathf.Clamp(decreaseDelayOverTime, minAsteroidDelay, maxAsteroidDelay);
        
    }
    void SpawnAsteroid() {
        //Pick random spawn points and random asteroid prefabs
        int randomSpawnIndex = Random.Range(0, spawnPoints.Length);
        Transform randomSpawnPoint = spawnPoints[randomSpawnIndex];
        int randomAsteroidIndex = Random.Range(0, asteroidPrefabs.Length);
        GameObject randomAsteroidPrefab = asteroidPrefabs[randomAsteroidIndex];
        //Spawn
        Instantiate(randomAsteroidPrefab, randomSpawnPoint.position, Quaternion.identity);
    }
    IEnumerator AsteroidSpawnTimer() {
        //Wait
        yield return new WaitForSeconds(asteroidDelay);
        //Spawn
        SpawnAsteroid();
        //Repeat
        StartCoroutine("AsteroidSpawnTimer");
    }


}
