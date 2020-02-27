using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buttons : MonoBehaviour
{
    public void Backpack()
    {
        GameControl gameControl = GameObject.FindWithTag("GameController").GetComponent<GameControl>();
        gameControl.Backpack();
    }

    public void Help()
    {
        GameControl gameControl = GameObject.FindWithTag("GameController").GetComponent<GameControl>();
        gameControl.Help();
    }

    public void News()
    {
        GameControl gameControl = GameObject.FindWithTag("GameController").GetComponent<GameControl>();
        gameControl.News();
    }

    public void Compose()
    {
        GameControl gameControl = GameObject.FindWithTag("GameController").GetComponent<GameControl>();
        gameControl.Compose();
    }

    public void MainMenu()
    {
        GameControl gameControl = GameObject.FindWithTag("GameController").GetComponent<GameControl>();
        gameControl.MainMenu();
    }
}
