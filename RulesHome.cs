using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RulesHome : MonoBehaviour
{
    public void BackToHome()
    {
        SceneManager.LoadScene("Menu");
    }
}
