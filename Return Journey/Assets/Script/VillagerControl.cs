using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VillagerControl : MonoBehaviour
{
    public List<string> word;//储存对话内容
    private int index = 0;
    public GameObject gameControl;
    Text talkText;

    private void Awake()
    {
        gameControl = GameObject.FindWithTag("GameController");
    }

    private void Start()//如果对话内容为空的话加上对话内容
    {
        talkText = gameControl.GetComponent<GameControl>().talk.GetComponentInChildren<Text>();
        if (word.Count == 0)
        {
            word.Add("你好，蓝色的小伙子"); 
        }
    }

    public void VillageHeadWordChange(bool i = false)//根据剧情改变对话内容
    {
        if (this.name == "VillageHead")
        {
            if (i)
            {
                word.Clear();
                word.Add("村长：你好，宁肯普，有什么事吗？");
                word.Add("宁肯普：村长您好，我终于修好了飞船，但是我飞船里面的电池不见了，没有他我的飞船就启动不了，请问您知道这个东西在哪吗？");
                word.Add("村长：他大概长什么样子?");
                word.Add("宁肯普：一个像方块一样的东西，一直闪着红光。");
                word.Add("村长：我好像没有在村子里见过这个东西。对了，我们村里的猎人回来了，你去问问他，或许他知道些什么");
                word.Add("宁肯普：谢谢村长。");
            }
            else
            {
                word.Clear();
                word.Add("村长：你好，我这里有些事情需要你帮忙一下");
            }
        }
    }

    public void Talk()
    {
        if (!gameControl.GetComponent<GameControl>().GetTalkVisiable())
        {
            gameControl.GetComponent<GameControl>().OpenTalk();
        }
        if (index < word.Count)
        {
            talkText.text = word[index];//显示对话内容
            gameControl.GetComponent<GameControl>().AddNews(word[index]);//把对话内容存储到消息记录中
            index++;
        }
        else
        {
            TalkOver();
            if(gameControl.GetComponent<PartControl>().part == 1)//对话完毕就会开启对应的交互界面
            {
                if (this.name == "VillageHead")
                {
                    Task();
                    VillageHeadWordChange();
                }
                else if (this.name == "RestaurantOwner")
                {
                    Restaurant();
                }
                else if (this.name == "SmithyOwner")
                {
                    Smithy();
                }
                else if (this.name == "PawnShopOwner")
                {
                    PawnShop();
                }
                else if (this.name == "Guard")
                {
                    Guard();
                }
                else if(this.name == "Hunter")
                {
                    gameControl.GetComponent<GameControl>().Prompt("请在六点之前收集好物资，为进入洞穴拿电池做好准备。");
                }
            }
            else if(gameControl.GetComponent<PartControl>().part == 0)
            {
                if (this.name == "VillageHead")
                {
                    gameControl.GetComponent<PartControl>().SetPartOver();
                }
            }
        }
    }

    private void Task()//打开或关闭交互界面，下面类似
    {
        Camera mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        mainCamera.canMouseMove = !mainCamera.canMouseMove;//鼠标移动不再影响视角移动
        GameControl gameControl = this.gameControl.GetComponent<GameControl>();
        gameControl.isTaskVisiable = !gameControl.isTaskVisiable;//界面的打开或关闭
        gameControl.task.SetActive(gameControl.isTaskVisiable);
        gameControl.Mouse(!gameControl.isTaskVisiable);//鼠标的显示或隐藏
    }

    private void Restaurant()//打开或关闭交互界面
    {
        Camera mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        mainCamera.canMouseMove = !mainCamera.canMouseMove;
        GameControl gameControl = this.gameControl.GetComponent<GameControl>();
        gameControl.isRestaurantVisiable = !gameControl.isRestaurantVisiable;
        gameControl.restaurant.SetActive(gameControl.isRestaurantVisiable);
        gameControl.Mouse(!gameControl.isRestaurantVisiable);
    }

    private void Smithy()//打开或关闭交互界面
    {
        Camera mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        mainCamera.canMouseMove = !mainCamera.canMouseMove;
        GameControl gameControl = this.gameControl.GetComponent<GameControl>();
        gameControl.isSmithyVisiable = !gameControl.isSmithyVisiable;
        gameControl.smithy.SetActive(gameControl.isSmithyVisiable);
        gameControl.Mouse(!gameControl.isSmithyVisiable);

    }

    private void PawnShop()//打开或关闭交互界面
    {
        Camera mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        mainCamera.canMouseMove = !mainCamera.canMouseMove;
        GameControl gameControl = this.gameControl.GetComponent<GameControl>();
        gameControl.isPawnShopVisiable = !gameControl.isPawnShopVisiable;
        gameControl.pawnShop.SetActive(gameControl.isPawnShopVisiable);
        gameControl.Mouse(!gameControl.isPawnShopVisiable);
    }

    private void Guard()//打开或关闭交互界面
    {
        Camera mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        mainCamera.canMouseMove = !mainCamera.canMouseMove;
        GameControl gameControl = this.gameControl.GetComponent<GameControl>();
        gameControl.isGuardVisiable = !gameControl.isGuardVisiable;
        gameControl.guard.SetActive(gameControl.isGuardVisiable);
        gameControl.Mouse(!gameControl.isGuardVisiable);
    }

    private void TalkOver()//关闭对话界面并且将对话框中的内容清除
    {
        index = 0;
        if (gameControl.GetComponent<GameControl>().GetTalkVisiable())
        {
            gameControl.GetComponent<GameControl>().CloseTalk();
        }
        talkText.text = "";
    }

    private void OnCollisionEnter(Collision collision)//触碰到就会说话
    {
        if (collision.transform.tag == "Player")
        {
            Talk();
        }
    }
    private void OnCollisionExit(Collision collision)//离开会关闭对话框并且关闭UI界面
    {
        if(collision.transform.tag == "Player")
        {
            TalkOver();
            if(this.name == "VillageHead" && gameControl.GetComponent<GameControl>().isTaskVisiable)
            {
                Task();
            }
            else if (this.name == "RestaurantOwner" && gameControl.GetComponent<GameControl>().isRestaurantVisiable)
            {
                Restaurant();
            }
            else if(this.name== "SmithyOwner"&& gameControl.GetComponent<GameControl>().isSmithyVisiable)
            {
                Smithy();
            }
            else if(this.name == "PawnShopOwner" && gameControl.GetComponent<GameControl>().isPawnShopVisiable)
            {
                PawnShop();
            }
            else if(this.name == "Guard"&& gameControl.GetComponent<GameControl>().isGuardVisiable)
            {
                Guard();
            }
        }
    }
}
