using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuitGame : MonoBehaviour
{
    public void RulesPage()
    {
        SceneManager.LoadScene("Rules");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
