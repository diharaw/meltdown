using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float m_damage = 10.0f;
    public float m_velocity = 20.0f;
    public float m_maxLifeTime = 2.0f;
    public ParticleSystem m_impactParticleSystem;

    private float m_currentVelocity = 0.0f;
    private Transform m_transform;
    private float m_lifeTime = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_lifeTime = 0.0f;
        m_transform = GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_lifeTime >= m_maxLifeTime)
            gameObject.SetActive(false);
        else
        {
            m_lifeTime += Time.deltaTime;
            m_transform.position = m_transform.position + m_transform.forward * m_currentVelocity * Time.deltaTime;
        }
    }

    public void ResetState()
    {
        m_currentVelocity = m_velocity;
        m_lifeTime = 0.0f;
    }

    private void OnCollisionEnter(Collision collision)
    {
        m_currentVelocity = 0.0f;
        StartCoroutine("EmitImpactParticles");
    }

    private void OnTriggerEnter(Collider other)
    {
        m_currentVelocity = 0.0f;
        StartCoroutine("EmitImpactParticles");
    }

    IEnumerator EmitImpactParticles()
    {
        m_impactParticleSystem.Play();
        // TODO: reduce hit points
        yield return new WaitForSeconds(1.0f);
        gameObject.SetActive(false);
    }
}