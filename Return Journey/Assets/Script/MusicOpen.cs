using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicOpen : MonoBehaviour
{
    GameControl gameControl;
    GameObject musicPlayer;
    public GameObject ahah;
    public void Music()
    {
        if(gameControl == null)
        {
            gameControl = GameObject.FindWithTag("GameController").GetComponent<GameControl>();
        }
        if(musicPlayer == null)
        {
            musicPlayer = GameObject.Find("MusicPlayer");
        }
        musicPlayer.SetActive(this.GetComponent<Toggle>().isOn);
        if (gameControl.boss != null)
        {
            gameControl.boss.GetComponent<BossControl>().SetMusic(this.GetComponent<Toggle>().isOn);
        }
    }
}
