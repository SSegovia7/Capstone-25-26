using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shipController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb2d;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private GameObject _box;
    [SerializeField] private float launchForce;

    private float BoxForce = 5f;
    public delegate int NumDelegate(int value);
    public NumDelegate TookDamage;
    public NumDelegate GainHP;

    private int firingCooldown = 0;

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
    public void ShipDash(float force)
    {
        _rb2d.AddForce(_rb2d.velocity * force * 20f, ForceMode2D.Force);
    }
    public void SetShipPosition(Vector3 position)
    {
        transform.position = position;
    }
    public void Shoot()
    {
        if (firingCooldown > 0) { firingCooldown--; return; }
        firingCooldown = 8;
        Instantiate(_bullet, new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z), Quaternion.identity);
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "EnemyBullet" || col.gameObject.tag == "Enemy")
        {
            TookDamage?.Invoke(1);
            col.gameObject.GetComponent<enemyController>().EnemyTakeDamage(1);
            var cookieBox = Instantiate(_box, transform.position, Quaternion.identity);
            int sign = -1;
            if (Random.Range(-1, 1) >= 0) { sign = 1; }
            cookieBox.GetComponent<Rigidbody2D>().AddForce(new Vector3(Random.Range(4, 18) * sign, launchForce, 0) * BoxForce, ForceMode2D.Force);
        }
        else if (col.gameObject.tag == "Box")
        {
            bool succeded = col.gameObject.GetComponent<boxManager>().CollectThisBox();
            if (!succeded) { return; }
            else
            {
                
            }
        }
    }
}
