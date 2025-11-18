using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class gameManager : MonoBehaviour
{
    [SerializeField] private GameObject _PANELGAMEOVER;
    [SerializeField] private shipController _SHIP;
    [SerializeField] private uiHandler _HUD;
    [SerializeField] private InputAction _MOVE;
    [SerializeField] private InputAction _FIRE;
    [SerializeField] private InputAction _PAUSE;
    [SerializeField] private InputAction _DASH;
    public bool _HOLDTOFIRE = true;
    public bool _PLAYERCONTROLENABLED = true;

    private bool _FIRETOGGLE = false;
    private bool _FIRING = false;
    public float speed;
    public float dashForce;
    private float speedMultiplier = 1f;
    public int playerHealth = 20;
    public float dashCooldown = 2f;
    private bool canDash;
    private Vector2 moveValue;

    public bool inDialogue = false;
    [SerializeField] private GameObject dialogueManager;
    private DialogueManager dialogueScript;

    public DialogueData[] testData;

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
        _MOVE.canceled += context => MoveZero(context);
        _DASH.started += context => OnDash(context);
        _FIRE.started += context => OnFireDown(context);
        _FIRE.canceled += context => OnFireUp(context);
        _PAUSE.started += context => OnPause(context);

        dialogueScript = dialogueManager.GetComponent<DialogueManager>();
        dialogueScript.gameManager = this;

        //SetupDialogue(testData);
    }
    void FixedUpdate()
    {
        if (!_PLAYERCONTROLENABLED) { return; }

        _SHIP.MoveShip(moveValue * speedMultiplier, speed);
        if (_FIRING)
        {
            _SHIP.Shoot();
        }
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
            AudioManager.PlaySound(AudioManager.Sound.GS_Dash);
            speedMultiplier = dashForce;
            StartCoroutine(ShipDashCooldown());
        }
    }
    private void MoveDelta(InputAction.CallbackContext context)
    {
        moveValue = context.ReadValue<Vector2>();
    }
    private void MoveZero(InputAction.CallbackContext context)
    {
        moveValue = Vector2.zero;
    }
    /*private void OnFire(InputAction.CallbackContext context)
    {
        _SHIP.Shoot();
    }*/

    private void OnFireDown(InputAction.CallbackContext context)
    {
        if (_HOLDTOFIRE)
        {
            _FIRING = true;
        }
        else
        {
            _FIRING = !_FIRING;
        }
    }
    private void OnFireUp(InputAction.CallbackContext context)
    {
        if (_HOLDTOFIRE)
        {
            _FIRING = false;
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
        if (_PANELGAMEOVER != null)
        {
            _PANELGAMEOVER.SetActive(true);
            Time.timeScale = 0f;
        }
    }

    private void HandleSettings()
    {
        int fireToggle = PlayerPrefs.GetInt("FireMode");

        switch (fireToggle)
        {
            case 0:
                _HOLDTOFIRE = false;
                break;
            case 1:
                _HOLDTOFIRE = true;
                _FIRETOGGLE = false;
                break;
        }
    }
    IEnumerator ShipDashCooldown()
    {
        canDash = false;
        yield return new WaitForSeconds(0.1f);
        speedMultiplier = 1f;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
    //Dialogue can be called by calling the SetUp Dialogue Function, anything needed once dialogue ends can be put in End Dialogue which runs once the last
    //piece of dialogue resolves
    private void SetupDialogue(DialogueData[] dialogueToRun)
    {
        _SHIP.inDialogue = true;
        dialogueScript.currentDialogueData = dialogueToRun;
        _SHIP.SetShipPosition(new Vector3(0, 0, 0));
    }
    public void EndDialogue()
    {
        _SHIP.inDialogue = false;
    }
}

/*
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
    //public bool _HOLDTOFIRE = true;
    public bool _PLAYERCONTROLENABLED = true;

    //private bool _FIRING = false;
    public float speed;
    public float dashForce;
    private float speedMultiplier = 1f;
    public int playerHealth = 20;
    public float dashCooldown = 2f;
    private bool canDash;
    private bool _FIRETOGGLE = false;
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

        _FIRE.started += context => OnFireDown(context);
        _FIRE.canceled += context => OnFireUp(context);

        _PAUSE.started += context => OnPause(context);
    }
    void FixedUpdate()
    {
        if (!_PLAYERCONTROLENABLED) { return; }

        _SHIP.MoveShip(moveValue * speedMultiplier, speed);

        if (_FIRING)
        {
            _SHIP.Shoot();
        }
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
            speedMultiplier = dashForce;
            StartCoroutine(ShipDashCooldown());
        }
    }
    private void MoveDelta(InputAction.CallbackContext context)
    {
        moveValue = context.ReadValue<Vector2>();
    }
    private void OnFireDown(InputAction.CallbackContext context)
    {
        if (_HOLDTOFIRE)
        {
            _FIRING = true;
        }
        else
        {
            _FIRING = !_FIRING;
        }
    }
    private void OnFireUp(InputAction.CallbackContext context)
    {
        if (_HOLDTOFIRE)
        {
            _FIRING = false;
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
                _HOLDTOFIRE = false;
                break;
            case 1:
                _HOLDTOFIRE = true;
                _FIRETOGGLE = false;
                break;
        }
    }
    IEnumerator ShipDashCooldown()
    {
        canDash = false;
        yield return new WaitForSeconds(0.2f);
        speedMultiplier = 1f;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}
*/