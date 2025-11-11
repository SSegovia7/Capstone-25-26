using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public enum MovementType
    {
        Linear,
        Wave,
        TrackPlayer,
        BasicShooter

    }

    [Header("General Settings")]
    public MovementType movementType = MovementType.Linear;
    public float verticalSpeed = 2f;

    [Header("Lifetime Settings")]
    public float lifeTime = 10f; // Time in seconds before auto-destroy

    [Header("Linear Movement")]
    public float horizontalSpeedRange = 1f;
    private float linearHorizontalSpeed;

    [Header("Wave Movement")]
    public float waveAmplitude = 1f;
    public float waveFrequency = 2f;
    private float waveOffset;

    [Header("Track Player Movement")]
    public string playerTag = "Player";
    public float trackStrength = 0.5f;
    public float maxHorizontalSpeed = 2f;
    private Transform player;

    [Header("Basic Shooter Movement")]
    public float stopY = 2f;
    public float patrolSpeed = 3f;
    public float shootInterval = 1.5f;
    public GameObject projectilePrefab;
    public Transform shootPoint;
    public float exitSpeed = 4f;
    public float nextShootTime;
    private bool movingDown = true;
    private bool exiting = false;
    private int horizontalDir = 1;

    private Rigidbody2D rb;
    private float nextPlayerSearchTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Randomized pattern setup
        linearHorizontalSpeed = UnityEngine.Random.Range(-horizontalSpeedRange, horizontalSpeedRange);
        waveOffset = UnityEngine.Random.Range(0f, 2f * Mathf.PI);

        // Auto-destroy after set lifetime
        Destroy(gameObject, lifeTime);
    }

    void FixedUpdate()
    {
        // Find player if needed
        if (player == null && Time.time >= nextPlayerSearchTime)
        {
            FindPlayer();
        }

        Vector2 velocity = Vector2.zero;

        switch (movementType)
        {
            case MovementType.Linear:
                velocity = new Vector2(linearHorizontalSpeed, -verticalSpeed);
                break;

            case MovementType.Wave:
                float waveX = Mathf.Sin(Time.time * waveFrequency + waveOffset) * waveAmplitude;
                velocity = new Vector2(waveX, -verticalSpeed);
                break;

            case MovementType.TrackPlayer:
                if (player != null)
                {
                    float directionX = player.position.x - transform.position.x;
                    float horizontalMove = Mathf.Clamp(directionX * trackStrength, -maxHorizontalSpeed, maxHorizontalSpeed);
                    velocity = new Vector2(horizontalMove, -verticalSpeed);
                }
                else
                {
                    velocity = new Vector2(0f, -verticalSpeed);
                }
                break;

            case MovementType.BasicShooter:
                //
                break;
        }

        rb.velocity = velocity;
    }

     private void HandleBasicShooter(ref Vector2 velocity, float elapsed) 
     {
        // start exit 3 seconds before the enemy is destroyed 
        if (lifeTime - elapsed <= 3f && !exiting) 
        {
            exiting = true;
            movingDown = false;
            horizontalDir = transform.position.x < 0 ? 1 : -1;
        }

        // move left to right to off screen
        if (exiting) 
        {
            velocity = new Vector2(horizontalDir * exitSpeed, verticalSpeed);
            return;
        }

        // move down until it reach pos Y
        if (movingDown) 
        {
            velocity = new Vector2(0, -verticalSpeed);
            if (transform.position.y <= stopY) 
            {
                movingDown = false;
                nextShootTime = Time.time + shootInterval;
                
            }
            return;
        }
        // Patrol left and right 

        velocity = new Vector2(horizontalDir * patrolSpeed, 0f);

        // Revers direction when reach the edge of the screen

        float screenHalfWidth = Camera.main.orthographicSize * Camera.main.aspect;
        if (Mathf.Abs (transform.position.x) >=screenHalfWidth * 0.9f) 
        {
            horizontalDir *= -1;
        }
        // shooting behavior

        if (Time.time >= nextShootTime) 
        {
            // shoot tigger 
            nextShootTime = Time.time + shootInterval;
        }
     
     }
    private void Shoot() 
    {
        if (projectilePrefab == null) return;
        {
            Vector3 spawnPos = shootPoint != null ? shootPoint.position : transform.position;
            Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
        }
    }
    private void FindPlayer()
    {
        GameObject found = GameObject.FindGameObjectWithTag(playerTag);
        if (found != null)
        {
            player = found.transform;
        }
        else
        {
            nextPlayerSearchTime = Time.time + 2f;
        }
    }
}