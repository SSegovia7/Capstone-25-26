using UnityEngine;

public class PickUpTrigger : MonoBehaviour
{
    [Header("Charge amount applied to PowerUpSystem")]
    public float chargeAmount = 100f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Detect player only
        if (other.CompareTag("Player"))
        {
            // Add charge to the power-up bar
            if (PowerUpSystem.Instance != null)
            {
                PowerUpSystem.Instance.AddChargeKill(chargeAmount);
            }

            // Destroy the pickup object
            Destroy(gameObject);
        }
    }
}
