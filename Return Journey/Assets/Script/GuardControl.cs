using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardControl : MonoBehaviour
{
    public GameObject[] archers;
    public List<GameObject> enemyList = new List<GameObject>();
    public int damage = 5;//攻击伤害

    private float time = 0;
    private float attackRate = 1;//攻击间隔一秒



    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Enemy")
        {
            enemyList.Add(other.gameObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Enemy")
        {
            enemyList.Remove(other.gameObject);
        }
    }

    private void Update()
    {
        if (time < attackRate)
        {
            time += Time.deltaTime;
        }
        if (enemyList.Count > 0)
        {
            if (enemyList[0] != null)
            {
                if (time >= attackRate)
                {
                    foreach (GameObject i in archers)
                    {
                        i.GetComponent<Archer>().Attack(enemyList[0]);
                    }
                    time -= attackRate;
                }
            }
            else
            {
                enemyList.Remove(enemyList[0]);
            }
        }
    }

}
