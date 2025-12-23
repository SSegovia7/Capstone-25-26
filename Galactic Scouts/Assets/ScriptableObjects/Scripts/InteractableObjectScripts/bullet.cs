using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    public float speed = 5f;
    public float bulletDamage = 1f;
    private Rigidbody2D _rb;
    // Start is called before the first frame update
    /*void Update()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + (1 * speed * Time.deltaTime), transform.position.z);
    }*/

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }
    private void Start()
    {
        _rb.velocity = transform.up * speed;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Despawn")
        {
            Destroy(this.gameObject);
        }
        if (col.gameObject.tag == "Enemy")
        {
            col.gameObject.GetComponent<enemyController>().EnemyTakeDamage(bulletDamage);
            Destroy(this.gameObject);
        }
    }
}
