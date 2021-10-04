using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPlantController : MonoBehaviour
{
    public static PowerPlantController sharedInstance;

    public float m_maxHitPoints = 1000.0f;
    public float m_maxPowerDraw = 100.0f;
    public float m_maxPowerDrawDecayRate = 10.0f;
    public float m_baseStabilityDecayRate = 10.0f;
    public ParticleSystem m_destructionParticleSystem;
    public AudioSource m_destructionAudioSource;
    public GameObject m_powerPlantMesh;
    public ReactorState m_reactorState;

    private float m_hitPoints;
    private float m_stabilityDecayRate = 10.0f;
    private float m_powerDraw = 0.0f;

    void Awake()
    {
        sharedInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        m_hitPoints = m_maxHitPoints;
        TakeDamage(0);
        UIController.sharedInstance.UpdateStabilityBar(m_hitPoints / m_maxHitPoints);
        UIController.sharedInstance.UpdatePowerDrawBar(m_powerDraw / m_maxPowerDraw);
        StartCoroutine("DecrementHitPoints");
    }

    public bool isDestroyed()
    {
        return m_hitPoints <= 0.0f;
    }

    public void TakeDamage(float damageAmount)
    {
        if (!isDestroyed())
        {
            m_hitPoints -= damageAmount;

            if (m_hitPoints <= 0.0f)
            {
                m_hitPoints = 0.0f;
                m_destructionParticleSystem.Play();
                m_destructionAudioSource.Play();
                m_powerPlantMesh.SetActive(false);
                Globals.sharedInstance.m_isGameOver = true;
                UIController.sharedInstance.m_gameOverPanel.SetActive(true);
                UIController.sharedInstance.m_txtGameOverScore.text = Globals.sharedInstance.m_xp.ToString();

                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
            }

            UIController.sharedInstance.UpdateStabilityBar(m_hitPoints / m_maxHitPoints);
            m_reactorState.SetReactorHealth(m_hitPoints / m_maxHitPoints);
        }
    }

    public void DoRepair(float repairAmount)
    {
        if (!isDestroyed())
        {
            m_hitPoints += repairAmount;

            if (m_hitPoints > m_maxHitPoints)
                m_hitPoints = m_maxHitPoints;

            UIController.sharedInstance.UpdateStabilityBar(m_hitPoints / m_maxHitPoints);
            m_reactorState.SetReactorHealth(m_hitPoints / m_maxHitPoints);
        }
    }

    public bool IncreasePowerDraw(int powerUnits)
    {
        if (m_powerDraw < m_maxPowerDraw)
        {
            m_powerDraw += powerUnits;
            UIController.sharedInstance.UpdatePowerDrawBar(m_powerDraw / m_maxPowerDraw);
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

        UIController.sharedInstance.UpdatePowerDrawBar(m_powerDraw / m_maxPowerDraw);
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

            UIController.sharedInstance.UpdateStabilityBar(m_hitPoints / m_maxHitPoints);

            yield return new WaitForSeconds(1.0f);
        }
    }
}
