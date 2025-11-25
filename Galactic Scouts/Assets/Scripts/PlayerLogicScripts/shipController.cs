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
    [SerializeField] private float _invincibleTimerInSeconds;
    public enum ShootingMode 
    {
        Single,
        Triple
    }
    public ShootingMode curentShootingMode = ShootingMode.Single;

    private bool _isInvincible = false;
    private float BoxForce = 5f;
    public delegate int NumDelegate(int value);
    public NumDelegate TookDamage;
    public NumDelegate GainHP;

    private float _FIRECOOLDOWN = 0;
    [SerializeField] public float FiringCooldown;

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
            //Instantiate(_bullet, new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z), Quaternion.identity);
           
            Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y - 0.2f, transform.position.z);

            switch (curentShootingMode) 
            {
                case ShootingMode.Single:
                    ShootSingle(spawnPos);
                    break;

                case ShootingMode.Triple:
                    ShootTriple(spawnPos);
                    break;

                default:
                    ShootSingle(spawnPos);
                    break;
            }                      
        }
    }
    private void ShootSingle(Vector3 spawnPos)
    {
        Instantiate(_bullet, spawnPos, Quaternion.identity);
    }
    private void ShootTriple(Vector3 spawnPos) 
    {
        // Angles for triple shot
        float[] angles = { -15f, 0f, 15f };

        foreach (float angle in angles)
        {
            Quaternion rot = Quaternion.Euler(0f, 0f, angle);
            Instantiate(_bullet, spawnPos, rot);
        }
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "EnemyBullet" || col.gameObject.tag == "Enemy")
        {
            if(_isInvincible) return;

            TookDamage?.Invoke(1);
            col.gameObject.GetComponent<enemyController>().EnemyTakeDamage(1);
            StartCoroutine(InvincibiltyFrames());
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

    private IEnumerator InvincibiltyFrames()
    {
        _isInvincible = true;

        Color spriteColor = _sprite.color;
        spriteColor.a = 0.3f; //Changes Transparency values
        _sprite.color = spriteColor;

        yield return new WaitForSeconds(_invincibleTimerInSeconds);

        spriteColor.a = 1f; //Changes transparency value bakc to normal
        _sprite.color = spriteColor;

        _isInvincible = false;
    }
}
