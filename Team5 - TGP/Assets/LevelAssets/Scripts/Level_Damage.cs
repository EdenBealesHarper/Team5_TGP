using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Damage : MonoBehaviour
{
    [SerializeField]
    float deltDamage = 0.0f;

    private bool playerColliding;
    private GameObject Player;
    private Health playerHealth;
    private Transform playerTF;
    private Rigidbody2D playerRB;
    private BoxCollider2D boxCollide;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        playerHealth = Player.GetComponent<Health>();
        playerTF = Player.GetComponent<Transform>();
        playerRB = Player.GetComponent<Rigidbody2D>();

        boxCollide = this.GetComponent<BoxCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerColliding = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerColliding = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerColliding)
        {
            playerHealth.TakeDamage(deltDamage);
            playerRB.AddForce(new Vector2(300.0f * -playerTF.localScale.x, 0.0f));

            if (playerTF.position.y > boxCollide.offset.y + (0.5 * boxCollide.size.y))
            {
                Debug.Log("Juuuuuump");
                playerRB.AddForce(new Vector2(0.0f, 300.0f));
            }
        }
    }
}
