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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public virtual void StartFiring() { }

    public virtual void StopFiring() { }

    public virtual void Fire() { }
}
