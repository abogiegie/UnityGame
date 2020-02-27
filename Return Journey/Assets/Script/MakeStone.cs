using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeStone : MonoBehaviour
{
    private float len = 95;
    public List<GameObject> stoneList;
    private float time = 1200;
    private float makeTime = 50;

    private void Update()
    {
        time += Time.deltaTime;
        if (time > makeTime)
        {
            Stone((int)(Random.value * 3));
            time -= makeTime;
        }
    }

    private void Stone(int index)
    {
        GameObject stone = GameObject.Instantiate(stoneList[index], this.transform);
        stone.transform.position = new Vector3(transform.position.x - len + len * Random.value * 2, transform.position.y, transform.position.z - 7 + 14 * Random.value);
    }
}
