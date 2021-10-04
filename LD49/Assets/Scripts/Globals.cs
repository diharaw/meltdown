using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Globals : MonoBehaviour
{
    public static Globals sharedInstance;

    public bool m_isPaused = true;
    public bool m_isGameOver = false;
    public int m_level = 1;
    public int m_currentMaxUnlockedTurretIndex = 0;
    public float m_xp = 0;
    public float m_xpIncrementPerKill = 50.0f;

    private float[] m_xpLut;

    // Start is called before the first frame update
    void Awake()
    {
        m_xpLut = new float[10];

        m_xpLut[0] = 0.0f;
        m_xpLut[1] = 500.0f;
        m_xpLut[2] = 1000.0f;
        m_xpLut[3] = 2000.0f;
        m_xpLut[4] = 5000.0f;
        m_xpLut[5] = 7500.0f;
        m_xpLut[6] = 10000.0f;
        m_xpLut[7] = 15000.0f;
        m_xpLut[8] = 20000.0f;
        m_xpLut[9] = 25000.0f;
        
        sharedInstance = this;
    }

    public void AddXp()
    {
        m_xp += m_xpIncrementPerKill;

        if (m_level < 10 && m_xp >= m_xpLut[m_level])
        {
            m_level++;
            VoiceQuips.sharedInstance.PlayLevelUpClip();
            unlockUpgrades();
        }

        UIController.sharedInstance.UpdateXpBar(xpBarValue());
        UIController.sharedInstance.UpdateLevelTxt(m_level);
        UIController.sharedInstance.UpdateScoreTxt((int)m_xp);
    }

    public float xpBarValue()
    {
        if (m_level < 10)
            return (m_xp - m_xpLut[m_level - 1]) / (m_xpLut[m_level] - m_xpLut[m_level - 1]);
        else
            return 0;
    }

    private void unlockUpgrades()
    {
        if (m_level > 1)
        {
            // Unlock Energy Weapon
            UIController.sharedInstance.m_playerEnergyCannon.SetActive(true);

            // Unlock Minigun Turret
            m_currentMaxUnlockedTurretIndex = 1;
            UIController.sharedInstance.m_turretMinigun.SetActive(true);
            UIController.sharedInstance.m_disabledTurretMinigun.SetActive(false);
        }
        if (m_level > 2)
        {
            // Unlock Mine
            UIController.sharedInstance.m_mine.SetActive(true);
            UIController.sharedInstance.m_disabledMine.SetActive(false);

            // Unlock Double Cannon Turret
            m_currentMaxUnlockedTurretIndex = 2;
            UIController.sharedInstance.m_turretDualCannon.SetActive(true);
            UIController.sharedInstance.m_disabledTurretDualCannon.SetActive(false);
        }
        if (m_level > 3)
        {
            // Unlock Energy Turret
            m_currentMaxUnlockedTurretIndex = 3;
            UIController.sharedInstance.m_turretEnergyCannon.SetActive(true);
            UIController.sharedInstance.m_disabledTurretEnergyCannon.SetActive(false);
        }
    }
}
