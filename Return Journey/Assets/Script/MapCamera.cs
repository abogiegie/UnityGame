using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCamera : MonoBehaviour
{
    public GameObject target;
    public Vector3 cameraMove = new Vector3(0, 300, 0);
    // Update is called once per frame
    void Update()
    {
        transform.position = target.transform.position + cameraMove;
    }
}
