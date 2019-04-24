using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienScript : MonoBehaviour {

    // alien ship movement
    public Rigidbody2D rb;
    public Vector2 direction;
    public float speed;
    public bool disabled;

    // alien ship render
    public SpriteRenderer spriteRenderer;
    public Collider2D col2D;

    // alien ship shooting
    public float DelayShoot;
    public float LastShoot = 0f;
    public float BulletSpeed;
    public GameObject Bullet;

    // alien explosion
    public GameObject Explosion;
    public int points;
    
    // find player
    public Transform player;


	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindWithTag("Player").transform;

	}
    private void Update()
    {
        if (disabled)
        {
            return;
        }
        if(Time.time > LastShoot + DelayShoot)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg -90.0f;
            Quaternion q = Quaternion.AngleAxis(angle, Vector3.forward);

            GameObject newBullet = Instantiate(Bullet, transform.position, q);
            newBullet.GetComponent<Rigidbody2D>().AddRelativeForce(new Vector2(0f, BulletSpeed));
            Destroy(newBullet, 6.0f);
            LastShoot = Time.time;

        }
    }
    // Update is called once per frame
    void FixedUpdate () {
        
        if (disabled)
        {
            return;
        }
        if (GameObject.FindWithTag("Player") != null)
        {
            direction = (player.position - transform.position).normalized;
            rb.MovePosition(rb.position + direction * speed * Time.deltaTime);
        }
        
       
	}
    void Respawn()
    {
        Vector2 NewPos = new Vector2(Random.Range(-13.14f, 13.14f), Random.Range(-6.99f, 6.99f));
        transform.position = NewPos;
        disabled = false;
        col2D.enabled = true;
        spriteRenderer.enabled = true;
    }
    void Disable()
    {
        disabled = true;
        col2D.enabled = false;
        spriteRenderer.enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Bullet"))
        {
            player.SendMessage("ScorePoints", points);
            GameObject newExplosion = Instantiate(Explosion, transform.position, transform.rotation);
            Destroy(newExplosion, 3f);
            Disable();
            Invoke("Respawn", 3f);
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject newExplosion = Instantiate(Explosion, transform.position, transform.rotation);
        Destroy(newExplosion, 3f);
        Disable();
        Invoke("Respawn", 3f);

    }
}
