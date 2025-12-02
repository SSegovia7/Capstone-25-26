using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyController : MonoBehaviour
{
    public float speed = 2f;
    public int health = 3;
    public float powerUpAmountDamage = 2f;
    public float powerUpAmountKill = 5f;
    public bool powerUpIncrase = false;
    private Rigidbody2D rb2d;
    void Start()
    {
        rb2d = this.gameObject.GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y - (1 * speed * Time.deltaTime), transform.position.z);
    }
    public void EnemyTakeDamage(int damage)
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
        Destroy(this.gameObject);
    }
}
