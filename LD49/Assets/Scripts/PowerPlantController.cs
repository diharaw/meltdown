using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPlantController : MonoBehaviour
{
    public float m_maxHitPoints = 1000.0f;
    public float m_maxPowerDraw = 100.0f;
    public float m_maxPowerDrawDecayRate = 10.0f;
    public float m_baseStabilityDecayRate = 10.0f;

    private float m_hitPoints;
    private float m_stabilityDecayRate = 10.0f;
    private float m_powerDraw = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_hitPoints = m_maxHitPoints;
        StartCoroutine("DecrementHitPoints");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TakeDamage(float damageAmount)
    {
        m_hitPoints -= damageAmount;

        if (m_hitPoints <= 0.0f)
        {
            // TODO: play explosion
            // TODO: show game over screen
        }
    }

    public void DoRepair(int repairAmount)
    {
        m_hitPoints += repairAmount;

        if (m_hitPoints > m_maxHitPoints)
            m_hitPoints = m_maxHitPoints;
    }

    public bool IncreasePowerDraw(int powerUnits)
    {
        if (m_powerDraw < m_maxPowerDraw)
        {
            m_powerDraw += powerUnits;
            return true;
        }
        else
            return false;
    }

    public void DecreasePowerDraw(int powerUnits)
    {
        m_powerDraw -= powerUnits;

        if (m_powerDraw < 0.0f)
            m_powerDraw = 0.0f;
    }

    IEnumerator DecrementHitPoints()
    {
        while (true)
        {
            m_stabilityDecayRate = m_baseStabilityDecayRate + (m_powerDraw / m_maxPowerDraw) * m_maxPowerDrawDecayRate;
            m_hitPoints -= m_stabilityDecayRate;

            if (m_hitPoints <= 0.0f)
            {
                // TODO: play explosion
                // TODO: show game over screen
                Debug.Log("Power Plant Exploded!");
            }

            yield return new WaitForSeconds(1.0f);
        }
    }
}
