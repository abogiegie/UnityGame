using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropControl : MonoBehaviour
{
    GameControl gameControl;

    private void Awake()
    {
        gameControl = GameObject.FindWithTag("GameController").GetComponent<GameControl>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            gameControl.Prompt("点击鼠标右键即可拾取物品");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            if (Input.GetMouseButtonDown(1))
            {
                PickUp(other.gameObject);
            }
        }
    }

    private void PickUp(GameObject player)
    {
        player.GetComponent<PlayerControl>().gameControl.GetComponent<GameControl>().backpack.GetComponent<Backpack>().AddGood(this.transform.tag);
        Destroy(this.gameObject);
    }
}
