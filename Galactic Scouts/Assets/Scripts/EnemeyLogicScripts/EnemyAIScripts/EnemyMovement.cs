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
        BasicShooter,
        Thief
    }

    [Header("General Settings")]
    public MovementType movementType = MovementType.Linear;
    public float verticalSpeed = 2f;

    [Header("Lifetime Settings")]
    public float lifeTime = 10f;
    public float spawnTime;

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
    public float minX = -4f;
    public float maxX = 4f;

    [Header("Thief Settings")]
    public float thiefSpeed = 3f;
    public float thiefTrackStrength = 1f;
    private bool hasStolen = false;
    private Transform thiefTarget;
    public bool escapeMode = false;
    private float escapeHorizontalDir = 0f;
    private Rigidbody2D rb;
    private float nextPlayerSearchTime;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        linearHorizontalSpeed = UnityEngine.Random.Range(-horizontalSpeedRange, horizontalSpeedRange);
        waveOffset = UnityEngine.Random.Range(0f, 2f * Mathf.PI);

        spawnTime = Time.time;

        Destroy(gameObject, lifeTime);
    }

    void FixedUpdate()
    {
        if (player == null && Time.time >= nextPlayerSearchTime)
        {
            FindPlayer();
        }

        Vector2 velocity = Vector2.zero;
        float elapsed = Time.time - spawnTime;

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
                HandleBasicShooter(ref velocity, elapsed);
                break;

            case MovementType.Thief:
                HandleThief(ref velocity);
                break;
        }

        rb.velocity = velocity;
    }

    // BASIC SHOOTER LOGIC
    private void HandleBasicShooter(ref Vector2 velocity, float elapsed)
    {
        if (lifeTime - elapsed <= 3f && !exiting)
        {
            exiting = true;
            movingDown = false;
            horizontalDir = transform.position.x < 0 ? 1 : -1;
        }

        if (exiting)
        {
            velocity = new Vector2(horizontalDir * exitSpeed, verticalSpeed);
            return;
        }

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

        velocity = new Vector2(horizontalDir * patrolSpeed, 0f);

        if (transform.position.x >= maxX)
        {
            horizontalDir = -1;
        }
        else if (transform.position.x <= minX)
        {
            horizontalDir = 1;
        }

        if (Time.time >= nextShootTime)
        {
            Shoot();
            nextShootTime = Time.time + shootInterval;
        }
    }

    private void Shoot()
    {
        if (projectilePrefab == null) return;

        Vector3 spawnPos = shootPoint != null ? shootPoint.position : transform.position;
        Instantiate(projectilePrefab, spawnPos, Quaternion.identity);
    }

    // THIEF LOGIC
    private void HandleThief(ref Vector2 velocity)
    {
        // Phase 0: EscapeMode
        if (escapeMode) 
        {
            velocity = new Vector2(escapeHorizontalDir, verticalSpeed);
            return;
        }
        // Phase 1: Follow Player
        if (!hasStolen)
        {
            if (thiefTarget == null)
            {
                GameObject p = GameObject.FindGameObjectWithTag("Player");
                if (p != null) thiefTarget = p.transform;
            }
        }
        else
        {
            // Phase 2: Find closest Box
            if (thiefTarget == null)
            {
                GameObject[] boxes = GameObject.FindGameObjectsWithTag("Box");
                if (boxes.Length > 0)
                {
                    thiefTarget = GetClosestObject(boxes).transform;
                }
            }
        }

        if (thiefTarget == null)
        {
            velocity = Vector2.zero;
            return;
        }

        Vector2 dir = (thiefTarget.position - transform.position);

        // Normal full 2D movement toward target
        velocity = dir.normalized * thiefSpeed;
    }

    private GameObject GetClosestObject(GameObject[] list)
    {
        GameObject closest = null;
        float minDist = Mathf.Infinity;
        Vector3 pos = transform.position;

        foreach (GameObject go in list)
        {
            float dist = (go.transform.position - pos).sqrMagnitude;
            if (dist < minDist)
            {
                minDist = dist;
                closest = go;
            }
        }

        return closest;
    }

    // COLLISION HANDLING
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (movementType != MovementType.Thief)
            return;

        if (!hasStolen && collision.CompareTag("Player"))
        {
            hasStolen = true;
            thiefTarget = null; // force finding a box next frame
        }
        // AFTER STEALING IT CHASES THE BOX
        if (hasStolen && !escapeMode && collision.CompareTag("Box")) 
        {
            Destroy(collision.gameObject);

            enemyController ec = GetComponent<enemyController>();
            if (ec != null) ec.boxPick = true;

            escapeMode = true;

            escapeHorizontalDir = Random.Range(-4f, 4f);

            thiefTarget = null;
        }
    }

    // PLAYER SEARCH
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