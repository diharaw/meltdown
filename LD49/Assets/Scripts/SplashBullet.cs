using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplashBullet : Bullet
{
    public float m_effectiveRadius = 5.0f;

    private Collider[] m_colliderBuffer;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        m_colliderBuffer = new Collider[32];
    }

    protected override void OnCollisionEnter(Collision collision)
    {
        if ((1 << collision.gameObject.layer) == m_effectiveLayer.value)
        {
            int numColliders = Physics.OverlapSphereNonAlloc(transform.position, m_effectiveRadius, m_colliderBuffer, m_effectiveLayer.value);

            for (int i = 0; i < numColliders; i++)
            {
                VehicleController controller = m_colliderBuffer[i].gameObject.GetComponent<VehicleController>();
                controller.TakeDamage(m_damage);
            }
        }
    }
}
