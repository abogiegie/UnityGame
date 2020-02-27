using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CaveControl : MonoBehaviour
{
    GameObject gameControl;

    private void Awake()
    {
        gameControl = GameObject.FindWithTag("GameController");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            if(gameControl.GetComponent<GameControl>().getBattery == true)
            {
                gameControl.GetComponent<PartControl>().SetPartOver();
            }
            else
            {
                gameControl.GetComponent<PartControl>().word = new string[]
                {
                    "宁肯普：难得这次蘑菇怪的大首领出去了，我必须要找到电池，不然不知道下一次机会是什么时候。"
                };
                gameControl.GetComponent<PartControl>().ShowWord();
            }
        }
    }

}
