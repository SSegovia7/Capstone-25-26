using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class gameManager : MonoBehaviour
{
    [SerializeField] private shipController _SHIP;

    [SerializeField] InputAction _MOVE;
     [SerializeField] InputAction _FIRE;
    public bool _FIRETOGGLE;
    public bool _PLAYERCONTROLENABLED;
    public float speed = 100f;

    void Start()
    {
        HandleSettings();
        _SHIP.SetShipPosition(new Vector3(0, 0, 0));
        _MOVE.Enable();
    }
    void FixedUpdate()
    {
        if (!_PLAYERCONTROLENABLED) { return; }
        Vector2 moveValue = _MOVE.ReadValue<Vector2>();
        _SHIP.MoveShip(moveValue, speed);
    }

    private void HandleSettings()
    {
        int fireToggle = PlayerPrefs.GetInt("FireMode");

        switch (fireToggle)
        {
            case 0:
                _FIRETOGGLE = false;
                break;
            case 1:
                _FIRETOGGLE = true;
                break;
        }
    }
}