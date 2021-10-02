using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class EnemyController : MonoBehaviour
{
    public WaveController m_waveController;

    private NavMeshAgent m_agent;

    // Start is called before the first frame update
    void Start()
    {
        m_agent = GetComponent<NavMeshAgent>();
        m_agent.SetDestination(new Vector3(0.0f, 0.0f, 0.0f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
