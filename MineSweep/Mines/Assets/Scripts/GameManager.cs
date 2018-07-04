using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour 
{
    public InputField[] InputNumbers;
    public static GameManager Instance;
    [HideInInspector]
    public Mine mine;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        List<string> togglesName = new List<string>();
        togglesName.Add("LowToggle");
        togglesName.Add("MediumToggle");
        togglesName.Add("HighToggle");
        togglesName.Add("CustomToggle");
        foreach (string toggleName in togglesName)
        {
            GameObject btnObj = GameObject.Find(toggleName);
            Toggle tg = btnObj.GetComponent<Toggle>();
            tg.onValueChanged.AddListener(delegate(bool isOn)
            {
                this.OnValueChanged(isOn, btnObj);
            });
        }
        
    }
    void Update() 
    {
        Global.GCloumn = InputNumbers[0].text;
        Global.GRow = InputNumbers[1].text;
        Global.GMineNumber = InputNumbers[2].text;
    }

    public void OnButtonStart() 
    {
        SceneManager.LoadScene("mainScene");
    }

    public void OnValueChanged(bool ison, GameObject sender)
    {
        switch (sender.name)
        {
            case "LowToggle":
                InputNumbers[0].text = "9";
                InputNumbers[1].text = "9";
                InputNumbers[2].text = "10";
                break;
            case "MediumToggle":
                InputNumbers[0].text = "16";
                InputNumbers[1].text = "16";
                InputNumbers[2].text = "40";
                break;
            case "HighToggle":
                InputNumbers[0].text = "30";
                InputNumbers[1].text = "16";
                InputNumbers[2].text = "99";
                break;
            case "CustomToggle":
                InputNumbers[0].text = "";
                InputNumbers[1].text = "";
                InputNumbers[2].text = "";
                break;
            default:
                break;
        }
    }

    public void OnButtonRetry() 
    {
        SceneManager.LoadScene("startScene");
    }
    public void OnButtonExit() 
    {
        Application.Quit();
    }

    public void Win() 
    {
        mine.Stop();
    }

    public void Failed() 
    {
        mine.Stop();
    }
}
