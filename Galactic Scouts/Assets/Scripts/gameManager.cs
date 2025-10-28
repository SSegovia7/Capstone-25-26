using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class gameManager : MonoBehaviour
{
    [SerializeField] private shipController _SHIP;
    [SerializeField] private uiHandler _HUD;
    [SerializeField] private InputAction _MOVE;
    [SerializeField] private InputAction _FIRE;
    [SerializeField] private InputAction _PAUSE;
    [SerializeField] private InputAction _DASH;
    public bool _HOLDTOFIREENABLED = true;
    public bool _PLAYERCONTROLENABLED = true;

    private bool _FIRETOGGLE = false;
    public float speed = 100f;
    public float dashForce = 200f;
    public int playerHealth = 20;
    public float dashCooldown = 2f;
    private bool canDash;
    private Vector2 moveValue;

    void Start()
    {
        _SHIP.TookDamage += PlayerTakeDamage;
        _SHIP.GainHP += PlayerHealHealth;
        _HUD.UpdateHealthDisplay(playerHealth);
        // HandleSettings();
        _SHIP.SetShipPosition(new Vector3(0, 0, 0));
        _MOVE.Enable();
        _FIRE.Enable();
        _DASH.Enable();
        _PAUSE.Enable();
        canDash = true;

        _MOVE.performed += context => MoveDelta(context);
        _DASH.started += context => OnDash(context);
        _FIRE.performed += context => OnFire(context);
        _PAUSE.started += context => OnPause(context);
    }
    void FixedUpdate()
    {
        if (!_PLAYERCONTROLENABLED) { return; }

        _SHIP.MoveShip(moveValue, speed);
    }
    private void OnPause(InputAction.CallbackContext context)
    {
        _PLAYERCONTROLENABLED = !_PLAYERCONTROLENABLED;
        _HUD.TogglePauseMenu();
        Time.timeScale = (_PLAYERCONTROLENABLED == false) ? 0f : 1f;
    }
    private void OnDash(InputAction.CallbackContext context)
    {
        if (canDash == true)
        {
            _SHIP.ShipDash(dashForce);
            StartCoroutine(ShipDashCooldown());
        }
    }
    private void MoveDelta(InputAction.CallbackContext context)
    {
        moveValue = context.ReadValue<Vector2>();
    }
    private void OnFire(InputAction.CallbackContext context)
    {
        _SHIP.Shoot();
    }
    private int PlayerTakeDamage(int amount)
    {
        playerHealth -= 1;
        _HUD.UpdateHealthDisplay(playerHealth);

        if (playerHealth <= 0)
        {
            PlayerLose();
        }
        return 0;
    }
    private int PlayerHealHealth(int amount)
    {
        playerHealth += 1;
        _HUD.UpdateHealthDisplay(playerHealth);

        return 0;
    }
    private void PlayerLose()
    {
        //TODO
    }

    private void HandleSettings()
    {
        int fireToggle = PlayerPrefs.GetInt("FireMode");

        switch (fireToggle)
        {
            case 0:
                _HOLDTOFIREENABLED = false;
                break;
            case 1:
                _HOLDTOFIREENABLED = true;
                _FIRETOGGLE = false;
                break;
        }
    }
    IEnumerator ShipDashCooldown()
    {
        canDash = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}