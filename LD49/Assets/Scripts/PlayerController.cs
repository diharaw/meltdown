using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : VehicleController
{
    public float m_rotationSpeed = 10.0f;
    public float m_dashMultiplier = 2.0f;
    public float m_dashDuration = 0.1f; // In seconds
    public float m_dashDelay = 1.0f; // In seconds
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

    private Transform m_transform;
    private Rigidbody m_rigidbody;
    private Vector3 m_movementDirection;
    private Vector3 m_aimDirection;
    private float m_currentMovementSpeed;
    private bool m_dashCooldownInProgress = false;
    private int m_currentMaxUnlockedTurretIndex = 3;
    private int m_currentTurretIndex = 0;
    private int m_availableScrap = 100;

    // Start is called before the first frame update
    void Start()
    {
        m_currentHitPoints = m_maxHitPoints;
        m_currentMovementSpeed = m_movementSpeed;
        m_transform = GetComponent<Transform>();
        m_rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(m_movementDirection, Vector3.up);
            m_rigidbody.rotation = Quaternion.RotateTowards(m_rigidbody.rotation, toRotation, m_rotationSpeed * Time.deltaTime);

            m_rigidbody.velocity = m_transform.forward * m_currentMovementSpeed;
        }
        else
            m_rigidbody.velocity = new Vector3(0.0f, 0.0f, 0.0f);

        if (m_aimDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(m_aimDirection, Vector3.up);
            m_turretBaseTransform.rotation = Quaternion.RotateTowards(m_turretBaseTransform.rotation, toRotation, m_rotationSpeed * Time.deltaTime);
        }
    }

    public void AddScrap(int amount)
    {
        m_availableScrap += amount;
    }

    public void OnMovement(InputAction.CallbackContext value)
    {
        Vector3 movement = value.ReadValue<Vector2>();
        m_movementDirection = new Vector3(movement.x, 0.0f, movement.y).normalized;
    }

    public void OnGamepadAim(InputAction.CallbackContext value)
    {
        Vector3 aim = value.ReadValue<Vector2>();
        m_aimDirection = new Vector3(aim.x, 0.0f, aim.y).normalized;
    }

    public void OnMouseAim(InputAction.CallbackContext value)
    {
        Vector2 aim = value.ReadValue<Vector2>();
        Vector3 screenPos = m_camera.WorldToScreenPoint(m_transform.position);

        m_aimDirection = (new Vector3(aim.x, 0.0f, aim.y) - new Vector3(screenPos.x, 0.0f, screenPos.y)).normalized;
    }

    public void OnFireLeft(InputAction.CallbackContext value)
    {
        if (value.started)
            m_leftWeaponController.StartFiring();
        else if (value.canceled)
            m_leftWeaponController.StopFiring();
    }

    public void OnFireRight(InputAction.CallbackContext value)
    {
        if (value.started)
            m_rightWeaponController.StartFiring();
        else if (value.canceled)
            m_rightWeaponController.StopFiring();
    }

    public void OnDash(InputAction.CallbackContext value)
    {
        if (!m_dashCooldownInProgress)
        {
            m_currentMovementSpeed = m_movementSpeed * m_dashMultiplier;
            m_dashCooldownInProgress = true;
            StartCoroutine("DashCooldown");
        }
    }

    public void OnCycleTurrets(InputAction.CallbackContext value)
    {
        float cycleTurrets = value.ReadValue<float>();

        if (cycleTurrets != 0.0f)
        {
            m_currentTurretIndex += (int)cycleTurrets;

            if (m_currentTurretIndex > m_currentMaxUnlockedTurretIndex)
                m_currentTurretIndex = 0;
            else if (m_currentTurretIndex < 0)
                m_currentTurretIndex = m_currentMaxUnlockedTurretIndex;
        }
    }

    public void OnPlaceTurret(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            if (m_powerPlantController.IncreasePowerDraw(m_turretPowerDrawCost[m_currentTurretIndex]) && m_turretScrapCost[m_currentTurretIndex] < m_availableScrap)
            {
                m_availableScrap -= m_turretScrapCost[m_currentTurretIndex];
                Instantiate(m_turretPrefabs[m_currentTurretIndex], m_transform.position, Quaternion.identity);
            }
        }
    }

    public void OnPlaceMine(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            if (m_mineScrapCost < m_availableScrap)
            {
                m_availableScrap -= m_mineScrapCost;
                Instantiate(m_minePrefab, m_transform.position, Quaternion.identity);
            }
        }
    }

    IEnumerator DashCooldown()
    {
        yield return new WaitForSeconds(m_dashDuration);
        m_currentMovementSpeed = m_movementSpeed;
        yield return new WaitForSeconds(m_dashDelay);
        m_dashCooldownInProgress = false;
    }
}
