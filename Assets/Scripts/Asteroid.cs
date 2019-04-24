using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{

    public float MaxThrust;
    public float MaxTorque;
    public Rigidbody2D AsteroidRB;

    //screen positions
    public float screenTop;
    public float screenBottom;
    public float screenLeft;
    public float screenRight;
    public int AsteroidSize; // 3 big, 2 medium , 1 small;
    public GameObject AsteroidMedium;
    public GameObject AsteroidSmall;
    
    //points
    public int Points;
    public GameObject Player;

    // GM
    public GameManagerScript gm;

    //explosion
    public GameObject Explosion;
    // Use this for initialization
    void Start()
    {
        // add random value torque & thrust;
        Vector2 Thurst = new Vector2(Random.Range(-MaxThrust, MaxThrust), Random.Range(-MaxThrust, MaxThrust));
        float Torque = Random.Range(-MaxTorque, MaxTorque);

        AsteroidRB.AddForce(Thurst);
        AsteroidRB.AddTorque(Torque);

        //find player
        Player = GameObject.FindWithTag("Player");

        //find GM
        gm = GameObject.FindObjectOfType<GameManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {

        //wrap asteroid
        Vector2 newPosition = transform.position;

        if (transform.position.y > screenTop)
        {
            newPosition.y = screenBottom;
        }
        if (transform.position.y < screenBottom)
        {
            newPosition.y = screenTop;
        }

        if (transform.position.x > screenRight)
        {
            newPosition.x = screenLeft;
        }
        if (transform.position.x < screenLeft)
        {
            newPosition.x = screenRight;
        }

        transform.position = newPosition;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet"))
        {
            // destroy bullet
            Destroy(collision.gameObject);
            Debug.Log("HIT by " + collision.gameObject.name);
            switch (AsteroidSize)
            {
                case 3:
                    {
                        Instantiate(AsteroidMedium, transform.position, transform.rotation);
                        Instantiate(AsteroidMedium, transform.position, transform.rotation);
                        gm.UpdateNumberOfAsteroids(1);
                        Destroy(gameObject);
                        
                        break;

                    }
                case 2:
                    {
                        Instantiate(AsteroidSmall, transform.position, transform.rotation);
                        Instantiate(AsteroidSmall, transform.position, transform.rotation);
                        gm.UpdateNumberOfAsteroids(1);

                        Destroy(gameObject);
                        break;
                    }
                case 1:
                    {
                        gm.UpdateNumberOfAsteroids(-1);
                        Destroy(gameObject);
                        break;
                    }
                default: break;

            }
            if (GameObject.FindWithTag("Player") != null)
            {
                Player.SendMessage("ScorePoints", Points);
            }
            GameObject NewExplosion = Instantiate(Explosion, transform.position, transform.rotation);
            Destroy(NewExplosion, 3f);
        }
    }
}
 