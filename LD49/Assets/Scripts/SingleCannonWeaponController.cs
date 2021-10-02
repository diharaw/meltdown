using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleCannonWeaponController : WeaponController
{
    private Animator m_animator;
    
    // Start is called before the first frame update
    void Start()
    {
        m_animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void StartFiring()
    {
        m_animator.SetBool("isFiring", true);
    }

    public override void StopFiring()
    {
        m_animator.SetBool("isFiring", false);
    }

    public override void Fire()
    {
        m_particleSystem.Play();
        m_audioSource.Play();
        GameObject bullet = BulletPool.sharedInstance.GetPooledBullet();
        bullet.transform.position = m_muzzleTip.position;
        bullet.transform.rotation = m_muzzleTip.rotation;
        bullet.GetComponent<Bullet>().m_effectiveLayer = m_effectiveLayer;
    }
}
