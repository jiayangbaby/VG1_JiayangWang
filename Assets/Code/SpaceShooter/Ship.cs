using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Ship : MonoBehaviour
{
    //Outlet
    public GameObject projectilePrefab;
    public Image imageHealthBar;
    public TMP_Text hullUpgradeText;
    public TMP_Text fireSpeedUpgradeText;
    public TMP_Text RepairHullText;

    //State Tracking
    public float firingDelay = 1f;
    public float healthMax = 100f;
    public float health = 100f;

    // Methods
    void Start()
    {
        StartCoroutine("FiringTimer");
    }
    void Update()
    {
        if (health > 0)
        {
            float yPosition = Mathf.Sin(GameController.instance.timeElapsed) * 3f;
            transform.position = new Vector2(0, yPosition);
        }
    }
    void Die() {
        StopCoroutine("FiringTimer");
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.bodyType = RigidbodyType2D.Dynamic;

    }
    void TakeDamage(float damageAmout) {
        health -= damageAmout;
        if (health <= 0) {
            Die();
        }
        imageHealthBar.fillAmount = health / healthMax;
    }
    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.GetComponent<Asteroid>()) {
            TakeDamage(10f);
        }    
    }
    public void RepairHull() {
        int cost = 100;
        if (GameController.instance.money > -cost && health < healthMax && health > 0) {
            GameController.instance.money -= cost;

            health = healthMax;
            imageHealthBar.fillAmount = health / healthMax;

            RepairHullText.text = "Repiar $" + Mathf.RoundToInt(healthMax);

        }

    }
    public void UpgradeHull() {
        int cost = Mathf.RoundToInt(healthMax);

        if (GameController.instance.money >= cost) {
            health += 50;
            healthMax += 50;
            imageHealthBar.fillAmount = health / healthMax;

            hullUpgradeText.text = "Hull Strength $" + Mathf.RoundToInt(healthMax);

        }

    }
    public void UpgradeFireSpeed() {
        int cost = 100 + Mathf.RoundToInt((1f- firingDelay)*100f);

        if (GameController.instance.money >= cost) {
            GameController.instance.money -= cost;
            firingDelay -= 0.05f;

            int newCost = 100 + Mathf.RoundToInt((1f-firingDelay));
            fireSpeedUpgradeText.text = "Fire Speed $" + newCost;


        }

    }
    void FireProjectile() {
        Instantiate(projectilePrefab, transform.position, Quaternion.identity);
   
    }
    IEnumerator FiringTimer()
    {
        //Wait
        yield return new WaitForSeconds(firingDelay);
        //Spawn
        FireProjectile();
        //Repeat
        StartCoroutine("FiringTimer");
    }
}
