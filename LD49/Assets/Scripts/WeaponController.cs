using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public ParticleSystem m_particleSystem;
    public Transform m_muzzleTip;
    public AudioSource m_audioSource;
    public float m_timeBetweenShots = 1.0f;
    public LayerMask m_effectiveLayer;
 
    protected bool m_isFiring = false;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        m_audioSource.pitch = m_audioSource.pitch + Random.Range(-0.2f, 0.2f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetLayer(int _layer)
    {
        gameObject.layer = _layer;
    }

    public bool isFiring()
    {
        return m_isFiring;
    }

    public virtual void StartFiring() 
    {
        m_isFiring = true;
    }

    public virtual void StopFiring() 
    {
        m_isFiring = false;
    }

    public virtual void Fire() { }
}
