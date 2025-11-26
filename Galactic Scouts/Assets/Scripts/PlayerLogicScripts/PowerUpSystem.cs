using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpSystem : MonoBehaviour
{
    public static PowerUpSystem Instance;

    [Header("UI Conection")]
    [SerializeField] private Slider powerSlider;
    [SerializeField] private float maxCharge = 100f;

    [SerializeField] private float enemyKillCharge = 10f;
    [SerializeField] private float pickUpFullCharge = 100f;

    [Header("References")]
    public gameManager _gameManager;
    public shipController _shipController;

    [Header("Power Up Settings")]
    [SerializeField] private float speedIncreaseAmount = 1.5f;
    [SerializeField] private float fireRateMultiplier = 0.7f;
    [SerializeField] private float powerUpDuration = 10f;

    // Internal state for restoring values
    private float originalSpeed;
    private float originalFireRate;
    private Coroutine activeRoutine;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }

    private void Start()
    {
        if (powerSlider != null)
            powerSlider.maxValue = maxCharge;
    }

    public void AddCharge(float amount)
    {
        if (powerSlider == null) return;

        powerSlider.value += amount;

        if (powerSlider.value >= maxCharge)
        {
            powerSlider.value = 0f;
            ActivateRandomPowerUp();
        }
    }

    private void ActivateRandomPowerUp()
    {
        int random = Random.Range(0, 3);

        switch (random)
        {
            case 0:
                StartTripleShotPowerUp();
                break;

            case 1:
                StartSpeedPowerUp();
                break;

            case 2:
                StartFireRatePowerUp();
                break;
        }
    }

    private void StartSpeedPowerUp()
    {
        if (_gameManager == null) return;

        // Stop previous power-up if active
        if (activeRoutine != null) StopCoroutine(activeRoutine);

        // Save original value
        originalSpeed = _gameManager.defaultSpeed;

        // Apply boost
        _gameManager.speed *= speedIncreaseAmount;

        Debug.Log("Power up activated: speed increase");

        // Start timed effect
        activeRoutine = StartCoroutine(ResetSpeedAfterTime());
    }

    private IEnumerator ResetSpeedAfterTime()
    {
        yield return new WaitForSeconds(powerUpDuration);

        _gameManager.speed = _gameManager.defaultSpeed;
        Debug.Log("Power up expired: speed restored");
    }

    private void StartFireRatePowerUp()
    {
        if (_shipController == null) return;

        if (activeRoutine != null) StopCoroutine(activeRoutine);

        // Save original cooldown
        originalFireRate = _shipController.defaultFiringCooldown;

        // Apply boost (multiplying by <1 makes cooldown shorter)
        _shipController.FiringCooldown *= fireRateMultiplier;

        Debug.Log("Power up activated: fire rate increase");

        activeRoutine = StartCoroutine(ResetFireRateAfterTime());
    }

    private IEnumerator ResetFireRateAfterTime()
    {
        yield return new WaitForSeconds(powerUpDuration);

        _shipController.FiringCooldown = _shipController.defaultFiringCooldown;
        Debug.Log("Power up expired: fire rate restored");
    }

    private void StartTripleShotPowerUp()
    {
        if (_shipController == null) return;

        if (activeRoutine != null) StopCoroutine(activeRoutine);

        var ship = FindObjectOfType<shipController>();

        ship.curentShootingMode = shipController.ShootingMode.Triple;

        Debug.Log("Power up activated: TripleShot");

        activeRoutine = StartCoroutine(ResetTripleShotAfterTime());
    }

    private IEnumerator ResetTripleShotAfterTime()
    {
        yield return new WaitForSeconds(powerUpDuration);

        var ship = FindObjectOfType<shipController>();

        ship.curentShootingMode = shipController.ShootingMode.Triple;

        Debug.Log("Power up expired: Single restored");
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PowerUp"))
        {
            AddCharge(pickUpFullCharge);
            // Pickup is destroyed by its own script now
        }
    }
}