using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    GameObject player;
    private void Awake()
    {
        player = GameObject.Find("Player");
    }
    void Start()
    {
        transform.LookAt(player.transform);
    }
}
