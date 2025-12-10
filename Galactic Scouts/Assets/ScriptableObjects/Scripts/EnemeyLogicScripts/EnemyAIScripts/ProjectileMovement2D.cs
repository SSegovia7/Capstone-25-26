 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileMovement2D : MonoBehaviour
{
    public enum MovementType
    {
        Linear,
        PlayerTracking
       

    }
    [Header("General Settings")]
    public MovementType movementType = MovementType.Linear;
    public float speed = 5f;
    public float lifeTime = 5f;
    public string playerTag = "player";

    private Vector2 moveDirection;
    private Rigidbody2D rb;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null) 
        {
            Debug.LogError("rigidbody Missing");
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        switch (movementType)
        {
            case MovementType.Linear:
                moveDirection = Vector2.down;
                break;

            case MovementType.PlayerTracking:
                AimAtPlayer();
                break;    
        }
        rb.velocity = moveDirection * speed;
        Destroy(gameObject, lifeTime);
    }
    // Update is called once per frame
    void AimAtPlayer() 
    {
        GameObject player = GameObject.FindGameObjectWithTag(playerTag);
        if (player != null)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            moveDirection = direction;

            // Rotate sprite/transform to face the target
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle - 90f, Vector3.forward); // -90 assumes default sprite faces up
        }
        else
        {
            // If no player found, default to downward
            moveDirection = Vector2.down;
        }
    }
}
