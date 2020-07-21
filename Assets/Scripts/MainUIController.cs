using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainUIController : MonoBehaviour {

	private static MainUIController instance;

	public static MainUIController Instance {
		get { return instance; }
	}

	public int score = 0;
	public int length = 0;

	public Text scoreText;
	public Text lengthText;
	public Text msgText;

	public Image bgImg;
	public bool hasBorder = true;
	private Color tmpColor;

	public Image pauseImg;
	public Button pauseBtn;
	public Sprite[] pauseSprites = new Sprite[2];
	public bool isPause;

	public Button homeBtn;

	void Awake() {
		instance = this;

		pauseBtn.onClick.AddListener(new UnityAction(Pause));
		homeBtn.onClick.AddListener(new UnityAction(Home));

		// 出现暂停后，返回主菜单的时候，再进入蛇无法运动的情况
		Time.timeScale = 1;
	}

	void Start() {
		if (PlayerPrefs.GetInt("mode", 1) == 0)
		{
			hasBorder = false;
			foreach (Transform item in bgImg.gameObject.transform) {
				item.gameObject.GetComponent<Image>().enabled = false;
			}
		}
	}

	void Update() {
        switch (score / 100)
        {
			case 0:
				break;
			case 1:
				break;
			case 2:
				ColorUtility.TryParseHtmlString("#CCEEFFFF", out tmpColor);
				bgImg.color = tmpColor;
				msgText.text = "阶 段 " + 2;
				break;
			case 3:
				ColorUtility.TryParseHtmlString("#CCFFDBFF", out tmpColor);
				bgImg.color = tmpColor;
				msgText.text = "阶 段 " + 3;
				break;
			case 4:
				ColorUtility.TryParseHtmlString("#EBFFCCFF", out tmpColor);
				bgImg.color = tmpColor;
				msgText.text = "阶 段 " + 4;
				break;
			case 5:
				ColorUtility.TryParseHtmlString("#FFF3CCFF", out tmpColor);
				bgImg.color = tmpColor;
				msgText.text = "阶 段 " + 5;
				break;
			case 6:
				ColorUtility.TryParseHtmlString("#FFDACCFF", out tmpColor);
				bgImg.color = tmpColor;
				msgText.text = "无 尽 阶 段";
				break;
        }

		
    }

	public void UpdateUI(int s = 5, int l = 1) {
		score += s;
		length += l;

		scoreText.text = "得 分\n" + score;
		lengthText.text = "长 度\n" + length; 
	}

	public void Pause() {
		isPause = !isPause;
		if (isPause)
		{
			Time.timeScale = 0;
			pauseImg.sprite = pauseSprites[1];
		}
		else {
			Time.timeScale = 1;
			pauseImg.sprite = pauseSprites[0];
		}
	}

	public void Home() {
		SceneManager.LoadScene(0);
	}
}
