using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameControl : MonoBehaviour
{
    public bool getBattery = false;
    private bool isMainMenuVisiable = false;
    private bool backpackVisiable = false;
    private bool helpVisiable = false;
    private bool newsVisiable = false;
    private bool composeVisiable = false;
    private bool talkVisiable = false;
    public bool isTaskVisiable = false;
    public bool isRestaurantVisiable = false;
    public bool isSmithyVisiable = false;
    public bool isPawnShopVisiable = false;
    public bool isGuardVisiable = false;
    public bool isFixUFOVisiable = false;
    private float time = 4*60;
    private int date = 0;
    private int hour = 8;
    private int minute = 0;
    private float dayTime = 12*60;
    public float radiusSun = 250;
    public bool isNight = false;
    private int weather = 0;//天气0为晴，1为阴

    public GameObject door;
    public GameObject archer;//守门的弓箭兵
    public GameObject UFO;
    public GameObject boss;

    public GameObject mainMenu;
    public GameObject portal;
    public GameObject hunter;
    public GameObject villageHead;
    public GameObject enemyMaker;
    public GameObject animalMaker;
    public GameObject backpack;
    public GameObject help;
    public GameObject news;
    public GameObject compose;
    public GameObject talk;
    public GameObject task;
    public GameObject guard;
    public GameObject fixUFO;
    public GameObject restaurant;
    public GameObject smithy;
    public GameObject pawnShop;
    public GameObject sun;
    public GameObject mainCamera;
    public GameObject promptBox;
    public GameObject canvas;
    public Text timeText;
    public Text weatherText;
    public Text newsText;

    private void Awake()//唤醒时获取对象
    {
        door = GameObject.FindWithTag("Door");
        sun = GameObject.FindWithTag("Sun");
        mainCamera = GameObject.FindWithTag("MainCamera");
        this.portal = GameObject.Find("Portal");
        this.UFO = GameObject.Find("UFO");
        canvas = GameObject.Find("MainCanvas");
        boss = GameObject.Find("Boss");
        foreach (GameObject ui in GameObject.FindGameObjectsWithTag("UI"))
        {
            switch (ui.name)
            {
                case "MainMenu":
                    mainMenu = ui;
                    break;
                case "Canvas":
                    canvas = ui;
                    break;
                case "BackPack":
                    backpack = ui;
                    break;
                case "Compose":
                    compose = ui;
                    break;
                case "Help":
                    help = ui;
                    break;
                case "News":
                    news = ui;
                    break;
                case "Talk":
                    talk = ui;
                    talk.SetActive(false);
                    break;
                case "Task":
                    task = ui;
                    break;
                case "Restaurant":
                    restaurant = ui;
                    break;
                case "Smithy":
                    smithy = ui;
                    break;
                case "PawnShop":
                    pawnShop = ui;
                    break;
                case "Guard":
                    guard = ui;
                    break;
                case "FixUFO":
                    fixUFO = ui;
                    break;
                case "TimeWeather":
                    foreach(Text text in ui.GetComponentsInChildren<Text>())
                    {
                        if (text.name == "Time")
                        {
                            timeText = text;
                        }
                        else if(text.name == "Weather")
                        {
                            weatherText = text;
                        }
                    }
                    break;
            }

        }
        foreach(GameObject villager in GameObject.FindGameObjectsWithTag("Villager"))
        {
            switch (villager.name)
            {
                case "Guard":
                    archer = villager;
                    break;
                case "Hunter":
                    hunter = villager;
                    hunter.SetActive(false);
                    break;
            }
        }

        Cursor.lockState = CursorLockMode.Locked;//鼠标锁定并消失
        Cursor.lockState = CursorLockMode.Confined;//鼠标限制在游戏中
        Cursor.visible = false;
    }

    void Start()//开始时关闭各种UI
    {
        if (mainMenu != null)
        {
            mainMenu.SetActive(false);
        }
        if (backpack != null)
        {
            backpack.SetActive(false);
        }
        if (compose != null)
        {
            compose.SetActive(false);
        }
        if (help != null)
        {
            help.SetActive(false);
        }
        if (news != null)
        {
            news.SetActive(false);
        }
        if (restaurant != null)
        {
            restaurant.SetActive(false);
        }
        if (smithy != null)
        {
            smithy.SetActive(false);
        }
        if (pawnShop != null)
        {
            pawnShop.SetActive(false);
        }
        if (guard != null)
        {
            guard.SetActive(false);
        }
        if (fixUFO != null)
        {
            fixUFO.SetActive(false);
        }
        if (task != null)
        {
            task.SetActive(false);
            task.GetComponent<TaskControl>().UpdateAllTask();
        }
        if (portal != null)
        {
            portal.SetActive(false);
        }
        SetDifficulty(Difficulty.Medium);
    }

    // Update is called once per frame
    void Update()
    {
        ButtonListen();
        ChangeTime();
        ChangeSun();
    }

    private void ButtonListen()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            MainMenu();//打开或者关闭主菜单
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            Backpack();//打开或者关闭背包
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            Help();//打开或者关闭帮助
        }
        if (Input.GetKeyDown(KeyCode.N))
        {
            News();//打开或者关闭消息记录
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            Compose();//打开或者关闭合成界面
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))//改变背包显示物品的种类
        {
            backpack.GetComponent<Backpack>().goodType[(int)showType.All].GetComponent<Toggle>().isOn = true;
            backpack.GetComponent<Backpack>().SelectAll();
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))//改变背包显示物品的种类
        {
            backpack.GetComponent<Backpack>().goodType[(int)showType.Equipment].GetComponent<Toggle>().isOn = true;
            backpack.GetComponent<Backpack>().SelectEquipment();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))//改变背包显示物品的种类
        {
            backpack.GetComponent<Backpack>().goodType[(int)showType.Material].GetComponent<Toggle>().isOn = true;
            backpack.GetComponent<Backpack>().SelectMaterial();
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))//改变背包显示物品的种类
        {
            backpack.GetComponent<Backpack>().goodType[(int)showType.Prop].GetComponent<Toggle>().isOn = true;
            backpack.GetComponent<Backpack>().SelectProp();
        }
    }

    public void MainMenu()//打开或者关闭主菜单
    {
        isMainMenuVisiable = !isMainMenuVisiable;
        if (backpackVisiable)//关闭背包界面
        {
            backpackVisiable = !backpackVisiable;
            backpack.SetActive(backpackVisiable);
        }
        if (helpVisiable)//关闭help界面
        {
            helpVisiable = !helpVisiable;
            help.SetActive(helpVisiable);
        }
        if (newsVisiable)//关闭消息记录界面
        {
            newsVisiable = !newsVisiable;
            news.SetActive(newsVisiable);
        }
        if (composeVisiable)//关闭合成界面
        {
            composeVisiable = !composeVisiable;
            compose.SetActive(composeVisiable);
        }
        mainMenu.SetActive(isMainMenuVisiable);
        Mouse(!isMainMenuVisiable);
        if (talkVisiable)
        {
            Mouse(!talkVisiable);
        }
        if(!isMainMenuVisiable)
        {
            this.GetComponent<PartControl>().IndexReduce();
        }
    }

    public int GetDate()
    {
        return this.date;
    }
    public float GetTime()
    {
        return this.time;
    }
    public void SetDate(int date)
    {
        this.date = date;
    }
    public void SetTime(float time)
    {
        this.time = time;
    }

    private void ChangeTime()
    {
        time += Time.deltaTime;
        minute = (int)(time * 2) % 60;
        hour = (int)time / 30;
        if (time > dayTime)
        {
            time -= dayTime;
            date++;
        }
        timeText.text = date.ToString() + "天" + hour.ToString() + "时" + minute.ToString() + "分";
        if (isNight == false && (hour >= 18 || hour < 6))//晚上
        {
            SetWeather();
            SetNight();
            if (this.GetComponent<PartControl>().part < 2)
            {
                enemyMaker.GetComponent<EnemyMaker>().Attack();//所有敌人攻向村庄
                enemyMaker.GetComponent<EnemyMaker>().maxEnemyCount++;//增加enemymaker制造敌人的最大数量
            }
        }
        else if(isNight == true && (hour >= 6 && hour < 18))//早上
        {
            SetWeather();
            SetNight();
            if (this.GetComponent<PartControl>().part < 2)
            {
                task.GetComponent<TaskControl>().UpdateAllTask();//更新村长任务
                animalMaker.GetComponent<AnimalMaker>().MakeAnimal();//制造动物
                enemyMaker.GetComponent<EnemyMaker>().MakeEnemy();//制造敌人
            }
            if (UFO.GetComponent<UFOControl>().isFixedUp)
            {
                portal.SetActive(true);
            }
        }
    }

    private void SetWeather()//改变天气，天气会与天空盒同步
    {
        weather = (int)(Random.value * 2);
        if (weather == 0)//晴天
        {
            weatherText.text = "晴天";
        }
        else//阴天
        {
            weatherText.text = "阴天";
        }
    }

    private void SetNight()//改变白天黑夜
    {
        isNight = !isNight;
        if (isNight)
        {
            sun.GetComponent<Light>().intensity = 0;
            mainCamera.GetComponent<Camera>().ChangeSkybox((SkyType)(2 + weather));//从两个晚上的天空盒选择一个
        }
        else
        {
            sun.GetComponent<Light>().intensity = 1;
            mainCamera.GetComponent<Camera>().ChangeSkybox((SkyType)(weather));//从两个白天的天空和选择一个
        }
    }

    private void ChangeSun()//改变平行光的位置
    {
        float angle = time / dayTime * 2 * Mathf.PI - 0.5f * Mathf.PI;
        sun.transform.SetPositionAndRotation(new Vector3(radiusSun * Mathf.Cos(angle), radiusSun * Mathf.Sin(angle), radiusSun * 0.3f), transform.rotation);
        sun.transform.LookAt(new Vector3(0, 0, 0));
    }

    public void Mouse(bool mouseLock)//设置鼠标的隐藏和显示
    {
        if (mouseLock)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    public void Backpack()//开启或关闭背包，对话框出现时强制关闭，以下类似
    {
        if (!talkVisiable)
        {
            backpackVisiable = !backpackVisiable;
            if (helpVisiable)//关闭help界面
            {
                helpVisiable = !helpVisiable;
                help.SetActive(helpVisiable);
            }
            if (newsVisiable)//关闭消息记录界面
            {
                newsVisiable = !newsVisiable;
                news.SetActive(newsVisiable);
            }
            if (composeVisiable)//关闭合成界面
            {
                composeVisiable = !composeVisiable;
                compose.SetActive(composeVisiable);
            }
            backpack.SetActive(backpackVisiable);
            Mouse(!backpackVisiable);
        }
    }

    public void Help()//开启或关闭帮助，对话框出现时强制关闭
    {
        if (!talkVisiable)
        {
            helpVisiable = !helpVisiable;
            if (backpackVisiable)
            {
                backpackVisiable = !backpackVisiable;
                backpack.SetActive(backpackVisiable);
            }
            if (newsVisiable)
            {
                newsVisiable = !newsVisiable;
                news.SetActive(newsVisiable);
            }
            if (composeVisiable)
            {
                composeVisiable = !composeVisiable;
                compose.SetActive(composeVisiable);
            }
            help.SetActive(helpVisiable);
            Mouse(!helpVisiable);
        }
    }

    public void News()//显示或隐藏历史消息
    {
        if (!talkVisiable)
        {
            newsVisiable = !newsVisiable;
            if (backpackVisiable)
            {
                backpackVisiable = !backpackVisiable;
                backpack.SetActive(backpackVisiable);
            }
            if (helpVisiable)
            {
                helpVisiable = !helpVisiable;
                help.SetActive(helpVisiable);
            }
            if (composeVisiable)
            {
                composeVisiable = !composeVisiable;
                compose.SetActive(composeVisiable);
            }
            news.SetActive(newsVisiable);
            Mouse(!newsVisiable);
        }
    }

    public void Compose()//显示或者隐藏合成框
    {
        if (!talkVisiable)
        {
            composeVisiable = !composeVisiable;
            if (backpackVisiable)
            {
                backpackVisiable = !backpackVisiable;
                backpack.SetActive(backpackVisiable);
            }
            if (helpVisiable)
            {
                helpVisiable = !helpVisiable;
                help.SetActive(helpVisiable);
            }
            if (newsVisiable)
            {
                newsVisiable = !newsVisiable;
                news.SetActive(newsVisiable);
            }
            compose.SetActive(composeVisiable);
            Mouse(!composeVisiable);
        }
    }

    private int newsCount = 0;

    public void AddNews(string content)//将一些有用的消息显示到消息记录中
    {
        ScrollRect scroll = news.GetComponentInChildren<ScrollRect>(); 
        Text text = GameObject.Instantiate(newsText, scroll.content);
        text.text = content;
        newsCount++;
        if (newsCount > 50)//多于50条信息将会删除
        {
            Destroy(scroll.GetComponentInChildren<Text>().gameObject);
            newsCount--;
        }
    }

    public void OpenTalk()//开启对话框，关闭背包帮助和消息
    {
        mainCamera.GetComponent<Camera>().canMouseMove = false;
        backpackVisiable = false;
        helpVisiable = false;
        newsVisiable = false;
        backpack.SetActive(false);
        help.SetActive(false);
        news.SetActive(false);
        talk.SetActive(true);
        talkVisiable = true;
        Mouse(false);
    }

    public void CloseTalk()//关闭对话框 
    {
        mainCamera.GetComponent<Camera>().canMouseMove = true;
        talk.SetActive(false);
        talkVisiable = false;
        Mouse(true);
    }

    public bool GetTalkVisiable()//获取talk窗口是否在显示
    {
        return talkVisiable;
    }

    private GameObject pom = null;

    public void Prompt(string promptContent, bool addNews = true)//提示框的显示
    {
        if (pom == null || pom.GetComponentInChildren<Text>().text != promptContent)//如果短时间内出现多个提示内容，则不会再次
        {
            pom = GameObject.Instantiate(promptBox, canvas.transform);
            pom.GetComponent<PromptBox>().SetText(promptContent);
            pom.GetComponent<PromptBox>().canvas = this.canvas;
        }
        if (addNews)//如果需要的话，就将对话内容增加到消息记录中
        {
            AddNews(promptContent);
        }
    }

    public void SetDifficulty(Difficulty difficulty)//设置难度
    {
        if(this.GetComponent<PartControl>().part == 1)
        {
            if(difficulty == Difficulty.Easy)
            {
                enemyMaker.GetComponent<EnemyMaker>().damage = 5;
                enemyMaker.GetComponent<EnemyMaker>().HP = 50;
                enemyMaker.GetComponent<EnemyMaker>().maxEnemyCount = 5 + date;
                Prompt("难度：容易");
            }
            else if(difficulty == Difficulty.Medium)
            {
                enemyMaker.GetComponent<EnemyMaker>().damage = 10;
                enemyMaker.GetComponent<EnemyMaker>().HP = 100;
                enemyMaker.GetComponent<EnemyMaker>().maxEnemyCount = 10 + date;
                Prompt("难度：中等");
            }
            else
            {
                enemyMaker.GetComponent<EnemyMaker>().damage = 15;
                enemyMaker.GetComponent<EnemyMaker>().HP = 200;
                enemyMaker.GetComponent<EnemyMaker>().maxEnemyCount = 20 + date;
                Prompt("难度：困难");
            }
        }
        if(this.GetComponent<PartControl>().part == 2)
        {
            if (difficulty == Difficulty.Easy)
            {
                foreach(GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    if (enemy.name != "Boss")
                    {
                        enemy.GetComponent<EnemyControl>().ChangeHP(50);
                        enemy.GetComponent<EnemyControl>().damage = 5;
                    }
                }
                boss.GetComponent<EnemyControl>().ChangeHP(500);
                boss.GetComponent<EnemyControl>().damage = 10;
                boss.GetComponent<EnemyControl>().speed = 3;
                Prompt("难度：容易");
            }
            else if (difficulty == Difficulty.Medium)
            {
                foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    if (enemy.name != "Boss")
                    {
                        enemy.GetComponent<EnemyControl>().ChangeHP(100);
                        enemy.GetComponent<EnemyControl>().damage = 10;
                    }
                }
                boss.GetComponent<EnemyControl>().ChangeHP(1000);
                boss.GetComponent<EnemyControl>().damage = 15;
                boss.GetComponent<EnemyControl>().speed = 3.5f;
                Prompt("难度：中等");
            }
            else
            {
                foreach (GameObject enemy in GameObject.FindGameObjectsWithTag("Enemy"))
                {
                    if (enemy.name != "Boss")
                    {
                        enemy.GetComponent<EnemyControl>().ChangeHP(200);
                        enemy.GetComponent<EnemyControl>().damage = 15;
                    }
                }
                boss.GetComponent<EnemyControl>().speed = 4;
                boss.GetComponent<EnemyControl>().damage = 20;
                boss.GetComponent<EnemyControl>().ChangeHP(1500);
                Prompt("难度：困难");
            }
        }
    }
}
