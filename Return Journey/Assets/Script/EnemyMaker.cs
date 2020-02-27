using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMaker : MonoBehaviour
{
    public List<GameObject> enemyPrefabList;
    public float roate = 0.7f;
    public int maxEnemyCount = 10;
    public List<Vector3> targetList;

    private float time = 0;
    private bool making = false;
    public List<GameObject> enemyList = new List<GameObject>();

    public int damage = 10;
    public int HP = 100;

    private void Start()
    {
        MakeEnemy();
    }

    private void Update()
    {
        if (making)
        {
            time += Time.deltaTime;
            if (time > roate && enemyList.Count < maxEnemyCount)
            {
                int index = (int)(Random.value * targetList.Count);//为生成的enemy设置目标点
                GameObject enemy = GameObject.Instantiate(enemyPrefabList[(int)(Random.value * enemyPrefabList.Count)], transform);
                enemy.GetComponent<EnemyControl>().tarPos = targetList[index];
                enemy.GetComponent<EnemyControl>().damage = this.damage;
                enemy.GetComponent<EnemyControl>().ChangeHP(this.HP);
                enemyList.Add(enemy);
                time = 0;
            }
            if(enemyList.Count >= maxEnemyCount)
            {
                making = false;
            }
        }
    }

    public void MakeEnemy()
    {
        making = true;
    }

    public void DeleteEnemyInList(GameObject enemy)
    {
        enemyList.Remove(enemy);
    }

    public void Attack()
    {
        foreach(GameObject e in enemyList)
        {
            EnemyControl enemyControl = e.GetComponent<EnemyControl>();
            enemyControl.tarPos = targetList[targetList.Count - 1];//村庄
            enemyControl.AttackVillage();
        }
    }
}
