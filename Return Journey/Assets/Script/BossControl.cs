using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossControl : MonoBehaviour
{
    GameObject gameControl;
    GameObject wall;
    GameObject music;
    public GameObject bossMusic;

    private void Awake()
    {
        gameControl = GameObject.FindWithTag("GameController");
        wall = GameObject.Find("Wall");
        music = GameObject.Find("MusicPlayer");
        bossMusic = GameObject.Find("BossMusic");
    }

    public void DestroyWall()
    {
        Destroy(wall);
        music.SetActive(true);
        gameControl.GetComponent<PartControl>().word = new string[]
        {
            "宁肯普：终于打死了这个怪物，也拿到了电池，这下我应该可以回家了，想想有点小激动呢。"
        };
        gameControl.GetComponent<PartControl>().ShowWord();
    }

    public void SetMusic(bool isOpen)
    {
        bossMusic.SetActive(isOpen);
    }
}
