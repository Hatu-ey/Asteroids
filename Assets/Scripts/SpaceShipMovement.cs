using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SpaceShipMovement : MonoBehaviour {

    // spaceship movement
    public Rigidbody2D rb;
    public float Thrust;
    public float turnThrust;
    private float thrustInput;
    private float turnInput;
    public float maxVelocity;
    private bool DisablePlayerInput;
    //screen positions

    public float screenTop;
    public float screenBottom;
    public float screenLeft;
    public float screenRight;

    // Bullets 
    public GameObject Bullet;
    public float BulletForce;
    


    // HUD
    private int Score;
    private int lives;
    public Color defaultColor;
    public Color NewColor;
    public GameObject GameOverPanel; 

    // SFX
    public AudioSource SpaceShipFX;
    public AudioClip MoveFX;
    public GameObject Explosion;

    //Display text
    public Text ScoreText;
    public Text LivesText;

    // render, colider
    public SpriteRenderer spriter;
    public Collider2D col2d;

    //hyper space boolean
    private bool hyperspacing;

	// Use this for initialization
	void Start () {
        Score = 0;
        lives = 3;
        ScoreText.text = "Score " + Score;
        LivesText.text = "Lives " + lives;
	}

    // Update is called once per frame
    void Update () {

        //movement input
        thrustInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
        

        //input fire1, fire bullets
        if(Input.GetButtonDown("Fire1") && !DisablePlayerInput)
        {
            
            GameObject newBullet = Instantiate(Bullet, transform.position, transform.rotation);
            newBullet.GetComponent<Rigidbody2D>().AddRelativeForce(Vector2.up * BulletForce);
            Destroy(newBullet, 6.0f);
        }

        // rotate ship
        transform.Rotate(Vector3.forward * turnInput * Time.deltaTime * -turnThrust);

        // limit speed
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, maxVelocity);

        // wrap ship
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

        // play music on move
        if (rb.velocity.magnitude >= 1.5f && !SpaceShipFX.isPlaying)
        {
            SpaceShipFX.PlayOneShot(MoveFX);
        }

        // hyperspace
        if (Input.GetButtonDown("HyperSpace") && !hyperspacing)       
        {
            DisablePlayerInput = true;
            hyperspacing = true;
            spriter.enabled = false;
            col2d.enabled = false;
            Invoke("Hyperspace",1f);
            
        }
        
    }

    private void FixedUpdate()
    {
        rb.AddRelativeForce(Vector2.up * thrustInput);
        
    }

    void ScorePoints(int PointsToAdd)
    {
        Score += PointsToAdd;
        ScoreText.text = "Score " + Score;
        Debug.Log("Score: " + Score);
        

    }

    void Respawn()
    {
        
        rb.velocity = Vector3.zero;
        transform.position = Vector3.zero;
        spriter.enabled = true;
        spriter.color = NewColor;
        Invoke("Invulnerable", 3f);
        
    }

    void Invulnerable()
    {
        col2d.enabled = true;
        spriter.color = defaultColor;
        DisablePlayerInput = false;
    }

    void Hyperspace()
    {
        Vector2 NewPos = new Vector2(Random.Range(-13.14f, 13.14f),Random.Range(-6.99f, 6.99f));
        transform.position = NewPos;
        spriter.enabled = true;
        col2d.enabled = true;
        hyperspacing = false;
        DisablePlayerInput = false;
    }
    void LoseLife()
    {                  
            Debug.Log("Death");
            lives--;
            LivesText.text = "Lives: " + lives;
            // Explosion
            GameObject NewExplosion = Instantiate(Explosion, transform.position, transform.rotation);
            Destroy(NewExplosion, 3f);
            spriter.enabled = false;
            col2d.enabled = false;
            DisablePlayerInput = true;
            Invoke("Respawn",3f);
            
            if (lives <=0)
            {
                GameOver();
            }     
    }

    void GameOver()
    {
        CancelInvoke();
        GameOverPanel.SetActive(true);
        Destroy(gameObject);
       
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        LoseLife();   
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag ("AlienBullet"))
        {
            LoseLife();
            Destroy(other.gameObject);
        }
            
        
    }
}
