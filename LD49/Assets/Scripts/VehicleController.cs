using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleController : MonoBehaviour
{
    public ParticleSystem m_destructionParticleSystem;
    public GameObject m_meshGameObject;
    public float m_maxHitPoints = 100.0f;
    public float m_movementSpeed = 10.0f;

    protected float m_currentHitPoints;
    protected AudioSource m_destructionAudioSource;

    public virtual void TakeDamage(float damage)
    {
        if (m_currentHitPoints > 0.0f)
        {
            m_currentHitPoints -= damage;

            if (m_currentHitPoints <= 0.0f)
            {
                m_currentHitPoints = 0.0f;
                StartCoroutine("EmitDestructionParticles");
            }
        }
    }

    protected IEnumerator EmitDestructionParticles()
    {
        m_destructionParticleSystem.Play();

        if (!m_destructionAudioSource.isPlaying)
            m_destructionAudioSource.Play();
        
        yield return new WaitForSeconds(0.5f);

        m_meshGameObject.SetActive(false);
        
        yield return new WaitForSeconds(m_destructionAudioSource.clip.length);

        // If this is a non-player vehicle (i.e AI) drop pickups
        if (gameObject.tag != "Player")
        {
            float pickupRandomRadius = 2.0f;

            GameObject coolant = PickupPool.sharedInstance.GetPooledCoolantPickup();
            coolant.transform.position = gameObject.transform.position + new Vector3(Random.Range(-pickupRandomRadius, pickupRandomRadius), 0.0f, Random.Range(-pickupRandomRadius, pickupRandomRadius));
            coolant.transform.rotation = gameObject.transform.rotation;

            GameObject scrap = PickupPool.sharedInstance.GetPooledScrapPickup();
            scrap.transform.position = gameObject.transform.position + new Vector3(Random.Range(-pickupRandomRadius, pickupRandomRadius), 0.0f, Random.Range(-pickupRandomRadius, pickupRandomRadius));
            scrap.transform.rotation = gameObject.transform.rotation;
        }

        Destroy(gameObject);
    }
}
