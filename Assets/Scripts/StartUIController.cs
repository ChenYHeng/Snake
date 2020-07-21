using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class StartUIController : MonoBehaviour {
    public Text lastText;
    public Text bestText;

    public Toggle blue;
    public Toggle yellow;

    public Toggle border;
    public Toggle noBorder;

    public Button go;

    void Awake() {
        lastText.text = "上次：长度" + PlayerPrefs.GetInt("lastLength", 0) + "，分数" + PlayerPrefs.GetInt("lastScore", 0);
        bestText.text = "最好：长度" + PlayerPrefs.GetInt("bestLength", 0) + "，分数" + PlayerPrefs.GetInt("bestScore", 0);

        go.onClick.AddListener(new UnityAction(StartGame));

        blue.onValueChanged.AddListener(BlueSelected);
        yellow.onValueChanged.AddListener(YellowSelected);

        border.onValueChanged.AddListener(BorderSelected);
        noBorder.onValueChanged.AddListener(NoBorderSelected);
    }

    void Start() {
        if (PlayerPrefs.GetString("sh", "sh01") == "sh01")
        {
            blue.isOn = true;
            PlayerPrefs.SetString("sh", "sh01");
            PlayerPrefs.SetString("sb01", "sb0101");
            PlayerPrefs.SetString("sb02", "sb0102");
        }
        else {
            yellow.isOn = true;
            PlayerPrefs.SetString("sh", "sh02");
            PlayerPrefs.SetString("sb01", "sb0201");
            PlayerPrefs.SetString("sb02", "sb0202");
        }
        if (PlayerPrefs.GetInt("border", 1) == 1)
        {
            border.isOn = true;
            PlayerPrefs.SetInt("mode", 1);
        }
        else {
            noBorder.isOn = true;
            PlayerPrefs.SetInt("mode", 0);
        }
    }

    public void BlueSelected(bool isOn) {
        if (isOn) {
            PlayerPrefs.SetString("sh", "sh01");
            PlayerPrefs.SetString("sb01", "sb0101");
            PlayerPrefs.SetString("sb02", "sb0102");
        }
    }

    public void YellowSelected(bool isOn) {
        if (isOn)
        {
            PlayerPrefs.SetString("sh", "sh02");
            PlayerPrefs.SetString("sb01", "sb0201");
            PlayerPrefs.SetString("sb02", "sb0202");
        }
    }

    public void BorderSelected(bool isOn) {
        if (isOn)
        {
            PlayerPrefs.SetInt("mode", 1);
        }
    }

    public void NoBorderSelected(bool isOn) {
        if (isOn)
        {
            PlayerPrefs.SetInt("mode", 0);
        }
    }

    void StartGame() {
        SceneManager.LoadScene(1);
    }
}
