using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstructionPanel : MonoBehaviour
{
    void Start()
    {
        UIController.sharedInstance.m_instructionPanel1.SetActive(true);
        Globals.sharedInstance.m_isPaused = true;

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void OnNextClicked()
    {
        UIController.sharedInstance.m_instructionPanel1.SetActive(false);
        UIController.sharedInstance.m_instructionPanel2.SetActive(true);
    }

    public void OnBackClicked()
    {
        UIController.sharedInstance.m_instructionPanel1.SetActive(true);
        UIController.sharedInstance.m_instructionPanel2.SetActive(false);
    }

    public void OnStartClicked()
    {
        UIController.sharedInstance.m_instructionPanel2.SetActive(false);
        Globals.sharedInstance.m_isPaused = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        WaveController.sharedInstance.StartSpawning();
    }
}