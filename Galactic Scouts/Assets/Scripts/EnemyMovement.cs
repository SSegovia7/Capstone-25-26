using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public enum MovementType
    {
        Linear,
        Wave,
        TrackPlayer
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
        }

        rb.velocity = velocity;
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