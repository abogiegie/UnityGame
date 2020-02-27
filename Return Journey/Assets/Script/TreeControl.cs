using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeControl : MonoBehaviour
{
    public GameObject wood;
    private int count = 1;
    float time = 400;
    public float addTime = 300;

    private void Update()
    {
        time += Time.deltaTime;
        if (time > 300)
        {
            count++;
            if (count > 50)
            {
                count = 50;
            }
            time = 0;
        }
    }

    public int MakeWood()
    {
        if (count > 0)
        {
            count--;
            Vector3 temp = new Vector3(Random.value * 2 - 1, 0, Random.value * 2 - 1);
            temp.Normalize();
            temp *= 2;
            GameObject.Instantiate(wood, transform.position + temp, transform.rotation);
        }
        return count;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            if (count > 0)
            {
                other.gameObject.GetComponent<PlayerControl>().gameControl.GetComponent<GameControl>().Prompt("使用剑攻击树木可采集木材", false);
            }
        }
    }
}
