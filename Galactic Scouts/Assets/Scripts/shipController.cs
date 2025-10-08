using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shipController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb2d;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private GameObject _bullet;

    public delegate void EmptyDelegate();
    public EmptyDelegate TookDamage;

    private int cooldown = 0;

    // void Update()
    // {
    //     if (!_playerInControl) { return; }
    //     //add script for dialogue and cutscenes
    // }

    public void MoveShip(Vector2 direction, float force)
    {
        Debug.Log(direction * force);
        _rb2d.velocity = direction * force * Time.fixedDeltaTime;
    }
    public void SetShipPosition(Vector3 position)
    {
        transform.position = position;
    }
    public void Shoot()
    {
        if (cooldown > 0) { cooldown--; return; }
        cooldown = 8;
        Instantiate(_bullet, new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z), Quaternion.identity);
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy" || col.gameObject.tag == "EnemyBullet")
        {
            TookDamage?.Invoke();
            col.gameObject.GetComponent<enemyController>().Death();
        }
    }
}
