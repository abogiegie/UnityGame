using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Help : MonoBehaviour
{
    public ScrollRect scrollRect;
    public Button helpButton; 

    public List<HelpContent> helpContent = new List<HelpContent>();

    private void Start()
    {
        for(int i = 0; i < helpContent.Count; i++)
        {
            AddContent(i);
        }
    }

    public void AddContent(int number)
    {
        Button button =  GameObject.Instantiate(this.helpButton, this.scrollRect.content);
        button.GetComponentsInChildren<Text>()[0].text = helpContent[number].title;
        button.GetComponentsInChildren<Text>()[1].text = helpContent[number].content;
        button.GetComponent<HelpContentShow>().WakeUp();
    }
}
[System.Serializable]
public class HelpContent
{
    public string title;
    public string content;
}
