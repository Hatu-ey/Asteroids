using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerScript : MonoBehaviour {

    public int AsteroidsCounter;
    private int spawner = 1;
    public GameObject AsteroidLarge;

    public void Start()
    {
        AsteroidCount();
    }

    private void Update()
    {     
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }      
    }
    void AsteroidCount()
    {
        AsteroidsCounter = GameObject.FindGameObjectsWithTag("Asteroid").Length;
    }
    public void UpdateNumberOfAsteroids(int change)
    {
        AsteroidsCounter += change;
        if(AsteroidsCounter <= 0)
        {
            Invoke("AddAsteroids", 3f);
        }
    }

    void AddAsteroids()
    {
        spawner++;

        for(int i = 0; i< spawner ; i++)
        {
            Vector2 spawnPosition = new Vector2(Random.Range(-15, 15),8);
            Instantiate(AsteroidLarge, spawnPosition, Quaternion.identity);
            
        }
        AsteroidCount();
    }
}
