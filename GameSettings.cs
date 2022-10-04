using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSettings : MonoBehaviour
{
    public Toggle redCpu, redHuman;
    public Toggle blueCpu, blueHuman;
    public Toggle greenCpu, greenHuman;
    public Toggle yellowCpu, yellowHuman;

    void ReadToggle()
    {
        //RED - 0
        if (redCpu.isOn)
        {
            SaveSettings.players[0] = "CPU";
        }
        else if (redHuman.isOn)
        {
            SaveSettings.players[0] = "HUMAN";
        }
        //Blue - 1
        if (blueCpu.isOn)
        {
            SaveSettings.players[1] = "CPU";
        }
        else if (blueHuman.isOn)
        {
            SaveSettings.players[1] = "HUMAN";
        }
        //Green - 2
        if (greenCpu.isOn)
        {
            SaveSettings.players[2] = "CPU";
        }
        else if (greenHuman.isOn)
        {
            SaveSettings.players[2] = "HUMAN";
        }
        //Yellow - 3
        if (yellowCpu.isOn)
        {
            SaveSettings.players[3] = "CPU";
        }
        else if (yellowHuman.isOn)
        {
            SaveSettings.players[3] = "HUMAN";
        }
    }

    public void StartGame(string sceneName)
    {
        ReadToggle();
        SceneManager.LoadScene(sceneName);
    }

}

public static class SaveSettings
{
    public static string[] players = new string[4];
}