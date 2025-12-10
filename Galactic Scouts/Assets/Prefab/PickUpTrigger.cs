using UnityEngine;

public class PickUpTrigger : MonoBehaviour
{
    [SerializeField] private float pickUpDelay = 1f;
    [SerializeField] private float launchForce = 7f;
    [SerializeField] private float horizontalRangeMin = 25f;
    [SerializeField] private float horizontalRangeMax = 40f;
    [SerializeField] private float impulseMultiplier = 5f;
    private bool canBePicked = false;

    private void Start()
    {
        ApplyImpulse();
        StartCoroutine(EnablePickupAfterDelay());
    }

    private void ApplyImpulse() 
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb == null) return;
        int sign = Random.Range(0, 2) == 0 ? -1 : 1;

        Vector3 force = new Vector3(Random.Range(horizontalRangeMin, horizontalRangeMax) * sign, launchForce, 0) * impulseMultiplier;
        rb.AddForce(force, ForceMode2D.Force);
    }
    private System.Collections.IEnumerator EnablePickupAfterDelay() 
    {
        canBePicked = false;
        yield return new WaitForSeconds(pickUpDelay);
        canBePicked = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Detect player only
        if (!canBePicked) return;
        if (!other.CompareTag("Player")) return;
        
            // Add charge to the power-up bar

            if (PowerUpSystem.Instance != null)
            {
                PowerUpSystem.Instance.ActivateRandomPowerUp();
            }

            // Destroy the pickup object
            Destroy(gameObject);
    }
}
