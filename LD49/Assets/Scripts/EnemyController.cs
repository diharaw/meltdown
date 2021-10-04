using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class EnemyController : VehicleController
{
    public WaveController m_waveController;
    public AudioSource m_engineAudioSource;
    
    private NavMeshAgent m_agent;
    private float m_targetRadius = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_currentHitPoints = m_maxHitPoints;
        m_agent = GetComponent<NavMeshAgent>();
        m_destructionAudioSource = GetComponent<AudioSource>();
        m_destructionAudioSource.pitch = m_destructionAudioSource.pitch + Random.Range(-0.2f, 0.2f);
        m_engineAudioSource.pitch = m_destructionAudioSource.pitch + Random.Range(-0.2f, 0.2f);

        Vector2 rnd = Random.insideUnitCircle;
        Vector3 offset = new Vector3(rnd.x, 0.0f, rnd.y).normalized * m_targetRadius;

        m_agent.speed = m_movementSpeed;
        m_agent.SetDestination(new Vector3(0.0f, 0.0f, 0.0f) + offset);
    }

    public override void TakeDamage(float damage)
    {
        m_currentHitPoints -= damage;

        if (m_currentHitPoints < 0.0f)
        {
            m_currentHitPoints = 0.0f;
            m_waveController.DecrementRemainingEnemies();
            Globals.sharedInstance.AddXp();
            StartCoroutine("EmitDestructionParticles");
        }
    }
}
