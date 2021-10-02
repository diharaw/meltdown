using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleCannonTurretController : TurretController
{
    public Animator m_animator;

    // Start is called before the first frame update
    void Start()
    {
        m_animator.StopPlayback();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Fire()
    {

    }

    public void StartFiring()
    {
        m_animator.StartPlayback();
    }

    public void StopFiring()
    {

    }
}
