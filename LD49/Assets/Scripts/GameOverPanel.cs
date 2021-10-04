using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    public void OnRestartClicked()
    {
        SceneManager.LoadScene(1);
    }

    public void OnQuitClicked()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;

        SceneManager.LoadScene(0);
    }
}
