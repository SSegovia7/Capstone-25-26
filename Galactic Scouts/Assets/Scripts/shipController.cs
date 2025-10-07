using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shipController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D _rb2d;
    [SerializeField] private SpriteRenderer _sprite;
    
    // void Update()
    // {
    //     if (!_playerInControl) { return; }
    //     //add script for dialogue and cutscenes
    // }

    public void MoveShip(Vector2 direction, float force)
    {
        Debug.Log(direction * force);
        _rb2d.AddForce(direction * force * Time.deltaTime, ForceMode2D.Force);
    }
    public void SetShipPosition(Vector3 position)
    {
        transform.position = position;
    }
    public void FireBullet()
    {
        
    }
}
