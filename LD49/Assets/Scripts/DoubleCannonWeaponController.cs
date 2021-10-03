using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleCannonWeaponController : WeaponController
{
    public ParticleSystem m_particleSystem2;
    public Transform m_muzzleTip2;

    private Animator m_animator;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void StartFiring()
    {
        if (!m_animator)
            m_animator = GetComponent<Animator>();

        if (!m_isFiring)
        {
            if (m_animator)
                m_animator.SetBool("isFiring", true);
            m_isFiring = true;
        }
    }

    public override void StopFiring()
    {
        if (m_animator)
            m_animator.SetBool("isFiring", false);
        m_isFiring = false;
    }

    public override void Fire()
    {
        m_particleSystem.Play();
        m_particleSystem2.Play();
        m_audioSource.Play();

        {
            GameObject bullet = BulletPool.sharedInstance.GetPooledCannonBullet();
            bullet.transform.position = m_muzzleTip.position;
            bullet.transform.rotation = m_muzzleTip.rotation;
            bullet.GetComponent<Bullet>().m_effectiveLayer = m_effectiveLayer;
            bullet.layer = gameObject.layer;
        }

        {
            GameObject bullet = BulletPool.sharedInstance.GetPooledCannonBullet();
            bullet.transform.position = m_muzzleTip2.position;
            bullet.transform.rotation = m_muzzleTip2.rotation;
            bullet.GetComponent<Bullet>().m_effectiveLayer = m_effectiveLayer;
            bullet.layer = gameObject.layer;
        }
    }
}
