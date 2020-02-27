using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Backpack : MonoBehaviour
{
    public ScrollRect scrollRect;

    private int selectType;
    public int selectGood;
    public List<GameObject> goodType = new List<GameObject>();
    private List<List<GoodData>> list = new List<List<GoodData>>();
    private List<Transform> deleteList = new List<Transform>();//用于清除当前物品栏内容

    public Slider slider;//容量指示条
    public int capacity = 0;//当前容量
    private int maxCapacity = 500;//最大容量

    private List<GoodData> allGoods = new List<GoodData>();
    private List<GoodData> equipment = new List<GoodData>();
    private List<GoodData> material = new List<GoodData>();
    private List<GoodData> prop = new List<GoodData>();
    public List<GameObject> goodObject = new List<GameObject>();
    public int money;

    public GameObject player;
    public GameObject gameControl;
    public Text moneyText;

    public List<GameObject> propPrefabList;

    void Awake()
    {
        gameControl = GameObject.FindWithTag("GameController");
        player = GameObject.FindWithTag("Player");
        selectType = (int)showType.All;
        list.Add(allGoods);
        list.Add(equipment);
        list.Add(material);
        list.Add(prop);
        ShowGoods();
        ShowMoney();
    }

    public void ShowMoney()
    {
        moneyText.text = "Do币:" + money.ToString();
    }

    public void DropProp(string name)
    {
        foreach(GameObject i in propPrefabList)
        {
            if(name == i.tag)
            {
                GameObject.Instantiate(i, player.transform.position, player.transform.rotation);
            }
        }
    }

    public void ShowGoods()
    {
        foreach(Transform child in this.scrollRect.content.transform)//清除当前物品栏内容
        {
            deleteList.Add(child);
        }
        for(int i = 0; i < deleteList.Count; i++)
        {
            Destroy(deleteList[i].gameObject);
        }
        deleteList.Clear();


        for(int i = 0; i < list[selectType].Count; i++)//添加内容
        {
            foreach(GameObject good in goodObject)
            {
                if(list[selectType][i].name == good.name)
                {
                    GameObject go = GameObject.Instantiate(good, this.scrollRect.content);
                    go.GetComponent<PropUse>().player = this.player;//传递玩家的GameObject
                    go.GetComponent<PropUse>().backpack = this.gameObject;//传递背包的Object
                    go.GetComponentInChildren<Text>().text = list[selectType][i].count.ToString();//物品数量数据
                }
            }
        }
        UpdateSlider();
    }

    private void UpdateSlider()
    {
        slider.value = ((float)capacity) / ((float)maxCapacity);
    }

    public int QueryGoodCount(string name)
    {
        foreach(GoodData good in allGoods)
        {
            if(good.name == name)
            {
                return good.count;
            }
        }
        return 0;
    }

    public bool AddGood(string name,int count = 1, bool promp = true)//promp==false不提示
    {
        GameObject good = null;
        foreach (GameObject i in goodObject)//查找物品对象的GameObject
        {
            if (i.name == name)
            {
                good = i;
                break;
            }
        }
        showType type = good.GetComponent<PropUse>().type;//获取物品信息
        int weight = good.GetComponent<PropUse>().weight;

        if (capacity + weight * count > maxCapacity)
        {
            gameControl.GetComponent<GameControl>().Prompt("背包已满", false);
            return false;
        }
        int index = IsGoodExist(name, type);
        if (index != -1)//背包中物品已经存在
        {
            if (type == showType.Equipment)//存在该武器
            {
                if (promp)
                {
                    gameControl.GetComponent<GameControl>().Prompt("已有" + name, false);
                }
                return false;
            }
            list[(int)type][index].count += count;//增加部分物品栏的物品数
            if (type != showType.All)
            {
                index = IsGoodExist(name, showType.All);
                list[0][index].count += count;//所有物品栏的物品数
            }
            capacity += weight * count;//背包重量增加
        }
        else//背包中物品不存在
        {
            list[(int)type].Add(new GoodData(name, count));//增加部分物品栏的新物品
            list[0].Add(new GoodData(name, count));//所有物品栏的新物品
            capacity += weight * count;//背包重量增加
        }
        if ((int)type == selectType || selectType == 0)//增加的物品与显示的物品栏属于同类
        {
            ShowGoods();
            if (promp)
            {
                gameControl.GetComponent<GameControl>().Prompt("获得" + name + "*" + count, false);
            }
        }
        UpdateSlider();
        return true;
    }

    public void DeleteAll()//清除所有背包内容
    {
        for(int i = 0; i < 4; i++)
        {
            list[i].Clear();
        }
    }

    public bool DeleteGood(string name ,int count = 1)//移除物品
    {
        GameObject good = null;
        foreach (GameObject i in goodObject)//查找物品对象的GameObject
        {
            if (i.name == name)
            {
                good = i;
                break;
            }
        }
        showType type = good.GetComponent<PropUse>().type;
        int weight = good.GetComponent<PropUse>().weight;

        int index = IsGoodExist(name, type);
        if (index != -1 && list[(int)type][index].count >= count)//物品已经存在，且数量足够
        {
            list[(int)type][index].count -= count;//减少部分物品栏的物品数
            if(list[(int)type][index].count == 0)//数量为0移除
            {
                list[(int)type].Remove(list[(int)type][index]);
            }
            if (type != showType.All)
            {
                index = IsGoodExist(name, showType.All);
                list[0][index].count -= count;//所有物品栏的物品数
                if(list[0][index].count == 0)//数量为0移除
                {
                    list[0].Remove(list[0][index]);
                }
            }
            capacity -= weight * count;//背包重量减少
        }
        else
        {
            gameControl.GetComponent<GameControl>().Prompt(name + "数量不足", false);
            return false;
        }
        if ((int)type == selectType || selectType == 0)//减少的物品与显示的物品栏属于同类
        {
            ShowGoods();
        }
        UpdateSlider();
        return true;
    }

    private int IsGoodExist(string name,showType type)//检查物品中在物品实例列表中的索引
    {
        for (int i = 0; i < list[(int)type].Count; i++)
        {
            if(name == list[(int)type][i].name)
            {
                return i;
            }
        }
        return -1;
    }

    public bool ChangeMoney(int count, bool promp = true)
    {
        money += count;
        if (money < 0)
        {
            money -= count;
            gameControl.GetComponent<GameControl>().Prompt("Do币不足", false);
            return false;
        }
        else if(money > 0 && promp)
        {
            gameControl.GetComponent<GameControl>().Prompt("获得" + count.ToString() + "Do币");
        }
        ShowMoney();
        return true;
    }
    public void SelectAll()
    {
        selectType = (int)showType.All;
        selectGood = 0;
        ShowGoods();
    }
    public void SelectEquipment()
    {
        selectType = (int)showType.Equipment;
        selectGood = 0;
        ShowGoods();
    }
    public void SelectMaterial()
    {
        selectType = (int)showType.Material;
        selectGood = 0;
        ShowGoods();
    }
    public void SelectProp()
    {
        selectType = (int)showType.Prop;
        selectGood = 0;
        ShowGoods();
    }
}
[System.Serializable]
public class GoodData
{
    public int count;//数量
    public string name;//名字

    public GoodData(string name ,int count)
    {
        this.name = name;
        this.count = count;
    }
}
 public enum showType
{
    All,
    Equipment,
    Material,
    Prop
}