using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PartControl : MonoBehaviour
{
    private string[] partNumber = new string[] { "一", "二", "三", "四" };
    public string[] word;

    private bool gameStart = false;

    public int part = 0;
    private int index = 0;

    private bool partOver;

    public GameObject partText;
    private Text talkText;

    //part1
    private GameObject firstEnemy;
    private GameObject backpack;


    private void Start()
    {
        talkText = this.GetComponent<GameControl>().talk.GetComponentInChildren<Text>();
        switch (part)
        {
            case 0:
                Part1();
                break;
            case 1:
                Part2();
                break;
            case 2:
                Part3();
                break;
        }
    }

    private void Part1()
    {
        firstEnemy = GameObject.FindWithTag("FirstEnemy");
        ShowPart();
        ShowWord();
    }

    private void Part2()
    {
        ShowPart();
        ShowWord();
    }

    private void Part3()
    {
        ShowPart();
        ShowWord();
    }

    private void Part4()
    {
        part = 3;
        ShowPart();
        ShowWord();
    }

    public void SetPartOver()
    {
        switch (part)
        {
            case 0:
                part++;
                this.GetComponent<StartGame>().SaveGame();
                SceneManager.LoadScene("Scenes/Part2");
                break;
            case 1:
                part++;
                this.GetComponent<StartGame>().SaveGame();
                SceneManager.LoadScene("Scenes/Part3");
                break;
            case 2:
                GameWin();
                break;
        }
    }

    public void IndexReduce()
    {
        if(index > 0)
        {
            index--;
        }
    }

    public void ShowWord()
    {
        if (!this.GetComponent<GameControl>().GetTalkVisiable())
        {
            this.GetComponent<GameControl>().OpenTalk();
        }
        if (index < word.Length)
        {
            talkText.text = word[index];//显示对话内容
            this.GetComponent<GameControl>().AddNews(word[index]);//把对话内容存储到消息记录中
            index++;
        }
        else
        {
            TalkOver();
            if (part == 0)
            {
                if (gameStart == false)
                {
                    if (firstEnemy != null)
                    {
                        firstEnemy.GetComponent<FirstEnemy>().SetSpeed(6);
                        gameStart = true;
                    }
                }
            }
        }
    }
    private bool isFirstTalk = true;
    private void TalkOver()
    {
        if (isFirstTalk)
        {
            this.GetComponent<StartGame>().LoadGame();
            if(part == 0)
            {
                this.GetComponent<GameControl>().backpack.GetComponent<Backpack>().AddGood("Meat", 1, false);
            }
            else if(part == 2)
            {
                this.GetComponent<GameControl>().backpack.GetComponent<Backpack>().AddGood("Arrow", 30, false);
                this.GetComponent<GameControl>().backpack.GetComponent<Backpack>().AddGood("Meat", 10, false);
            }
            isFirstTalk = false;
        }
        index = 0;
        if (this.GetComponent<GameControl>().GetTalkVisiable())
        {
            this.GetComponent<GameControl>().CloseTalk();
        }
        talkText.text = "";
        word = new string[] { "点击以关闭对话框" };
        if (part == 1 && this.GetComponent<GameControl>().villageHead.transform.tag != "Villager")
        {
            this.GetComponent<GameControl>().villageHead.transform.tag = "Villager";
        }
    }

    private void ShowPart()
    {
        GameObject text =  GameObject.Instantiate(partText);
        text.GetComponentInChildren<Text>().text = "第" + partNumber[part] + "幕";
        Destroy(text, 2);
    }

    public void GameWin()
    {
        GameObject text = GameObject.Instantiate(partText);
        text.GetComponentInChildren<Text>().text = "游戏胜利!!!!";
        Destroy(text, 2);
        word = new string[]
        {
            "你最后顺利地回到了村庄，和村民们告别，重新踏上了旅程。"
        };
        ShowWord();
    }

    public bool isGameOver = false;

    public void GameOver()
    {
        isGameOver = true;
        GameObject text = GameObject.Instantiate(partText);
        text.GetComponentInChildren<Text>().text = "游戏失败";
        Destroy(text, 2);
        PlayerControl player = GameObject.FindWithTag("Player").GetComponent<PlayerControl>();
        player.GetDamage(1000);
        word = new string[]
        {
            "请按esc键打开主菜单重新开始游戏。"
        };
        ShowWord();
    }
}
