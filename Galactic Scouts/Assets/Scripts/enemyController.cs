using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyController : MonoBehaviour
{
    public float speed = 2f;
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
    public void Death()
    {
        Destroy(this.gameObject);
    }
}
