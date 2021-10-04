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
    public GameObject m_root;
    public ParticleSystem m_destructionParticleSystem;
    public MeshRenderer m_healthBar;
    public GameObject m_meshGameObject;
    public GameObject m_healthBarGameObject;
    public AudioSource m_destructionAudioSource;
    public int m_powerDraw = 0;

    private float m_hitPoints = 0.0f;
    private Collider[] m_colliderBuffer;
    private GameObject m_trackedTarget = null;
    private Transform m_transform;
    
    // Start is called before the first frame update
    void Start()
    {
        if (m_destructionAudioSource)
            m_destructionAudioSource.pitch = m_destructionAudioSource.pitch + Random.Range(-0.2f, 0.2f);

        m_hitPoints = m_maxHitPoints;
        m_transform = GetComponent<Transform>();
        m_colliderBuffer = new Collider[32];
        m_weapon.m_effectiveLayer = m_effectiveLayer;
        float layer = Mathf.Log(m_bulletLayer.value, 2);
        m_weapon.SetLayer((int)layer);
        StartCoroutine("CheckForNearestTarget");
    }

    // Update is called once per frame
    void Update()
    {
        if (Globals.sharedInstance.m_isGameOver)
        {
            m_weapon.StopFiring();
            return;
        }

        if (m_hitPoints > 0.0f)
        {
            if (m_trackedTarget != null && m_trackedTarget.activeInHierarchy)
            {
                Vector3 toTarget = (m_trackedTarget.transform.position - transform.position);

                if (toTarget.magnitude < m_effectiveRadius)
                {
                    Vector3 aimDirection = toTarget.normalized;
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
        else
            m_weapon.StopFiring();
    }

    public void TakeDamage(float dmg)
    {
        if (m_root && m_destructionParticleSystem && m_hitPoints > 0.0f)
        {
            m_hitPoints -= dmg;

            if (m_hitPoints <= 0.0f)
            {
                m_hitPoints = 0.0f;
                StartCoroutine("EmitDestructionParticles");
            }

            m_healthBar.material.SetFloat("Health", m_hitPoints / m_maxHitPoints);
        }
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

            //if (m_trackedTarget != null)
            //    Debug.Log("Tracked Target: " + m_trackedTarget.name);

            yield return new WaitForSeconds(1.0f);
        }
    }

    IEnumerator EmitDestructionParticles()
    {
        m_destructionParticleSystem.Play();

        if (!m_destructionAudioSource.isPlaying)
            m_destructionAudioSource.Play();

        yield return new WaitForSeconds(0.5f);
        PowerPlantController.sharedInstance.DecreasePowerDraw(m_powerDraw);

        m_meshGameObject.SetActive(false);
        m_healthBarGameObject.SetActive(false);

        yield return new WaitForSeconds(m_destructionAudioSource.clip.length);

        Destroy(m_root);
    }
}
