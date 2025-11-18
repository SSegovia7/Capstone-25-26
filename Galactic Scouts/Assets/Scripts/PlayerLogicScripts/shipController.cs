using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shipController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb2d;
    [SerializeField] private SpriteRenderer _sprite;
    [SerializeField] private Animator _animator;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private GameObject _box;
    [SerializeField] private float launchForce;

    private float BoxForce = 5f;
    public delegate int NumDelegate(int value);
    public NumDelegate TookDamage;
    public NumDelegate GainHP;

    private float _FIRECOOLDOWN = 0;
    [SerializeField] private float FiringCooldown;

    public bool inDialogue = false;

    // void Update()
    // {
    //     if (!_playerInControl) { return; }
    //     //add script for dialogue and cutscenes
    // }

    public void MoveShip(Vector2 direction, float force)
    {
        if (!inDialogue)
        {
            //Debug.Log(direction * force);
            // _rb2d.AddForce(direction * force * Time.fixedDeltaTime, ForceMode2D.Force);
            Debug.Log("Current velocity is" + _rb2d.velocity);
            _animator.SetFloat("horizontalMovement", direction.x);
            _rb2d.velocity = direction * force * Time.fixedDeltaTime;
        }

    }
    public void ShipDash(float force)
    {
        if (!inDialogue)
        {
            _rb2d.AddForce(_rb2d.velocity * force, ForceMode2D.Force);
        }
    }
    public void SetShipPosition(Vector3 position)
    {
        transform.position = position;
    }
    public void Shoot()
    {
        if (!inDialogue)
        {
            if (_FIRECOOLDOWN > 0) { _FIRECOOLDOWN--; return; }

            AudioManager.PlaySound(AudioManager.Sound.GS_Shooting);

            _FIRECOOLDOWN = FiringCooldown;
            Instantiate(_bullet, new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z), Quaternion.identity);
        }
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
                AudioManager.PlaySound(AudioManager.Sound.GS_BoxCollect);
                GainHP?.Invoke(1);
            }
        }
    }
}
