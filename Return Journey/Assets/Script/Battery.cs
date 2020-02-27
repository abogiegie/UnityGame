using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battery : MonoBehaviour
{
    GameObject gameControl;
    GameObject wall;
    GameObject boss;
    GameObject music;
    private void Awake()
    {
        gameControl = GameObject.FindWithTag("GameController");
        wall = GameObject.Find("Wall");
        wall.SetActive(false);
        boss = GameObject.Find("Boss");
        boss.SetActive(false);
        music = GameObject.Find("MusicPlayer");
        gameControl.GetComponent<GameControl>().boss = this.boss;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.tag == "Player")
        {
            gameControl.GetComponent<PartControl>().word = new string[]
            {
                "宁肯普:终于找到电池了，还好电池没有被那些蘑菇怪弄坏，赶快把它带回去修好飞船吧",
                "大蘑菇怪：首领果然没说错，只要他一走就肯定会有人过来抢我们东西，我在这已经设下了埋伏，你已经出不去了。",
                "宁肯普：糟糕，没想到竟然有埋伏，看啦一场恶战必不可免了。"
            };
            gameControl.GetComponent<GameControl>().Prompt("你已经获得了飞船电池");
            gameControl.GetComponent<GameControl>().getBattery = true;
            gameControl.GetComponent<PartControl>().ShowWord();
            wall.SetActive(true);
            boss.SetActive(true);
            music.SetActive(false);
            Destroy(this.gameObject);
        }
    }

}
