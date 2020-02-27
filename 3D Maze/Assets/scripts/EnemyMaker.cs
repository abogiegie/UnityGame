using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyMaker : MonoBehaviour
{
    private int enemyCount = 10;
    MapMaker mapMaker;
    public GameObject enemy;
    private List<GameObject> enemyList = new List<GameObject>();
    GameObject treasure;
    Text treasureCount;

    private const string word = "敌人数量：";

    private void Awake()
    {
        mapMaker = gameObject.GetComponent<MapMaker>();
        treasure = GameObject.Find("Treasure");
        int temp = (int)(mapMaker.endList.Count * Random.value);
        treasure.transform.position = mapMaker.endList[temp];
        treasure.SetActive(false);

        treasureCount = GameObject.Find("TreasureCount").GetComponentInChildren<Text>();
        UpdateUI();
    }

    void Start()
    {
        for(int i = 0;i < enemyCount; i++)
        {
            GameObject enemyObject = Instantiate(enemy, mapMaker.endList[i], new Quaternion(0, 0, 0, 0));
            enemyList.Add(enemyObject);
        }
    }

    public void DelAllEnemy()
    {
        foreach(GameObject enemy in enemyList)
        {
            if(enemy != null)
                enemy.GetComponent<Enemy>().GetDamage(100);
        }
        enemyList.Clear();
    }

    private void UpdateUI()
    {
        if (treasureCount != null)
        {
            treasureCount.text = word + enemyCount;
        }
    }

    public void DelEnemy(GameObject theEnemy)
    {
        enemyList.Remove(theEnemy);
        enemyCount--;
        UpdateUI();
        if (enemyCount <= 0)
        {
            treasure.SetActive(true);
        }
    }
}
