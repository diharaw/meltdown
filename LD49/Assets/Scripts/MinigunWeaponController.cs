using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigunWeaponController : WeaponController
{
    private Animator m_animator;
    private bool m_isFiring = false;

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
        m_isFiring = true;
        m_particleSystem.Play();
        m_audioSource.Play();

        StartCoroutine("FireBullets");
    }

    public override void StopFiring()
    {
        m_animator.SetBool("isFiring", false);
        m_particleSystem.Stop();
        m_audioSource.Stop();
        m_isFiring = false;
    }

    IEnumerator FireBullets()
    {
        while (m_isFiring)
        {
            GameObject bullet = BulletPool.sharedInstance.GetPooledMinigunBullet();
            bullet.transform.position = m_muzzleTip.position;
            bullet.transform.rotation = m_muzzleTip.rotation;
            bullet.GetComponent<Bullet>().m_effectiveLayer = m_effectiveLayer;
            yield return new WaitForSeconds(m_timeBetweenShots);
        }
    }
}
