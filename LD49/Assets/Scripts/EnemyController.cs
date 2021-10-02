using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class EnemyController : MonoBehaviour
{
    public WaveController m_waveController;

    private NavMeshAgent m_agent;
    private float m_targetRadius = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();

        Vector2 rnd = Random.insideUnitCircle;
        Vector3 offset = new Vector3(rnd.x, 0.0f, rnd.y).normalized * m_targetRadius;

        m_agent.SetDestination(new Vector3(0.0f, 0.0f, 0.0f) + offset);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
