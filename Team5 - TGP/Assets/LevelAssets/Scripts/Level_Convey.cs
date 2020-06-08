using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Convey : MonoBehaviour
{
    [SerializeField]
    bool Left;

    [SerializeField]
    float conveyForce = 50.0f;

    private bool playerColliding;
    private GameObject Player;
    private Rigidbody2D playerRB;
    // Start is called before the first frame update
    void Start()
    {
       Player = GameObject.FindGameObjectWithTag("Player");
       playerRB = Player.GetComponent<Rigidbody2D>();
    }


    void OnTriggerStay2D(Collider2D other)
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
            if (Left)
            {
                //Debug.Log("Player move left");
                playerRB.AddForce(new Vector2(-conveyForce, 0.0f));
            }
            else if (!Left)
            {
                //Debug.Log("Player move right");
                playerRB.AddForce(new Vector2(conveyForce, 0.0f));
            }
        }
    }
}
