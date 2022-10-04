using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoBox : MonoBehaviour
{
    public static InfoBox instance;

    public Text infoText;

    void Awake()
    {
        instance = this;
        infoText.text = "";
        ShowMessage("");
    }

    public void ShowMessage(string _text)
    {
        infoText.text = _text;
    }
}
