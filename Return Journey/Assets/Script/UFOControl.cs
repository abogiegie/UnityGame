using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UFOControl : MonoBehaviour
{
    private int HP = 100;
    public bool isFixedUp = false;
    GameObject gameControl;

    private void Awake()
    {
        gameControl = GameObject.FindWithTag("GameController");
    }

    public void Fix(int hp)
    {
        HP -= hp;
        if (HP <= 0)
        {
            isFixedUp = true;
        }
    }

    public int GetHP()
    {
        return this.HP;
    }
    public void SetHP(int HP)
    {
        this.HP = HP;
    }

    private void UFOFixUI()
    {
        Camera mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        mainCamera.canMouseMove = !mainCamera.canMouseMove;
        GameControl gameControl = this.gameControl.GetComponent<GameControl>();
        gameControl.isFixUFOVisiable = !gameControl.isFixUFOVisiable;
        gameControl.fixUFO.SetActive(gameControl.isFixUFOVisiable);
        gameControl.Mouse(!gameControl.isFixUFOVisiable);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            UFOFixUI();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            if (gameControl.GetComponent<GameControl>().isFixUFOVisiable)
            {
                UFOFixUI();
                if (isFixedUp)
                {
                    StartPart3();
                }
            }
            else
            {
                if (isFixedUp)
                {
                    StartPart3();
                }
            }
        }
    }

    private void StartPart3()
    {
        gameControl.GetComponent<PartControl>().word = new string[]
        {
           "宁肯普：飞船终于修好了，但是为什么启动不了啊。..........找来找去.............。",
           "宁肯普：欸？我的电池哪去了，这没有电池我也没办法回去呀，这该怎么办。我去找村长问问，他应该知道些什么。",
        };
        gameControl.GetComponent<PartControl>().ShowWord();
        gameControl.GetComponent<GameControl>().villageHead.GetComponent<VillagerControl>().VillageHeadWordChange(true);
        gameControl.GetComponent<GameControl>().hunter.SetActive(true);
    }
}
