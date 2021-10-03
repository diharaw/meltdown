using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{
    public float m_effectiveRadius = 1.0f;
    public float m_maxHitPoints = 100.0f;
    public float m_rotationSpeed = 10.0f;
    public LayerMask m_effectiveLayer;
    public LayerMask m_bulletLayer;
    public WeaponController m_weapon;

    private float m_hitPoints = 0.0f;
    private Collider[] m_colliderBuffer;
    private GameObject m_trackedTarget = null;
    private Transform m_transform;

    // Start is called before the first frame update
    void Start()
    {
        m_hitPoints = m_maxHitPoints;
        m_transform = GetComponent<Transform>();
        m_colliderBuffer = new Collider[32];
        m_weapon.m_effectiveLayer = m_effectiveLayer;
        float layer = Mathf.Log(m_bulletLayer.value, 2);
        m_weapon.SetLayer((int) layer);
        StartCoroutine("CheckForNearestTarget");
    }

    // Update is called once per frame
    void Update()
    {
        if (m_hitPoints > 0.0f)
        {
            if (m_trackedTarget != null && m_trackedTarget.activeInHierarchy)
            {
                Vector3 aimDirection = (m_trackedTarget.transform.position - transform.position).normalized;
                Quaternion toRotation = Quaternion.LookRotation(aimDirection, Vector3.up);
                m_transform.rotation = Quaternion.RotateTowards(m_transform.rotation, toRotation, m_rotationSpeed * Time.deltaTime);

                if (!m_weapon.isFiring())
                    m_weapon.StartFiring();
            }
            else
                m_weapon.StopFiring();
        }
        else
            m_weapon.StopFiring();
    }

    IEnumerator CheckForNearestTarget()
    {
        while (m_hitPoints > 0.0f)
        {
            int numColliders = Physics.OverlapSphereNonAlloc(transform.position, m_effectiveRadius, m_colliderBuffer, m_effectiveLayer.value);

            float closestDistance = Mathf.Infinity;

            for (int i = 0; i < numColliders; i++)
            {
                float currentDistance = Vector3.Distance(m_colliderBuffer[i].gameObject.transform.position, m_transform.position);

                if (currentDistance < closestDistance)
                {
                    closestDistance = currentDistance;
                    m_trackedTarget = m_colliderBuffer[i].gameObject;
                }
            }

            yield return new WaitForSeconds(1.0f);
        }
    }
}
