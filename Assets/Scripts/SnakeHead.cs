using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SnakeHead : MonoBehaviour {
	// Canvas作为父组件
	private Transform canvas;

	// 蛇身
	public List<Transform> bodyList = new List<Transform>();
	public GameObject bodyPrefab;
	public Sprite[] bodySprites = new Sprite[2];

	public float velocity = 0.2f;

	public int step;
	private int x, y;
	private Vector3 headPos;

	private bool isDead = false;
	public GameObject deadEffect;

	// 声音
	public AudioClip eatClip;
	public AudioClip dieClip;


	void Awake()
    {
		canvas = GameObject.Find("Canvas").transform;

		gameObject.GetComponent<Image>().sprite = Resources.Load<Sprite>(PlayerPrefs.GetString("sh", "sh02"));
		bodySprites[0] = Resources.Load<Sprite>(PlayerPrefs.GetString("sb01", "sb0201"));
		bodySprites[1] = Resources.Load<Sprite>(PlayerPrefs.GetString("sb02", "sb0202"));
	}

	void Start() {
		x = 0; y = step;
		InvokeRepeating("Move", 0, velocity);
	}

	void Update() {
		PlayerInput();
	}

	void PlayerInput() {
		if (MainUIController.Instance.isPause || isDead) {
			return;
		}
		if (Input.GetKeyDown(KeyCode.Space))
		{
			CancelInvoke();
			InvokeRepeating("Move", 0, velocity - 0.1f);
		}
		if (Input.GetKeyUp(KeyCode.Space))
		{
			CancelInvoke();
			InvokeRepeating("Move", 0, velocity);
		}
		if (Input.GetKey(KeyCode.W) && y != -step)
		{
			x = 0; y = step;
			gameObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
		}
		else if (Input.GetKey(KeyCode.S) && y != step)
		{
			x = 0; y = -step;
			gameObject.transform.localRotation = Quaternion.Euler(0, 0, 180);
		}
		else if (Input.GetKey(KeyCode.A) && x != step)
		{
			x = -step; y = 0;
			gameObject.transform.localRotation = Quaternion.Euler(0, 0, 90);
		}
		else if (Input.GetKey(KeyCode.D) && x != -step)
		{
			x = step; y = 0;
			gameObject.transform.localRotation = Quaternion.Euler(0, 0, -90);
		}
	}

	void Move() {
		// 蛇头动
		headPos = gameObject.transform.localPosition;
		gameObject.transform.localPosition = new Vector3(headPos.x + x, headPos.y + y, headPos.z);

		// 蛇身动
		if (bodyList.Count > 0) {
            for (int i = bodyList.Count - 1; i >= 0; i--)
            {
				if (i == 0)
				{
					bodyList[i].localPosition = headPos;
				}
				else {
					bodyList[i].localPosition = bodyList[i - 1].localPosition;
				}
            }
		}
	}

	void Grow() {
		// 确定蛇身的颜色
		int index = bodyList.Count % 2;
		AudioSource.PlayClipAtPoint(eatClip, Vector3.zero);

		// 先实例化一个GameObject
		GameObject body = Instantiate(bodyPrefab, new Vector3(2000, 2000, 0), Quaternion.identity);
		body.GetComponent<Image>().sprite = bodySprites[index];
		body.transform.SetParent(canvas, false);
		bodyList.Add(body.transform);
	}

	void Die() {
		CancelInvoke();
		isDead = true;
		Instantiate(deadEffect);
		AudioSource.PlayClipAtPoint(dieClip, Vector3.zero);

		string lastLength = "lastLength", lastScore = "lastScore";
		string bestLength = "bestLength", bestScore = "bestScore";
		// 记录分数与长度
		PlayerPrefs.SetInt(lastLength, MainUIController.Instance.length);
		PlayerPrefs.SetInt(lastScore, MainUIController.Instance.score);
		if (PlayerPrefs.GetInt(bestScore, 0) < PlayerPrefs.GetInt(lastScore)) {
			PlayerPrefs.SetInt(bestLength, PlayerPrefs.GetInt(lastLength));
			PlayerPrefs.SetInt(bestScore, PlayerPrefs.GetInt(lastScore));
		}

		StartCoroutine(GameOver(1.5f));
	}

	IEnumerator GameOver(float t) {
		yield return new WaitForSeconds(t);

		SceneManager.LoadScene(1);
	}

	private void OnTriggerEnter2D(Collider2D collsion) {
		if (collsion.tag == "Food")
		{
			Destroy(collsion.gameObject);
			Grow();
			FoodMaker.Instance.MakeFood(Random.Range(0, 100) < 20 ? true : false);
			
			MainUIController.Instance.UpdateUI();

		}
		else if (collsion.tag == "SnakeBody")
		{
			Die();
		}
        else if (collsion.tag == "Reward")
        {
			Destroy(collsion.gameObject);
			Grow();
			MainUIController.Instance.UpdateUI(Random.Range(0, 10)*50);

		}
		else {
			if (MainUIController.Instance.hasBorder)
			{
				Die();
			}
			else {
				switch (collsion.gameObject.name)
				{
					case "Up":
						transform.localPosition = new Vector3(
							transform.localPosition.x,
							-transform.localPosition.y + 25,
							transform.localPosition.z);
						break;
					case "Down":
						transform.localPosition = new Vector3(
							transform.localPosition.x,
							-transform.localPosition.y - 25,
							transform.localPosition.z);
						break;
					case "Left":
						transform.localPosition = new Vector3(
							-transform.localPosition.x - 25 + 8 * 25,
							transform.localPosition.y,
							transform.localPosition.z);
						break;
					case "Right":
						transform.localPosition = new Vector3(
							-transform.localPosition.x + 25 + 8 * 25,
							transform.localPosition.y,
							transform.localPosition.z);
						break;
					default:
						break;
				}
			}
            
        }
	}
}
