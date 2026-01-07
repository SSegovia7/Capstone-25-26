using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyController : MonoBehaviour
{
    public float health = 3f;
    public float speed = 2f;
    public float powerUpAmountDamage = 2f;
    public float powerUpAmountKill = 5f;
    public float boxPickDelay = 0.5f;
    private float boxPickUpTimer = 0f;
    public bool powerUpIncrase = false;
    public bool boxPick = false;

    private Rigidbody2D rb2d;
    public GameObject boxPrefab;


    void Start()
    {
        rb2d = this.gameObject.GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - (1 * speed * Time.deltaTime), transform.position.z);
    }
    public void EnemyTakeDamage(float damage)
    {
        health -= damage;
        if (powerUpIncrase)
        {
            PowerUpSystem.Instance.AddChargeDamage(powerUpAmountDamage);
        }
        AudioManager.PlaySound(AudioManager.Sound.Enemy_TakeDamage);
        if (health <= 0)
        {
            Death();
        }
    }
    private void Death()
    {
        PowerUpSystem.Instance.AddChargeKill(powerUpAmountKill);
        // if the thief stole a box he drops a new box when killed
        if (boxPick && boxPrefab != null) 
        {
            Instantiate(boxPrefab, transform.position, Quaternion.identity);
        }
        Destroy(this.gameObject);
    }
    public void SetHealth(float newHealth) 
    {
        health = newHealth;
    }
}
