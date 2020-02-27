using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    GameObject gameControl;

    private void Awake()
    {
        gameControl = GameObject.FindWithTag("GameController");
    }

    public void GameStart()
    {
        SceneManager.LoadScene("Scenes/Part1");
        PlayerPrefs.DeleteAll();
    }
    public void GameContinue()//仅仅是到达之前的关卡
    {
        if (PlayerPrefs.HasKey("part"))
        {
            SceneManager.LoadScene("Scenes/Part" + (PlayerPrefs.GetInt("part") + 1).ToString());
        }
    }
    public void GameExit()
    {
        Application.Quit();
    }
    public void SaveGame()
    {
        if(gameControl == null)
        {
            gameControl = GameObject.FindWithTag("GameController");
        }
        //清除之前的存储信息
        PlayerPrefs.DeleteAll();
        //储存背包信息
        Backpack backpack = gameControl.GetComponent<GameControl>().backpack.GetComponent<Backpack>();
        foreach(GameObject good in backpack.goodObject)
        {
            PlayerPrefs.SetInt(good.name, backpack.QueryGoodCount(good.name));
        }
        PlayerPrefs.SetInt("money", backpack.money);
        //储存时间信息
        PlayerPrefs.SetInt("date", gameControl.GetComponent<GameControl>().GetDate());
        PlayerPrefs.SetFloat("time", gameControl.GetComponent<GameControl>().GetTime());
        //储存飞船的剩余修理量
        if(gameControl.GetComponent<GameControl>().UFO != null)
        {
            PlayerPrefs.SetInt("UFOHP", gameControl.GetComponent<GameControl>().UFO.GetComponent<UFOControl>().GetHP());
        }
        //储存游戏关卡数
        PlayerPrefs.SetInt("part", gameControl.GetComponent<PartControl>().part);
        //清除地面上的所有可拾取物品
        foreach(GameObject apple in GameObject.FindGameObjectsWithTag("Apple"))
        {
            Destroy(apple);
        }
        foreach (GameObject stone in GameObject.FindGameObjectsWithTag("Stone"))
        {
            Destroy(stone);
        }
        foreach (GameObject ironOre in GameObject.FindGameObjectsWithTag("IronOre"))
        {
            Destroy(ironOre);
        }
        foreach(GameObject meat in GameObject.FindGameObjectsWithTag("Meat"))
        {
            Destroy(meat);
        }
        foreach (GameObject wood in GameObject.FindGameObjectsWithTag("Wood"))
        {
            Destroy(wood);
        }
    }
    public void LoadGame()
    {
        if (gameControl == null)
        {
            gameControl = GameObject.FindWithTag("GameController");
        }
        if (gameControl.GetComponent<PartControl>().isGameOver)
        {
            Restart();
        }
        //清空背包信息
        Backpack backpack = gameControl.GetComponent<GameControl>().backpack.GetComponent<Backpack>();
        backpack.DeleteAll();
        //载入背包信息
        foreach (GameObject good in backpack.goodObject)
        {
            if (PlayerPrefs.HasKey(good.name))
            {
                if (PlayerPrefs.GetInt(good.name) != 0)
                {
                    backpack.AddGood(good.name, PlayerPrefs.GetInt(good.name), false);
                }
            }
        }
        if (PlayerPrefs.HasKey("money"))
        {
            backpack.money = PlayerPrefs.GetInt("money");
            backpack.ShowMoney();
        }
        //载入时间信息
        if (PlayerPrefs.HasKey("date"))
        {
            gameControl.GetComponent<GameControl>().SetDate(PlayerPrefs.GetInt("date"));
        }
        if (PlayerPrefs.HasKey("time"))
        {
            gameControl.GetComponent<GameControl>().SetTime(PlayerPrefs.GetFloat("time"));
        }
        //载入飞船剩余修理量
        if(PlayerPrefs.HasKey("UFOHP") && gameControl.GetComponent<GameControl>().UFO != null)
        {
            gameControl.GetComponent<GameControl>().UFO.GetComponent<UFOControl>().SetHP(PlayerPrefs.GetInt("UFOHP"));
        }
        GameObject player = GameObject.FindWithTag("Player");
        if((player.transform.position - player.GetComponent<PlayerControl>().startPosition).magnitude >= 15)
        {
            player.GetComponent<PlayerControl>().Return();
        }
    }
    public void BackGame()
    {
        if (gameControl == null)
        {
            gameControl = GameObject.FindWithTag("GameController");
        }
        gameControl.GetComponent<GameControl>().MainMenu();
    }

    public void Restart()
    {
        if (gameControl == null)
        {
            gameControl = GameObject.FindWithTag("GameController");
        }
        int part = gameControl.GetComponent<PartControl>().part;
        SceneManager.LoadScene("Scenes/Part" + (part + 1).ToString());
    }

    public void Prompt()
    {
        if (gameControl == null)
        {
            gameControl = GameObject.FindWithTag("GameController");
        }
        gameControl.GetComponent<GameControl>().Prompt("此关禁用此功能");
    }
}
