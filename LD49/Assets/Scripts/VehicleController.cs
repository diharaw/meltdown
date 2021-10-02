using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    public ParticleSystem m_destructionParticleSystem;
    public float m_maxHitPoints = 100.0f;
    public float m_movementSpeed = 10.0f;

    protected float m_currentHitPoints;

    public virtual void TakeDamage(float damage)
    {
        m_currentHitPoints -= damage;

        if (m_currentHitPoints < 0.0f)
        {
            m_currentHitPoints = 0.0f;
            StartCoroutine("EmitDestructionParticles");
        }
    }

    protected IEnumerator EmitDestructionParticles()
    {
        m_destructionParticleSystem.Play();
        yield return new WaitForSeconds(0.5f);
        gameObject.SetActive(false);
    }
}
