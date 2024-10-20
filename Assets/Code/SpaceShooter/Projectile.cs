using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    //Outlets
    Rigidbody2D _rb;
    //State Tracking
    Transform target;
    // Methods
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        
    }
    void ChooseNearstTarget() {
        //High defualt means first asteroid will always count as closet
        float closestDistance = 9999f;

        //Expenseive function. Correct appraoch would be object pooling
        Asteroid[] asteroids = FindObjectsOfType<Asteroid>();

        //Check each asteroid to see if it's the closet
        for (int i=0; i<asteroids.Length; i++)
        {
            Asteroid asteroid = asteroids[i];
            //Asteroid must be to our right;
            if (asteroid.transform.position.x > transform.position.x)
            {
                Vector2 directionToTarget = asteroid.transform.position - transform.position;

                //Filter for the closet target we've seen so far
                if (directionToTarget.sqrMagnitude < closestDistance)
                {
                    //Update closetst distance for future comparisons
                    closestDistance = directionToTarget.sqrMagnitude;

                    //Track this asteroid as the current closet target
                    target = asteroid.transform;
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //TODO: Make this dynamic
        float acceleration = 1f;
        float maxSpeed = 2f;

        //Home is on Target
        ChooseNearstTarget();
        if (target != null) {
            //Rotate towards target
            Vector2 directionToTarget = target.position - transform.position;
            float angle = Mathf.Atan2(directionToTarget.y, directionToTarget.x) * Mathf.Rad2Deg;

            _rb.MoveRotation(angle);
        }
        //Accelarate forward
        _rb.AddForce(transform.right*acceleration);
        //Cap max speed
        _rb.velocity = Vector2.ClampMagnitude(_rb.velocity, maxSpeed);
        
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //Only explode on Asteroids
        if (other.gameObject.GetComponent<Asteroid>())
        {
            Destroy(other.gameObject);
            Destroy(gameObject);

            //Create an explosion and destroy it soon after
            GameObject explosion = Instantiate(
                GameController.instance.explosionPrefab,
                transform.position,
                Quaternion.identity);
            Destroy(explosion, 0.25f);
        }
        
    }
}
