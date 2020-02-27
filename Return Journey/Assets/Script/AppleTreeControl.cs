using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AppleTreeControl : MonoBehaviour
{
    private float time = 0;
    private float bearTime;
    public GameObject apple;
    private List<Vector3> pos = new List<Vector3>();

    private void Start()
    {
        bearTime = Random.value * 10 + 300;
        Vector3 temp = this.transform.position;
        float len = 2;
        pos.Add(new Vector3(temp.x, 0, temp.z + len));
        pos.Add(new Vector3(temp.x, 0, temp.z - len));
        pos.Add(new Vector3(temp.x + len, 0, temp.z));
        pos.Add(new Vector3(temp.x - len, 0, temp.z));
    }

    void Update()
    {
        time += Time.deltaTime;
        if (time > bearTime)
        {
            time = 0;
            MakeApple();
        }
    }

    private void MakeApple()
    {
        int index = (int)(Random.value * 4);
        GameObject newApple = GameObject.Instantiate(apple, this.transform);
        newApple.transform.position = pos[index];
        Destroy(newApple, 12 * 60);//一天内删除苹果
    }
}
