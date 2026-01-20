using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public enum PowerUpType
{
    None,
    TripleShot,
    Speed,
    FireRate
}

public class PowerUpSystem : MonoBehaviour
{
    public static PowerUpSystem Instance;

    [Header("UI Charge")]
    [SerializeField] private Slider powerSlider;
    [SerializeField] private float maxCharge = 100f;
    [SerializeField] private float enemyKillCharge = 10f;
    [SerializeField] private float enemyDamageCharge = 2f;
    [SerializeField] private float pickUpFullCharge = 100f;

    [Header("Inventory System")]
    [SerializeField] private GameObject tripleShotIcon;
    [SerializeField] private GameObject speedIcon;
    [SerializeField] private GameObject fireRateIcon;

    [Header("References")]
    public gameManager _gameManager;
    public shipController _shipController;

    [Header("Power Up Settings")]
    [SerializeField] private float speedIncreaseAmount = 1.5f;
    [SerializeField] private float fireRateMultiplier = 0.7f;
    [SerializeField] private float powerUpDuration = 10f;

    private PowerUpType storedPowerUp = PowerUpType.None;
    private Coroutine activeRoutine;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    private void Start()
    {
        if (powerSlider != null)
            powerSlider.maxValue = maxCharge;

        UpdateInventoryUI();
    }

    public void AddChargeKill(float amount)
    {
        AddCharge(amount);
    }

    public void AddChargeDamage(float amount)
    {
        AddCharge(amount);
    }

    private void AddCharge(float amount)
    {
        if (powerSlider == null) return;

        powerSlider.value += amount;

        if (powerSlider.value >= maxCharge)
        {
            powerSlider.value = 0f;
            StoreRandomPowerUp();
        }
    }

    private void StoreRandomPowerUp()
    {
        int random = Random.Range(0, 3);
        storedPowerUp = (PowerUpType)(random + 1); // skip None
        UpdateInventoryUI();

        Debug.Log("Stored PowerUp: " + storedPowerUp);
    }

    public void ActivateStoredPowerUp()
    {
        if (storedPowerUp == PowerUpType.None) return;

        ResetAllPowerUps();

        switch (storedPowerUp)
        {
            case PowerUpType.TripleShot:
                StartTripleShotPowerUp();
                break;

            case PowerUpType.Speed:
                StartSpeedPowerUp();
                break;

            case PowerUpType.FireRate:
                StartFireRatePowerUp();
                break;
        }

        storedPowerUp = PowerUpType.None;
        UpdateInventoryUI();
    }

    private void UpdateInventoryUI()
    {
        tripleShotIcon.SetActive(storedPowerUp == PowerUpType.TripleShot);
        speedIcon.SetActive(storedPowerUp == PowerUpType.Speed);
        fireRateIcon.SetActive(storedPowerUp == PowerUpType.FireRate);
    }

    private void ResetAllPowerUps()
    {
        if (activeRoutine != null)
            StopCoroutine(activeRoutine);

        _gameManager.speed = _gameManager.defaultSpeed;
        _shipController.FiringCooldown = _shipController.defaultFiringCooldown;
        _shipController.curentShootingMode = shipController.ShootingMode.Single;
    }

    private void StartSpeedPowerUp()
    {
        _gameManager.speed *= speedIncreaseAmount;
        activeRoutine = StartCoroutine(ResetSpeedAfterTime());
    }

    private IEnumerator ResetSpeedAfterTime()
    {
        yield return new WaitForSeconds(powerUpDuration);
        _gameManager.speed = _gameManager.defaultSpeed;
    }

    private void StartFireRatePowerUp()
    {
        _shipController.FiringCooldown *= fireRateMultiplier;
        activeRoutine = StartCoroutine(ResetFireRateAfterTime());
    }

    private IEnumerator ResetFireRateAfterTime()
    {
        yield return new WaitForSeconds(powerUpDuration);
        _shipController.FiringCooldown = _shipController.defaultFiringCooldown;
    }

    private void StartTripleShotPowerUp()
    {
        _shipController.curentShootingMode = shipController.ShootingMode.Triple;
        activeRoutine = StartCoroutine(ResetTripleShotAfterTime());
    }

    private IEnumerator ResetTripleShotAfterTime()
    {
        yield return new WaitForSeconds(powerUpDuration);
        _shipController.curentShootingMode = shipController.ShootingMode.Single;
    }
}