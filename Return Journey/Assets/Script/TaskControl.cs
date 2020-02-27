using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskControl : MonoBehaviour
{
    public GameObject gameControl;
    //判断任务是否存在
    private GameObject wood;
    private GameObject stone;
    private GameObject ironOre;
    private GameObject apple;

    //任务的prefab
    public GameObject woodTask;
    public GameObject stoneTask;
    public GameObject ironOreTask;
    public GameObject appleTask;

    //任务列表存放的地方
    public ScrollRect scrollRect;

    private void Awake()
    {
        gameControl = GameObject.FindWithTag("GameController");
    }

    private void UpdateWoodTask()
    {
        if (wood == null)
        {
            wood = GameObject.Instantiate(woodTask, this.scrollRect.content);
            wood.GetComponent<GetGood>().backpack = gameControl.GetComponent<GameControl>().backpack;
        }
    }

    private void UpdateStoneTask()
    {
        if (stone == null)
        {
            stone = GameObject.Instantiate(stoneTask, this.scrollRect.content);
            stone.GetComponent<GetGood>().backpack = gameControl.GetComponent<GameControl>().backpack;
        }
    }

    private void UpdateIronOreTask()
    {
        if (ironOre == null)
        {
            ironOre = GameObject.Instantiate(ironOreTask, this.scrollRect.content);
            ironOre.GetComponent<GetGood>().backpack = gameControl.GetComponent<GameControl>().backpack;
        }
    }

    private void UpdateAppleTask()
    {
        if (apple == null)
        {
            apple = GameObject.Instantiate(appleTask, this.scrollRect.content);
            apple.GetComponent<GetGood>().backpack = gameControl.GetComponent<GameControl>().backpack;
        }
    }

    public void UpdateAllTask()
    {
        UpdateWoodTask();
        UpdateStoneTask();
        UpdateIronOreTask();
        UpdateAppleTask();
    }
}
