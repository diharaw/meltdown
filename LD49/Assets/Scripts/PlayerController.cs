using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : VehicleController
{
    public float m_rotationSpeed = 10.0f;
    public float m_dashMultiplier = 2.0f;
    public float m_dashDuration = 0.1f; // In seconds
    public float m_dashDelay = 1.0f; // In seconds
    public float m_invulnerabilityDuration = 1.0f; // In seconds
    public int m_mineScrapCost = 10;
    public Transform m_turretBaseTransform;
    public Camera m_camera;
    public GameObject[] m_turretPrefabs;
    public int[] m_turretPowerDrawCost;
    public int[] m_turretScrapCost;
    public GameObject m_minePrefab;
    public PowerPlantController m_powerPlantController;
    public WeaponController m_leftWeaponController;
    public WeaponController m_rightWeaponController;
    public GameObject m_rightWeapon;
    public AudioSource m_engineAudioSource;
    public AudioSource m_scrapAudioSource;
    public AudioSource m_dashAudioSource;

    private Transform m_transform;
    private Rigidbody m_rigidbody;
    private Vector3 m_movementDirection;
    private Vector3 m_aimDirection;
    private float m_currentMovementSpeed;
    private bool m_dashCooldownInProgress = false;
    private int m_currentTurretIndex = 0;
    private int m_availableScrap = 100;
    private const float m_enginePitchDelta = 0.2f;
    private float m_baseEnginePitch;
    private bool m_enginePitchTransition = false;
    private bool m_invulnerable = false;

    // Start is called before the first frame update
    void Start()
    {
        m_baseEnginePitch = m_engineAudioSource.pitch;
        m_currentHitPoints = m_maxHitPoints;
        m_currentMovementSpeed = m_movementSpeed;
        UIController.sharedInstance.UpdateHealthBar(m_currentHitPoints / m_maxHitPoints);
        UIController.sharedInstance.HighlightTurretIcon(m_currentTurretIndex);
        m_transform = GetComponent<Transform>();
        m_rigidbody = GetComponent<Rigidbody>();
        m_destructionAudioSource = GetComponent<AudioSource>();
        m_destructionAudioSource.pitch = m_destructionAudioSource.pitch + Random.Range(-0.2f, 0.2f);
        m_rightWeapon.SetActive(false);
        UIController.sharedInstance.UpdateXpBar(0);
        UIController.sharedInstance.UpdateLevelTxt(1);
        UIController.sharedInstance.UpdateScrapTxt(m_availableScrap);
    }

    // Update is called once per frame
    void Update()
    {
        // Unlock energy weapon if level if met 
        if (Globals.sharedInstance.m_level >= 2)
            m_rightWeapon.SetActive(true);

        if (Globals.sharedInstance.m_isGameOver)
        {
            m_leftWeaponController.StopFiring();
            m_rightWeaponController.StopFiring();
            m_rigidbody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
            StartRampDownEnginePitch();
            return;
        }

        if (Globals.sharedInstance.m_isPaused)
            return;

        if (m_movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(m_movementDirection, Vector3.up);
            m_rigidbody.rotation = Quaternion.RotateTowards(m_rigidbody.rotation, toRotation, m_rotationSpeed * Time.deltaTime);

            m_rigidbody.velocity = m_movementDirection * m_currentMovementSpeed;
            StartRampUpEnginePitch();
        }
        else
        {
            m_rigidbody.velocity = new Vector3(0.0f, 0.0f, 0.0f);
            StartRampDownEnginePitch();
        }

        if (m_aimDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(m_aimDirection, Vector3.up);
            m_turretBaseTransform.rotation = Quaternion.RotateTowards(m_turretBaseTransform.rotation, toRotation, m_rotationSpeed * Time.deltaTime);
        }
    }

    public void RecoverHealth()
    {
        m_currentHitPoints = m_maxHitPoints;
        UIController.sharedInstance.UpdateHealthBar(m_currentHitPoints / m_maxHitPoints);
    }

    public void AddScrap(int amount)
    {
        m_availableScrap += amount;
        UIController.sharedInstance.UpdateScrapTxt(m_availableScrap);
        m_scrapAudioSource.Play();
    }

    public void OnMovement(InputAction.CallbackContext value)
    {
        if (Globals.sharedInstance.m_isPaused || Globals.sharedInstance.m_isGameOver)
            return;

        Vector3 movement = value.ReadValue<Vector2>();
        m_movementDirection = new Vector3(movement.x, 0.0f, movement.y).normalized;
    }

    public void OnGamepadAim(InputAction.CallbackContext value)
    {
        if (Globals.sharedInstance.m_isPaused || Globals.sharedInstance.m_isGameOver)
            return;

        Vector3 aim = value.ReadValue<Vector2>();
        m_aimDirection = new Vector3(aim.x, 0.0f, aim.y).normalized;
    }

    public void OnMouseAim(InputAction.CallbackContext value)
    {
        if (Globals.sharedInstance.m_isPaused || Globals.sharedInstance.m_isGameOver)
            return;

        Vector2 aim = value.ReadValue<Vector2>();
        Vector3 screenPos = m_camera.WorldToScreenPoint(m_transform.position);

        m_aimDirection = (new Vector3(aim.x, 0.0f, aim.y) - new Vector3(screenPos.x, 0.0f, screenPos.y)).normalized;
    }

    public void OnFireLeft(InputAction.CallbackContext value)
    {
        if (Globals.sharedInstance.m_isPaused || Globals.sharedInstance.m_isGameOver)
            return;

        if (value.started)
            m_leftWeaponController.StartFiring();
        else if (value.canceled)
            m_leftWeaponController.StopFiring();
    }

    public void OnFireRight(InputAction.CallbackContext value)
    {
        if (Globals.sharedInstance.m_level < 2 || Globals.sharedInstance.m_isPaused || Globals.sharedInstance.m_isGameOver)
            return;

        if (value.started)
            m_rightWeaponController.StartFiring();
        else if (value.canceled)
            m_rightWeaponController.StopFiring();
    }

    public void OnDash(InputAction.CallbackContext value)
    {
        if (Globals.sharedInstance.m_isPaused || Globals.sharedInstance.m_isGameOver)
            return;

        if (!m_dashCooldownInProgress)
        {
            m_dashAudioSource.Play();
            m_invulnerable = true;
            m_currentMovementSpeed = m_movementSpeed * m_dashMultiplier;
            m_dashCooldownInProgress = true;
            StartCoroutine("DashCooldown");
            StartCoroutine("EndInvulnerability");
        }
    }

    public void OnCycleTurrets(InputAction.CallbackContext value)
    {
        if (Globals.sharedInstance.m_isPaused || Globals.sharedInstance.m_isGameOver)
            return;

        float cycleTurrets = value.ReadValue<float>();

        if (cycleTurrets != 0.0f)
        {
            m_currentTurretIndex += (int)cycleTurrets;

            if (m_currentTurretIndex > Globals.sharedInstance.m_currentMaxUnlockedTurretIndex)
                m_currentTurretIndex = 0;
            else if (m_currentTurretIndex < 0)
                m_currentTurretIndex = Globals.sharedInstance.m_currentMaxUnlockedTurretIndex;
            else if (Globals.sharedInstance.m_currentMaxUnlockedTurretIndex == 0)
                m_currentTurretIndex = 0;
            
            UIController.sharedInstance.HighlightTurretIcon(m_currentTurretIndex);
        }
    }

    public void OnPlaceTurret(InputAction.CallbackContext value)
    {
        if (Globals.sharedInstance.m_isPaused || Globals.sharedInstance.m_isGameOver)
            return;

        if (value.performed)
        {
            if (m_powerPlantController.IncreasePowerDraw(m_turretPowerDrawCost[m_currentTurretIndex]) && m_turretScrapCost[m_currentTurretIndex] < m_availableScrap)
            {
                m_availableScrap -= m_turretScrapCost[m_currentTurretIndex];
                GameObject gb = Instantiate(m_turretPrefabs[m_currentTurretIndex], m_transform.position, Quaternion.identity);

                gb.GetComponentInChildren<TurretController>().m_powerDraw = m_turretPowerDrawCost[m_currentTurretIndex];

                UIController.sharedInstance.UpdateScrapTxt(m_availableScrap);
            }
        }
    }

    public void OnPlaceMine(InputAction.CallbackContext value)
    {
        if (Globals.sharedInstance.m_level < 3 || Globals.sharedInstance.m_isPaused || Globals.sharedInstance.m_isGameOver)
            return;

        if (value.performed)
        {
            if (m_mineScrapCost < m_availableScrap)
            {
                m_availableScrap -= m_mineScrapCost;
                Instantiate(m_minePrefab, m_transform.position, Quaternion.identity);
                UIController.sharedInstance.UpdateScrapTxt(m_availableScrap);
            }
        }
    }

    public void OnQuit(InputAction.CallbackContext value)
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        SceneManager.LoadScene(0);
    }

    public override void TakeDamage(float damage)
    {
        if (m_invulnerable)
            return;

        base.TakeDamage(damage);

        UIController.sharedInstance.UpdateHealthBar(m_currentHitPoints / m_maxHitPoints);

        if (m_currentHitPoints == 0.0f)
        {
            Globals.sharedInstance.m_isGameOver = true;
            UIController.sharedInstance.m_gameOverPanel.SetActive(true);
            UIController.sharedInstance.m_txtGameOverScore.text = Globals.sharedInstance.m_xp.ToString();

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    void StartRampUpEnginePitch()
    {
        if (!m_enginePitchTransition && m_engineAudioSource.pitch < (m_baseEnginePitch + m_enginePitchDelta))
        {
            m_enginePitchTransition = true;
            StartCoroutine("RampUpEnginePitch");
        }
    }

    void StartRampDownEnginePitch()
    {
        if (!m_enginePitchTransition && m_engineAudioSource.pitch > m_baseEnginePitch)
        {
            m_enginePitchTransition = true;
            StartCoroutine("RampDownEnginePitch");
        }
    }


    IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(m_dashDuration);
        m_currentMovementSpeed = m_movementSpeed;
        yield return new WaitForSeconds(m_dashDelay);
        m_dashCooldownInProgress = false;
    }

    IEnumerator EndInvulnerability()
    {
        yield return new WaitForSeconds(m_invulnerabilityDuration);
        m_invulnerable = false;
    }

    IEnumerator RampUpEnginePitch()
    {
        float pitch = 0.0f;

        while (pitch < m_enginePitchDelta)
        {
            pitch += 0.05f;
            m_engineAudioSource.pitch = m_baseEnginePitch + pitch;
            yield return new WaitForSeconds(0.05f);
        }

        m_enginePitchTransition = false;
    }

    IEnumerator RampDownEnginePitch()
    {
        float pitch = 0.0f;
        float curretPitch = m_engineAudioSource.pitch;

        while (pitch < m_enginePitchDelta)
        {
            pitch += 0.05f;
            m_engineAudioSource.pitch = curretPitch - pitch;
            yield return new WaitForSeconds(0.05f);
        }

        m_enginePitchTransition = false;
    }
}
