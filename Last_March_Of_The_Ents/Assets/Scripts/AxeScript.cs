using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeScript : MonoBehaviour
{
    public float speedToMove = 1f;
    public float speed = 1.0f;
    public float amplitude = 1.0f;
    public Vector2 targetPosition;

    private Rigidbody2D rb2d;
    private bool canMove = true;
    public float forceToHitAway;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        targetPosition = new Vector2(0, transform.position.y);
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            float x = Time.time * speed;
            float y = amplitude * Mathf.Sin(x);
            Vector2 sineWaveMovement = new Vector2(0, y);
            Vector2 newPosition = Vector2.MoveTowards(rb2d.position, targetPosition, Time.fixedDeltaTime * speedToMove);
            newPosition += sineWaveMovement;
            rb2d.MovePosition(newPosition);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6) //Hand
        {
            canMove = false;

            Vector2 direction = rb2d.position - (Vector2)collision.transform.position;
            rb2d.AddForce(direction * forceToHitAway);
            GameControllerScript.numOfAxesBlocked++;

            GetComponent<Collider2D>().enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8) //Body
        {
            GameControllerScript.isHit = true;
            Destroy(this);
        }
    }
}
