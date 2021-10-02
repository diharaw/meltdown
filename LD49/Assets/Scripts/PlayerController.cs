using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float m_movementSpeed = 10.0f;
    public float m_rotationSpeed = 10.0f;
    public Transform m_turretBaseTransform;
    public Camera m_camera;

    private Transform m_transform;
    private Rigidbody m_rigidbody;
    private Vector3 m_movementDirection;
    private Vector3 m_aimDirection;

    // Start is called before the first frame update
    void Start()
    {
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

            m_rigidbody.velocity = m_transform.forward * m_movementSpeed;
        }

        if (m_aimDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(m_aimDirection, Vector3.up);
            m_turretBaseTransform.rotation = Quaternion.RotateTowards(m_turretBaseTransform.rotation, toRotation, m_rotationSpeed * Time.deltaTime);
        }
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

        Debug.Log("Mouse Aim " + aim.x + ", " + aim.y);
        Debug.Log("Screen Pos " + screenPos.x + ", " + screenPos.y);
    }

    public void OnFireLeft(InputAction.CallbackContext value)
    {

    }

    public void OnFireRight(InputAction.CallbackContext value)
    {

    }

    public void OnDash(InputAction.CallbackContext value)
    {

    }
}
