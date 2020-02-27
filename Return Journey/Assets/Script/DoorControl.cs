using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorControl : MonoBehaviour
{
    private GameObject gameControl;
    public int HP;
    public Vector3 doorMove;
    private bool isDoorOpen = false;
    private bool isFirstEnter = true;

    private void Awake()
    {
        gameControl = GameObject.FindWithTag("GameController");
    }

    private void CloseDoor()
    {
        if (isDoorOpen)
        {
            this.transform.position -= doorMove;
            isDoorOpen = false;
        }
    }
    private void OpenDoor()
    {
        if (!isDoorOpen)
        {
            this.transform.position += doorMove;
            isDoorOpen = true;
        }
    }

    public int Fix(int fixHP)
    {
        HP += fixHP;
        if (fixHP < 0)
        {
            gameControl.GetComponent<GameControl>().Prompt("门正在遭受攻击，请快回到村庄救援");
        }
        if (this.HP <= 0)
        {
            gameControl.GetComponent<PartControl>().GameOver();
            Destroy(this.gameObject, 3);
        }
        return HP;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Enemy")
        {
            CloseDoor();
        }
        if (other.transform.tag == "Player")
        {
            if (gameControl.GetComponent<PartControl>().part == 0 && isFirstEnter)
            {
                gameControl.GetComponent<PartControl>().word = new string[] { "卫兵：外来人，为了表示你对我们是友好的，给你一把剑，帮我们把那个敌人干掉，这样我们就放你进去", "打开背包，装备上短剑来攻击敌人吧" };
                gameControl.GetComponent<PartControl>().ShowWord();
                gameControl.GetComponent<GameControl>().backpack.GetComponent<Backpack>().AddGood("SmallSword");
                isFirstEnter = false;
            }
            else
            {
                other.GetComponent<PlayerControl>().gameControl.GetComponent<GameControl>().Prompt("点击鼠标右键开门", false);
            }
            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            if (Input.GetMouseButtonDown(1))
            {
                OpenDoor();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            CloseDoor();
        }
    }
}
