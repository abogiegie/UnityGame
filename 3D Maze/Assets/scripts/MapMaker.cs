using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapMaker : MonoBehaviour
{
    public GameObject map;
    public GameObject cubeBlack;
    private string mapTag;
    private int mapLayer;
    private const int size = 21;
    public readonly bool[,,] mapData = new bool[size, size, size];

    private int cubeCount = 0;
    private int smallCubeCount = size * size * size;

    public int GetCenter()
    {
        return center;
    }

    private void MakeCube(float[] pos, float scaleZ)
    {
        if(pos.Length == 3)
        {
            Vector3 avgPosition = new Vector3(pos[0], pos[1], pos[2]);
            GameObject gameObject = Instantiate(cubeBlack, map.transform);
            gameObject.transform.position = avgPosition;
            gameObject.transform.localScale = new Vector3(1, 1, scaleZ);
            gameObject.transform.tag = mapTag;
            gameObject.layer = mapLayer;
            cubeCount++;
        }
        else
        {
            Debug.Log("pos.length is wrong!");
        }
    }

    private void MakeCube()
    {
        int[] i = new int[3];
        for (i[0] = 0; i[0] < size; i[0]++)
        {
            for (i[1] = 0; i[1] < size; i[1]++)
            {
                float[] firstPosition = new float[3];
                int count = 0;
                for (i[2] = 0; i[2] < size; i[2]++)
                {
                    if (i[2] == size - 1 && mapData[i[0], i[1], i[2]] == true)
                    {
                        float[] temp = new float[3];
                        count++;
                        for (int ip = 0, len = temp.Length; ip < len; ip++)
                        {
                            if (count == 1)
                                temp[ip] = i[ip];
                            else
                                temp[ip] = (i[ip] + firstPosition[ip]) / 2;
                        }
                        MakeCube(temp, count);
                        break;
                    }
                    if (mapData[i[0], i[1], i[2]] == true)
                    {
                        if (count == 0)
                        {
                            for (int ip = 0, len = firstPosition.Length; ip < len; ip++)
                            {
                                firstPosition[ip] = i[ip];
                            }
                        }
                        count++;
                    }
                    else if (count != 0)
                    {
                        float[] temp = new float[3];
                        temp[0] = (i[0] + firstPosition[0]) / 2;
                        temp[1] = (i[1] + firstPosition[1]) / 2;
                        temp[2] = (i[2] + firstPosition[2] - 1) / 2;
                        MakeCube(temp, count);
                        count = 0;
                    }
                }
            }
        }
    }

    const int center = size / 2;
    const float random = 0.6f;
    const int dirCount = 6;
    const int dimCount = 3;
    readonly int[,] dir = new int[dirCount, dimCount]
    {
           { 0, 1, 0 } ,
           { 0, -1, 0 },
           { 1, 0, 0 },
           { -1, 0, 0 },
           { 0, 0, 1 },
           { 0, 0, -1 }
    };

    Queue queue = new Queue();

    private bool CheckRange(int[] p)
    {
        for (int i = 0; i< dimCount; i++)
        {
            if (p[i] <= 0 || p[i] >= size - 1)
            {
                return false;
            }
        }
        return true;
    }

    private bool IsOkToEnter(int[] p)
    {
        if (CheckRange(p) && mapData[p[0], p[1], p[2]])
        {
            for (int i = 0; i < dirCount; i++)
            {
                if (i == p[dimCount] + (p[dimCount] % 2 == 0 ? 1 : -1))
                {
                    continue;
                }
                else if (!mapData[p[0] + dir[i, 0], p[1] + dir[i, 1], p[2] + dir[i, 2]])
                {
                    return false;
                }
            }
            return true;
        }
        else return false;
    }

    private int[] GetNewPoint(int[] p)
    {
        int[] temp = new int[4];
        if (p.Length != 4)
        {
            Debug.Log("Point is wrong!");
            return new int[4] { -1, -1, -1, -1 };
        }
        temp[3] = p[3];
        for(int i = 0; i < dimCount; i++)
        {
            temp[i] = dir[p[3], i] + p[i];
        }
        return temp;
    }

    public List<Vector3> endList = new List<Vector3>();

    private void MakeWay()
    {
        int[] temp = new int[4];
        int[] temp1 = new int[4];
        while (queue.Count > 0)
        {
            bool flag = true;
            temp = (int[])queue.Dequeue();
            temp1 = GetNewPoint(temp);
            if (IsOkToEnter(temp1))
            {
                queue.Enqueue(temp1);
                mapData[temp1[0], temp1[1], temp1[2]] = false;
                smallCubeCount--;
                flag = false;
            }
            if (Random.value < random)
            {
                int count = (int)(Random.value * 3) + 1;
                int t = (int)(Random.value * 6);
                temp[dimCount] = (t == temp[dimCount] ? (t + 1) % dirCount : t % dirCount);
                while (count-- > 0)
                {
                    temp1 = GetNewPoint(temp);
                    if (IsOkToEnter(temp1))
                    {
                        queue.Enqueue(temp1);
                        mapData[temp1[0], temp1[1], temp1[2]] = false;
                        smallCubeCount--;
                        flag = false;
                    }
                    temp[dimCount] = (temp[dimCount] + 1) % 6;
                }
            }
            if (flag)
            {
                Vector3 tt = new Vector3(temp[0], temp[1], temp[2]);
                endList.Add(tt);
            }
        }
    }

    bool isFirst = true;
    public Vector3 startPlace;

    private void InitMapData()
    {
        if (isFirst)
        {
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    for (int k = 0; k < size; k++)
                        mapData[i, j, k] = true;

            isFirst = false;
            int[] temp = new int[4] { center, center, 1, 4 };
            queue.Enqueue(temp);
            mapData[center, center, 1] = false;
            startPlace = new Vector3(center, center, 1);
            MakeWay();
        }
        else
        {
            
        }
    }

    private void Awake()
    {
        map = GameObject.Find("Map");
        mapTag = map.gameObject.transform.tag;
        mapLayer = map.gameObject.layer;
        InitMapData();
        MakeCube();
        //map.transform.Rotate(new Vector3(90, 0, 0), Space.World);
        Debug.Log((float)smallCubeCount / (size * size * size));
        Debug.Log(cubeCount);
    }
}
