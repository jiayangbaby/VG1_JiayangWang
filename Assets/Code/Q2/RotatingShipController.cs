using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Q2 {
    public class RotatingShipController : MonoBehaviour
    {
        //Outlets
        Rigidbody2D _rb;
        //Configuration
        public float speed;
        public float RotationSpeed;
        // Methods
        void Start() {
            _rb = GetComponent<Rigidbody2D>();
        }
        void Update() {
            //Turn Left
            if (Input.GetKey(KeyCode.LeftArrow)) {
                _rb.AddTorque(RotationSpeed*Time.deltaTime);
            }
            //Turn Right
            if (Input.GetKey(KeyCode.RightArrow)) {
                _rb.AddTorque(-RotationSpeed*Time.deltaTime);
            }
            //Thrust Forward 
            if (Input.GetKey(KeyCode.Space)) {
                //Right as Forward, bcause the sapceship faces right
                _rb.AddRelativeForce(Vector2.right*speed*Time.deltaTime);
            }
        }
    }
}