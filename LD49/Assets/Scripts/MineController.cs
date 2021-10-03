using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineController : MonoBehaviour
{
    public float m_effectiveRadius = 5.0f;
    public float m_damage = 200.0f;

    private Collider[] m_colliderBuffer;
    public ParticleSystem m_particleSystem;

    // Start is called before the first frame update
    void Start()
    {
        m_colliderBuffer = new Collider[32];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("AI"))
        {
            int numColliders = Physics.OverlapSphereNonAlloc(transform.position, m_effectiveRadius, m_colliderBuffer, 1 << LayerMask.NameToLayer("AI"));

            for (int i = 0; i < numColliders; i++)
            {
                VehicleController controller = m_colliderBuffer[i].gameObject.GetComponent<VehicleController>();
                controller.TakeDamage(m_damage);
            }

            m_particleSystem.Play();
            
            Destroy(gameObject, 0.5f);
        }
    }
}
