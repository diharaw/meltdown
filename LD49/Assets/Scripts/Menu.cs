using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public GameObject m_menuPanel;
    public GameObject m_optionsPanel;
    public GameObject m_creditsPanel;
    public Slider m_mouseSensitivitySlider;

    public void OnStartClicked()
    {
        SceneManager.LoadScene(1);
    }

    public void OnOptionsClicked()
    {
        float mouseSensitivity = PlayerPrefs.GetFloat("MouseSensitivity", 0.0f);

        m_mouseSensitivitySlider.value = mouseSensitivity;

        m_menuPanel.SetActive(false);
        m_optionsPanel.SetActive(true);
    }

    public void OnCreditsClicked()
    {
        m_menuPanel.SetActive(false);
        m_creditsPanel.SetActive(true);
    }

    public void OnQuitClicked()
    {
        Application.Quit(0);
    }

    public void OnOptionsBackClicked()
    {
        m_menuPanel.SetActive(true);
        m_optionsPanel.SetActive(false);

        PlayerPrefs.SetFloat("MouseSensitivity", m_mouseSensitivitySlider.value);
        PlayerPrefs.Save();
    }

    public void OnCreditsBackClicked()
    {
        m_menuPanel.SetActive(true);
        m_creditsPanel.SetActive(false);
    }
}
