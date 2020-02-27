using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DifficultySelect : MonoBehaviour
{
    GameObject gameControl;

    public void SelectEasy()
    {
        
        if (this.GetComponent<Toggle>().isOn)
        {
            if(gameControl == null)
            {
                gameControl = GameObject.FindWithTag("GameController");
            }
            gameControl.GetComponent<GameControl>().SetDifficulty(Difficulty.Easy);
        }
        
    }

    public void SelectMedium()
    {
        if (this.GetComponent<Toggle>().isOn)
        {
            if (gameControl == null)
            {
                gameControl = GameObject.FindWithTag("GameController");
            }
            gameControl.GetComponent<GameControl>().SetDifficulty(Difficulty.Medium);
        }

    }

    public void SelectHard()
    {
        if (this.GetComponent<Toggle>().isOn)
        {
            if (gameControl == null)
            {
                gameControl = GameObject.FindWithTag("GameController");
            }
            gameControl.GetComponent<GameControl>().SetDifficulty(Difficulty.Hard);
        }

    }
}
public enum Difficulty{
    Easy,
    Medium,
    Hard
}
