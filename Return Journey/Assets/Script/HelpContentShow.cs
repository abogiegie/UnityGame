using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HelpContentShow : MonoBehaviour
{
    private bool titleVisiable = true;
    private bool contentVisiable = false;

    private Text title;
    private Text content;

    private string titleStr;
    private string contentStr;

    public void WakeUp()
    {
        title = this.transform.GetComponentsInChildren<Text>()[0];
        content = this.transform.GetComponentsInChildren<Text>()[1];
        titleStr = title.text;
        contentStr = content.text;
        content.text = null;
    }

    public void ShowContent()
    {
        if (titleVisiable)
        {
            titleVisiable = false;
            contentVisiable = true;
            title.text = null;
            content.text = contentStr;
        }
        else if (contentVisiable)
        {
            titleVisiable = true;
            contentVisiable = false;
            title.text = titleStr;
            content.text = null;
        }
        else
        {
            titleVisiable = true;
            contentVisiable = false;
            title.text = titleStr;
            content.text = null;
        }
    }
}
