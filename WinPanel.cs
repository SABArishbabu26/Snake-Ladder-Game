using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinPanel : MonoBehaviour
{
    public Text message;
    public GameObject qusButton;

    void Start()
    {
        DeactivateAllButtons();
    }

    public void ShowWinMessage(string winner, string playerType)
    {
        message.text = winner + " has won this Game!";
        if(playerType == "HUMAN")
        {
            qusButton.SetActive(true);
        }
        else if(playerType == "CPU")
        {
            qusButton.SetActive(false);
        }
    }

    public void BackButton()
    {
        SceneManager.LoadScene("Menu");
    }

    public void QuestionButton()
    {
        SceneManager.LoadScene("Questions");
    }

    void DeactivateAllButtons()
    {

        qusButton.SetActive(false);

    }
}
