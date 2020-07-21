using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FoodMaker : MonoBehaviour {
    private static FoodMaker instance;

    public static FoodMaker Instance {
        get {
            return instance;
        }
    }

    // 当贪吃蛇在0 0 0的位置中时，向上可走14步，向下可走14步，向左可走16步，向左可走24步
    private int xlimit = 24;
    private int ylimit = 14;
    private int xoffset = 8;

    public GameObject foodPrefab;
    public Sprite[] foodSprites;

    public GameObject rewardPrefab;

    void Awake() {
        instance = this;
    }

    void Start() {
        MakeFood(false);
    }

    public void MakeFood(bool isReward) {
        // 设置图片
        int index = Random.Range(0, foodSprites.Length);
        foodPrefab.GetComponent<Image>().sprite = foodSprites[index];

        GameObject item;
        int makeIndex = 1;
        if (isReward) {
            makeIndex = 2;
        }
        for (int i = 0; i < makeIndex; i++)
        {
            if (i == 1)
            {
                item = Instantiate(rewardPrefab);
            }
            else {
                item = Instantiate(foodPrefab);
            }
            item.transform.SetParent(gameObject.transform, false);
            int x = Random.Range(-xlimit + xoffset, xlimit);
            int y = Random.Range(-ylimit, ylimit);
            item.transform.localPosition = new Vector3(x * 25, y * 25, 0);
        }
        // 生成蛋糕并且指定位置
        //GameObject food = Instantiate(foodPrefab);
        //food.transform.SetParent(gameObject.transform, false);
        //int x = Random.Range(-xlimit + xoffset, xlimit);
        //int y = Random.Range(-ylimit, ylimit);
        //food.transform.localPosition = new Vector3(x * 25, y * 25, 0);

        //// 如果isReward是true，生成奖励
        //if (isReward) {
        //    GameObject reward = Instantiate(rewardPrefab);
        //    food.transform.SetParent(gameObject.transform, false);
        //}

    }
}
