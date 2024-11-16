using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Platformer {
    public class PlayerController : MonoBehaviour
    {
        public static PlayerController instance;

        //Outlet
        Rigidbody2D _rigidbody2D;
        public Transform aimPivot;
        public GameObject projectilePrefab;
        SpriteRenderer sprite;
        Animator animator;
        public TMP_Text scoreUI;

        //State Tracking
        public int jumpsLeft;
        public int score;
        public bool isPaused;

        //Methods
        private void Awake()
        {
            instance = this;
        }
        void Start()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            sprite = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();

            score = PlayerPrefs.GetInt("Score");
        }
        void FixedUpdate()
        {
            //This Update Event is sync'd with the Physics Engine
            animator.SetFloat("Speed",_rigidbody2D.velocity.magnitude);
            if (_rigidbody2D.velocity.magnitude > 0)
            {
                animator.speed = _rigidbody2D.velocity.magnitude / 3f;
            }
            else {
                animator.speed = 1f;
            }
            
            
        }

        void Update()
        {
            //Update UI
            scoreUI.text = score.ToString();
            //Update loop will be skipped if the game is paused
            if (isPaused) {
                return;
            }
            //Move Player Left
            if (Input.GetKey(KeyCode.A)){
                _rigidbody2D.AddForce(Vector2.left * 18 * Time.deltaTime, ForceMode2D.Impulse);
                sprite.flipX = true;
            }
            //Move Player Right
            if (Input.GetKey(KeyCode.D))
            {
                _rigidbody2D.AddForce(Vector2.right * 18 * Time.deltaTime, ForceMode2D.Impulse);
                sprite.flipX=false;
            }
            //Escape key will trigger showing the menu
            if (Input.GetKeyDown(KeyCode.Escape)) {
                MenuController.instance.Show();
            }
            //Aim Toward Mouse
            Vector3 mousePosition = Input.mousePosition;
            Vector3 mousePositionInWorld =Camera.main.ScreenToWorldPoint(mousePosition);
            Vector3 directionFormPlayerToMouse = mousePositionInWorld - transform.position;

            float radiansToMouse = Mathf.Atan2(directionFormPlayerToMouse.y, directionFormPlayerToMouse.x);
            float angleToMouse = radiansToMouse * Mathf.Rad2Deg;

            aimPivot.rotation = Quaternion.Euler(0,0,angleToMouse);

            //Shoot
            if (Input.GetMouseButtonDown(0)) {
                GameObject newProjectile = Instantiate(projectilePrefab);
                newProjectile.transform.position = transform.position;
                newProjectile.transform.rotation = aimPivot.rotation;
            }
            //Jump
            if (Input.GetKeyDown(KeyCode.Space)) {
                if (jumpsLeft > 0) {
                    jumpsLeft--;
                    _rigidbody2D.AddForce(Vector2.up * 7f, ForceMode2D.Impulse);

                }
            }
            animator.SetInteger("JumpsLeft", jumpsLeft);

        }
        void OnCollisionStay2D(Collision2D other)
        {
            //Check that we collided with the ground
            if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            {
                //Check what is directly below our character's feet
                RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, 0.9f);//charater is taller
                //RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, 0.7f);
                //we might have multiple things below the player's feet
                for (int i = 0; i < hits.Length; i++)
                {
                    RaycastHit2D hit = hits[i];
                    //Check if we collided with ground below our feet
                    if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Ground"))
                    {
                        //Reset Jump count
                        jumpsLeft = 2;
                    }
                }
            }
        }
        public void ResetScore() {
            score = 0;
            PlayerPrefs.DeleteKey("Score");
        }


    }
}

