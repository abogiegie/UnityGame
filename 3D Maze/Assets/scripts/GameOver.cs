using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOver : MonoBehaviour
{
    private GameObject gg;
    private Text ggText;
    private bool UIIsOn = false;

    public void ShowMenu()
    {
        if(gg != null)
        {
            ggText.text = "主菜单";
            UIIsOn = !UIIsOn;
            gg.SetActive(UIIsOn);
        }
    }

    private void Awake()
    {
        gg = GameObject.Find("GameOver");
        if(gg != null)
        {
            ggText = gg.GetComponentInChildren<Text>();
            gg.SetActive(false);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Scenes/Game");
    }

    public void Win()
    {
        if(gg != null)
        {
            ggText.text = "游戏胜利!";
            gg.SetActive(true);
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Defeat()
    {
        if(gg != null)
        {
            ggText.text = "游戏失败!";
            gg.SetActive(true);
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        gameObject.GetComponent<EnemyMaker>().DelAllEnemy();
    }

    public void Exit()
    {
        Application.Quit();
    }
}
