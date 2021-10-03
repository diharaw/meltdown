using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public static UIController sharedInstance;

    public Image m_imgReactorStabilityFill;
    public Image m_imgXpFill;
    public Image m_imgHealthFill;
    public Image m_imgPowerDrawFill;

    public Image m_imgPlayerCannon;
    public Image m_imgPlayerDualCannon;
    public Image m_imgPlayerMinigun;
    public Image m_imgPlayerEnergyCannon;

    public Image m_imgTurretCannon;
    public Image m_imgTurretDualCannon;
    public Image m_imgTurretMinigun;
    public Image m_imgTurretEnergyCannon;

    public Image m_imgDisabledTurretCannon;
    public Image m_imgDisabledTurretDualCannon;
    public Image m_imgDisabledTurretMinigun;
    public Image m_imgDisabledTurretEnergyCannon;

    public GameObject m_instructionPanel1;
    public GameObject m_instructionPanel2;
    public Button m_btnInstructionPanel1Next;
    public Button m_btnInstructionPanel12Start;
    public Button m_btnInstructionPanel12Back;

    public GameObject m_gameOverPanel;
    public Button m_btnRestart;
    public Button m_btnQuit;
    public TMP_Text m_txtGameOverScore;

    public TMP_Text m_txtWaveNumber;
    public TMP_Text m_txtScore;
    public TMP_Text m_txtLevel;
    public TMP_Text m_txtScrap;

    void Awake()
    {
        sharedInstance = this;
    }

    public void UpdateHealthBar(float value)
    {
        UIController.sharedInstance.m_imgHealthFill.fillAmount = value;
        UIController.sharedInstance.m_imgHealthFill.color = Color.red;
    }

    public void UpdateXpBar(float value)
    {
        UIController.sharedInstance.m_imgXpFill.fillAmount = value;
        UIController.sharedInstance.m_imgXpFill.color = Color.white;
    }

    public void UpdateScoreTxt(int score)
    {
        UIController.sharedInstance.m_txtScore.text = score.ToString();
    }

    public void UpdateWaveTxt(int wave)
    {
        UIController.sharedInstance.m_txtWaveNumber.text = wave.ToString();
    }

    public void UpdateStabilityBar(float value)
    {
        UIController.sharedInstance.m_imgReactorStabilityFill.fillAmount = value;
        UIController.sharedInstance.m_imgReactorStabilityFill.color = Lerp3(Color.red, Color.yellow, Color.green, value);
    }

    public void UpdatePowerDrawBar(float value)
    {
        UIController.sharedInstance.m_imgPowerDrawFill.fillAmount = value;
        UIController.sharedInstance.m_imgPowerDrawFill.color = Color.blue;
    }

    public void UpdateScrapTxt(int scrap)
    {
        UIController.sharedInstance.m_txtScrap.text = scrap.ToString();
    }

    Color Lerp3(Color a, Color b, Color c, float t)
    {
        if (t < 0.5f) // 0.0 to 0.5 goes to a -> b
            return Color.Lerp(a, b, t / 0.5f);
        else // 0.5 to 1.0 goes to b -> c
            return Color.Lerp(b, c, (t - 0.5f) / 0.5f);
    }
}
