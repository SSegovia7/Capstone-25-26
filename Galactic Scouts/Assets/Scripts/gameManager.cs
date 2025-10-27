using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class gameManager : MonoBehaviour
{
    [SerializeField] private shipController _SHIP;
    [SerializeField] private uiHandler _HUD;
    [SerializeField] InputAction _MOVE;
    [SerializeField] InputAction _FIRE;
    [SerializeField] InputAction _PAUSE;
    [SerializeField] InputAction _DASH;
    public bool _HOLDTOFIREENABLED = true;
    public bool _PLAYERCONTROLENABLED = true;

    private bool _FIRETOGGLE = false;
    public float speed = 100f;
    public float dashForce = 200f;
    public int playerHealth = 20;
    public float dashCooldown = 2f;
    private bool canDash;

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
    }
    void FixedUpdate()
    {
        if (!_PLAYERCONTROLENABLED) { return; }

        if (_PAUSE.IsPressed())
        {
            _PLAYERCONTROLENABLED = !_PLAYERCONTROLENABLED;
            _HUD.TogglePauseMenu();
        }

        Vector2 moveValue = _MOVE.ReadValue<Vector2>();
        _SHIP.MoveShip(moveValue, speed);

        if (_DASH.IsPressed())
        {
            if (canDash==true) {
                _SHIP.ShipDash(dashForce);
                StartCoroutine(ShipDashCooldown());
            }
            
        }

        if (_HOLDTOFIREENABLED == false)
        {
            if (_FIRE.WasPressedThisFrame())
            {
                _FIRETOGGLE = !_FIRETOGGLE;
            }
            if (_FIRETOGGLE)
            {
                _SHIP.Shoot();
            }
        }
        else
        {
            if (_FIRE.IsPressed())
            {
                _SHIP.Shoot();
            }
        }
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