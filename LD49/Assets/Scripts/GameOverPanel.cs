using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverPanel : MonoBehaviour
{
    public void OnRestartClicked()
    {
        SceneManager.LoadScene(0);
    }

    public void OnQuitClicked()
    {

    }
}
