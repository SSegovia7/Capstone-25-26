using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class gameManager : MonoBehaviour
{
    [SerializeField] private shipController _SHIP;

    [SerializeField] InputAction _INPUTS;
    public bool _FIRETOGGLE;
    private bool _PLAYERCONTROLENABLED;

    void Start()
    {
        HandleSettings();
        _SHIP.SetShipPosition(new Vector3(0, 0, 0));
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