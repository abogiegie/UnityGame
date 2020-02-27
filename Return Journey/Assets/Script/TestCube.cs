using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCube : MonoBehaviour
{
    GameControl gameControl;
    void Start()
    {
        gameControl = GameObject.FindWithTag("GameController").GetComponent<GameControl>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            gameControl.backpack.GetComponent<Backpack>().AddGood("Steel");
        }
    }
}
