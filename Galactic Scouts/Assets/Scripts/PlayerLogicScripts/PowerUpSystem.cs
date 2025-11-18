using System.Collections;
using System.Collections.Generic;
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

    public gameManager _gameManager;
    public shipController _shipController;

    [SerializeField] private float speedIncreaseAmount = 1.5f;
    [SerializeField] private float fireRateMultiplier = 0.7f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
    }
    private void Start()
    {
        if (powerSlider != null) 
        {
            powerSlider.maxValue = maxCharge;
        } 
    }

    public void AddCharge(float amount) 
    {
        if (powerSlider == null) return;
        powerSlider.value += amount;
        if (powerSlider.value >= maxCharge) 
        {
            powerSlider.value = 0f;
            // ActivateRandomPowerUp();
        }
    }

    private void ActivateRandomPowerUp() 
    {
        int random = Random.Range(0, 2);

        switch (random) 
        {
            case 0:
                //ApplySpeedIncrease();
                break;

            case 1:
                //ApplyFireRateBoost();
                break;
        }
    }
}
