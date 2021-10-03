using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyCannonWeaponController : WeaponController
{
    private bool m_isFiring = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public override void StartFiring()
    {
        m_isFiring = true;
        StartCoroutine("FireBullets");
    }

    public override void StopFiring()
    {
        m_isFiring = false;
    }

    public override void Fire()
    {
    }

    IEnumerator FireBullets()
    {
        while (m_isFiring)
        {
            m_particleSystem.Play();
            GameObject bullet = BulletPool.sharedInstance.GetPooledEnergyBullet();
            bullet.transform.position = m_muzzleTip.position;
            bullet.transform.rotation = m_muzzleTip.rotation;
            bullet.GetComponent<Bullet>().m_effectiveLayer = m_effectiveLayer;
            yield return new WaitForSeconds(m_timeBetweenShots);
        }
    }
}
